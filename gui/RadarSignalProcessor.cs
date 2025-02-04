using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class RadarSignalProcessor
    {

        #region "Events"

        public delegate void OnNewFormattedTimeSignalEventHandler(object sender, double[] signal, int antennaIndex);
        public event OnNewFormattedTimeSignalEventHandler? OnNewFormattedTimeSignal;

        public delegate void OnNewFrameSpectrumEventHandler(object sender, System.Numerics.Complex[] spectrum, int antennaIndex);
        public event OnNewFrameSpectrumEventHandler? OnNewFrameSpectrum;

        public delegate void OnNewEnergyOverTimeEventHandler(object sender, double energy, int antennaIndex);
        public event OnNewEnergyOverTimeEventHandler? OnNewEnergyOverTime;

        //public delegate void OnNewDBFOutputEventHandler(object sender, System.Numerics.Complex[,] dbfOutput);
        //public event OnNewDBFOutputEventHandler? OnNewDBFOutput;

        //public delegate void OnNew2DBFOutputEventHandler(object sender, System.Numerics.Complex[,] dbfOutputH, System.Numerics.Complex[,] dbfOutputV);
        //public event OnNew2DBFOutputEventHandler? OnNew2DBFOutput;

        //public delegate void OnNewTargetDetectedEventHandler(object sender, bool detected, double angle, double range);
        //public event OnNewTargetDetectedEventHandler? OnNewTargetDetected;

        //public delegate void OnNewDopplerFFTMatrixEventHandler(object sender, System.Numerics.Complex[,] dopplerFFTMatrix, int antennaIndex);
        //public event OnNewDopplerFFTMatrixEventHandler? OnNewDopplerFFTMatrix;

        public delegate void OnNewDopplerFFTMatrix3EventHandler(object sender, System.Numerics.Complex[,] dopplerFFTMatrixRx1, System.Numerics.Complex[,] dopplerFFTMatrixRx2, System.Numerics.Complex[,] dopplerFFTMatrixRx3);
        public event OnNewDopplerFFTMatrix3EventHandler? OnNewDopplerFFTMatrix3;

        #endregion

        private RadarConfiguration radarConfiguration;
        private int minBin = 0;
        private int maxBin = 16; // 0 to 5 meters (0.32cm per bin)

        private BackgroundRemover[] backgroundRemover;
        private CFAR[] gocafar;
        private DBF dbf;

        private bool[] somethingDetected;
        private int[] rangeOfMax;
        private double[] threshold;
        private SignalWindow window;
        private SignalWindow dopplerWindow;


        // Allocate only once (avoid to much reallocation) --> Might be done once in the constructor as well
        double[] timeBuffer;

        // Store matrix (range FFT per chirp)
        System.Numerics.Complex[,] rangeFFTMatrix;

        // Used to compute DBF and background
        System.Numerics.Complex[] spectrumAvg0;
        System.Numerics.Complex[] spectrumAvg1;
        System.Numerics.Complex[] spectrumAvg2;

        public RadarSignalProcessor(RadarConfiguration radarConfiguration)
        {
            this.radarConfiguration = radarConfiguration;
            backgroundRemover = new BackgroundRemover[radarConfiguration.AntennaCount];
            gocafar = new CFAR[radarConfiguration.AntennaCount];
            somethingDetected = new bool[radarConfiguration.AntennaCount];
            rangeOfMax = new int[radarConfiguration.AntennaCount];
            threshold = new double[radarConfiguration.AntennaCount];
            dbf = new DBF(32, 45, 0.5);
            window = new SignalWindow(SignalWindow.Type.TypeBlackmanHarris, radarConfiguration.SamplesPerChirp);
            dopplerWindow = new SignalWindow(SignalWindow.Type.TypeBlackmanHarris,
                radarConfiguration.ChirpsPerFrame);

            for (int i = 0; i < radarConfiguration.AntennaCount; ++i)
            {
                backgroundRemover[i] = new BackgroundRemover((radarConfiguration.SamplesPerChirp / 2) + 1);
                gocafar[i] = new CFAR(16, 32, 0.05);
                somethingDetected[i] = false;
                threshold[i] = 0.1;
            }

            timeBuffer = new double[radarConfiguration.SamplesPerChirp];

            int spectrumLen = (radarConfiguration.SamplesPerChirp / 2) + 1;

            // Store matrix (range FFT per chirp)
            rangeFFTMatrix = new System.Numerics.Complex[radarConfiguration.ChirpsPerFrame, spectrumLen];

            // Used to compute DBF and background
            spectrumAvg0 = new System.Numerics.Complex[spectrumLen];
            spectrumAvg1 = new System.Numerics.Complex[spectrumLen];
            spectrumAvg2 = new System.Numerics.Complex[spectrumLen];
        }

        public BackgroundFilterConfiguration getBackgroundFilterConfiguration()
        {
            return backgroundRemover[0].getConfiguration();
        }

        public void setBackgroundFilterConfiguration(BackgroundFilterConfiguration configuration)
        {
            for(int i = 0; i < radarConfiguration.AntennaCount; ++i)
                backgroundRemover[i].setConfiguration(configuration);
        }

        public void setThresholdValue(double value)
        {
            for(int i = 0; i < radarConfiguration.AntennaCount; ++i)
            {
                threshold[i] = value;
            }
        }

        private void sendFormattedTimeSignalEvent(double[] timeSignal, int antennaIndex)
        {
            if (OnNewFormattedTimeSignal != null)
            {
                double[] tmp = new double[timeSignal.Length];
                for(int i= 0; i < tmp.Length; ++i)
                {
                    tmp[i] = timeSignal[i];
                }
                OnNewFormattedTimeSignal.Invoke(this, tmp, antennaIndex);
            }
        }

        private void sendSpectrumEvent(System.Numerics.Complex[] spectrum, int antennaIndex)
        {
            if (OnNewFrameSpectrum != null)
            {
                System.Numerics.Complex[] tmp = new System.Numerics.Complex[spectrum.Length];
                for (int i = 0; i < tmp.Length; ++i)
                {
                    tmp[i] = spectrum[i];
                }
                OnNewFrameSpectrum.Invoke(this, tmp, antennaIndex);
            }
        }

        public double [] computeAsDBFS(System.Numerics.Complex[] spectrum)
        {
            double[] retval = new double[spectrum.Length];
            for (int i = 0; i < retval.Length; ++i)
            {
                retval[i] = (spectrum[i].Magnitude * 2) / window.getSum();
                retval[i] = 20 * Math.Log10(retval[i] / 1);
            }
            return retval;
        }


        /// <summary>
        /// Feed the processor with data
        /// /!\ in case more than one antenna, frame looks like:
        /// frame[0]: sample 0 of antenna 0
        /// frame[1]: sample 0 of antenna 1
        /// ...
        /// </summary>
        /// <param name="frame"></param>
        public void feedDopplerFFT(ushort[] frame)
        {
            int spectrumLen = (radarConfiguration.SamplesPerChirp / 2) + 1;

            System.Numerics.Complex[,]? dopplerFFTMatrixRx1 = null;
            System.Numerics.Complex[,]? dopplerFFTMatrixRx2 = null;
            System.Numerics.Complex[,]? dopplerFFTMatrixRx3 = null;


            // Extract per antenna
            for (int antennaIndex = 0; antennaIndex < radarConfiguration.AntennaCount; antennaIndex++)
            {
                // For each chirp within the frame
                for (int chirpIndex = 0; chirpIndex < radarConfiguration.ChirpsPerFrame; chirpIndex++)
                {
                    // Extract time signal of the chirp
                    int startIndex = chirpIndex * radarConfiguration.AntennaCount * radarConfiguration.SamplesPerChirp;
                    for (int sampleIndex = 0; sampleIndex < radarConfiguration.SamplesPerChirp; sampleIndex++)
                    {
                        int index = startIndex + sampleIndex * radarConfiguration.AntennaCount + antennaIndex;
                        timeBuffer[sampleIndex] = frame[index];
                    }

                    // Time buffer now contains the samples of one antenna during a chirp
                    // Scale between 0 and 1
                    ArrayUtils.scaleInPlace(timeBuffer, 1.0 / 4096.0);

                    // Compute the average
                    double average = ArrayUtils.getAverage(timeBuffer);

                    // Offset
                    ArrayUtils.offsetInPlace(timeBuffer, -average);

                    // Apply windows
                    window.applyInPlace(timeBuffer);

                    // Compute real FFT
                    // Size of spectrum is (SamplesPerChirp / 2) + 1
                    System.Numerics.Complex[] spectrum = FftSharp.FFT.ForwardReal(timeBuffer);

                    // Store it inside the matrix (will be used to compute FFT per bin and the average)
                    for (int freqIndex = 0; freqIndex < spectrum.Length; freqIndex++)
                    {
                        rangeFFTMatrix[chirpIndex, freqIndex] = spectrum[freqIndex];
                    }
                }

                // Send the last chirp samples (formatted)
                sendFormattedTimeSignalEvent(timeBuffer, antennaIndex);

                // [bin, velocity]
                System.Numerics.Complex[,] dopplerFFTMatrix = 
                    new System.Numerics.Complex[spectrumLen, radarConfiguration.ChirpsPerFrame];

                double maxAmplitude = double.NaN;
                int rangeForMaxAmplitude = 0;

                // Compute FFT for each frequency bin over time (chirp repetition time)
                System.Numerics.Complex[] binContent = new System.Numerics.Complex[radarConfiguration.ChirpsPerFrame];
                for (int freqIndex = 0; freqIndex < spectrumLen; freqIndex++)
                {
                    // Get the content for this frequency bin
                    for (int chirpIndex = 0; chirpIndex < radarConfiguration.ChirpsPerFrame; chirpIndex++)
                    {
                        binContent[chirpIndex] = rangeFFTMatrix[chirpIndex, freqIndex];
                    }

                    // Compute average and remove it (remove 0 speed)
                    System.Numerics.Complex avgComplex = ArrayUtils.getAverage(binContent);
                    ArrayUtils.offsetInPlace(binContent, -avgComplex);

                    dopplerWindow.applyInPlaceComplex(binContent);

                    // Get FFT (transform in place)
                    FftSharp.FFT.Forward(binContent);

                    // Get the doppler FFT for the bin
                    System.Numerics.Complex[] dopplerFFTForBin = FftShift(binContent);

                    // Copy and check for maximum
                    for (int i = 0; i < dopplerFFTForBin.Length; i++)
                    {
                        dopplerFFTMatrix[freqIndex, i] = dopplerFFTForBin[i];

                        double magnitude = dopplerFFTForBin[i].Magnitude;
                        if (double.IsNaN(maxAmplitude) || (magnitude > maxAmplitude))
                        {
                            maxAmplitude = magnitude;
                            rangeForMaxAmplitude = freqIndex;
                        }
                    }
                }

                // Store
                if (antennaIndex == 0) dopplerFFTMatrixRx1 = dopplerFFTMatrix;
                else if (antennaIndex == 1) dopplerFFTMatrixRx2 = dopplerFFTMatrix;
                else if (antennaIndex == 2) dopplerFFTMatrixRx3 = dopplerFFTMatrix;

                if (antennaIndex == (RadarConfiguration.ANTENNA_COUNT - 1))
                {
                    if (dopplerFFTMatrixRx1 == null
                        || dopplerFFTMatrixRx2 == null
                        || dopplerFFTMatrixRx3 == null)
                        return;

                    OnNewDopplerFFTMatrix3?.Invoke(this, dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
                }

                // Compute spectrum average
                for (int chirpIndex = 0; chirpIndex < radarConfiguration.ChirpsPerFrame; chirpIndex++)
                {
                    for (int freqIndex = 0; freqIndex < spectrumLen; freqIndex++)
                    {
                        if (antennaIndex == 0)
                        {
                            spectrumAvg0[freqIndex] += rangeFFTMatrix[chirpIndex, freqIndex];
                        }
                        else if (antennaIndex == 1)
                        {
                            spectrumAvg1[freqIndex] += rangeFFTMatrix[chirpIndex, freqIndex];
                        }
                        else if (antennaIndex == 2)
                        {
                            spectrumAvg2[freqIndex] += rangeFFTMatrix[chirpIndex, freqIndex];
                        }
                    }
                }
                for (int freqIndex = 0; freqIndex < spectrumLen; freqIndex++)
                {
                    if (antennaIndex == 0)
                    {
                        spectrumAvg0[freqIndex] /= radarConfiguration.ChirpsPerFrame;
                    }
                    else if (antennaIndex == 1)
                    {
                        spectrumAvg1[freqIndex] /= radarConfiguration.ChirpsPerFrame;
                    }
                    else if (antennaIndex == 2)
                    {
                        spectrumAvg2[freqIndex] /= radarConfiguration.ChirpsPerFrame;
                    }
                }

                // Send spectrum as event
                if (antennaIndex == 0) sendSpectrumEvent(spectrumAvg0, antennaIndex);
                else if (antennaIndex == 1) sendSpectrumEvent(spectrumAvg1, antennaIndex);
                else if (antennaIndex == 2) sendSpectrumEvent(spectrumAvg2, antennaIndex);

                OnNewEnergyOverTime?.Invoke(this, maxAmplitude, antennaIndex);
            }
        }

        private double convertDBFColToMeters(int colIndex)
        {
            System.Diagnostics.Debug.WriteLine("Col index: " + colIndex.ToString());
            double bandWidth = radarConfiguration.EndFrequency - radarConfiguration.StartFrequency;
            double celerity = 299792458;
            double slope = bandWidth / (radarConfiguration.SamplesPerChirp * (1 / radarConfiguration.SamplingRate));
            double fftLen = (radarConfiguration.SamplesPerChirp / 2) + 1;

            double fractionFs = colIndex / ((fftLen - 1) * 2);
            double freq = fractionFs * radarConfiguration.SamplingRate;
            double rangeMeters = (celerity * freq) / (2 * slope);
            System.Diagnostics.Debug.WriteLine("Col rangeMeters: " + rangeMeters.ToString());
            return rangeMeters;
        }

        private static System.Numerics.Complex[] FftShift(System.Numerics.Complex[] values)
        {
            int shiftBy = (values.Length + 1) / 2;

            System.Numerics.Complex[] values2 = new System.Numerics.Complex[values.Length];
            for (int i = 0; i < values.Length; i++)
                values2[i] = values[(i + shiftBy) % values.Length];

            return values2;
        }


        private System.Numerics.Complex[]? filteredBackgroundRx1;
        private System.Numerics.Complex[]? filteredBackgroundRx2;
        private System.Numerics.Complex[]? filteredBackgroundRx3;

        public void feedBackground(ushort[] frame)
        {
            // Allocate only once (avoid to much reallocation) --> Might be done once in the constructor as well
            double[] timeBuffer = new double[radarConfiguration.SamplesPerChirp];

            int spectrumLen = (radarConfiguration.SamplesPerChirp / 2) + 1;

            // Store matrix (range FFT per chirp)
            System.Numerics.Complex[,] rangeFFTMatrix = new System.Numerics.Complex[radarConfiguration.ChirpsPerFrame, spectrumLen];

            // Used to compute DBF and background
            System.Numerics.Complex[] spectrumAvg0 = new System.Numerics.Complex[spectrumLen];
            System.Numerics.Complex[] spectrumAvg1 = new System.Numerics.Complex[spectrumLen];
            System.Numerics.Complex[] spectrumAvg2 = new System.Numerics.Complex[spectrumLen];

            System.Numerics.Complex[,]? dopplerFFTMatrixRx1 = null;
            System.Numerics.Complex[,]? dopplerFFTMatrixRx2 = null;
            System.Numerics.Complex[,]? dopplerFFTMatrixRx3 = null;

            double coeff = 0.95;
            double a = 1 - coeff;
            double b = coeff;


            // Extract per antenna
            for (int antennaIndex = 0; antennaIndex < radarConfiguration.AntennaCount; antennaIndex++)
            {
                // For each chirp within the frame
                for (int chirpIndex = 0; chirpIndex < radarConfiguration.ChirpsPerFrame; chirpIndex++)
                {
                    // Extract time signal of the chirp
                    int startIndex = chirpIndex * radarConfiguration.AntennaCount * radarConfiguration.SamplesPerChirp;
                    for (int sampleIndex = 0; sampleIndex < radarConfiguration.SamplesPerChirp; sampleIndex++)
                    {
                        int index = startIndex + sampleIndex * radarConfiguration.AntennaCount + antennaIndex;
                        timeBuffer[sampleIndex] = frame[index];
                    }

                    // Time buffer now contains the samples of one antenna during a chirp
                    // Scale between 0 and 1
                    ArrayUtils.scaleInPlace(timeBuffer, 1.0 / 4096.0);

                    // Compute the average
                    double average = ArrayUtils.getAverage(timeBuffer);

                    // Offset
                    ArrayUtils.offsetInPlace(timeBuffer, -average);

                    // Apply windows
                    window.applyInPlace(timeBuffer);

                    // Compute real FFT
                    // Size of spectrum is (SamplesPerChirp / 2) + 1
                    System.Numerics.Complex[] spectrum = FftSharp.FFT.ForwardReal(timeBuffer);

                    if (antennaIndex == 0)
                    {
                        if (filteredBackgroundRx1 == null)
                        {
                            filteredBackgroundRx1 = new System.Numerics.Complex[spectrum.Length];
                            for(int i = 0; i <  filteredBackgroundRx1.Length; i++) filteredBackgroundRx1[i] = spectrum[i];
                        }
                        else
                        {
                            for(int i = 0; i < filteredBackgroundRx1.Length; i++)
                            {
                                filteredBackgroundRx1[i] = a * spectrum[i] + b * filteredBackgroundRx1[i];
                            }
                        }
                    }
                    else if (antennaIndex == 1)
                    {
                        if (filteredBackgroundRx2 == null)
                        {
                            filteredBackgroundRx2 = new System.Numerics.Complex[spectrum.Length];
                            for (int i = 0; i < filteredBackgroundRx2.Length; i++) filteredBackgroundRx2[i] = spectrum[i];
                        }
                        else
                        {
                            for (int i = 0; i < filteredBackgroundRx2.Length; i++)
                            {
                                filteredBackgroundRx2[i] = a * spectrum[i] + b * filteredBackgroundRx2[i];
                            }
                        }
                    }
                    else if (antennaIndex == 2)
                    {
                        if (filteredBackgroundRx3 == null)
                        {
                            filteredBackgroundRx3 = new System.Numerics.Complex[spectrum.Length];
                            for (int i = 0; i < filteredBackgroundRx3.Length; i++) filteredBackgroundRx3[i] = spectrum[i];
                        }
                        else
                        {
                            for (int i = 0; i < filteredBackgroundRx3.Length; i++)
                            {
                                filteredBackgroundRx3[i] = a * spectrum[i] + b * filteredBackgroundRx3[i];
                            }
                        }
                    }

                    System.Numerics.Complex[]? background = null;
                    if (antennaIndex == 0)
                    {
                        background = filteredBackgroundRx1;
                    }
                    else if (antennaIndex == 1)
                    {
                        background = filteredBackgroundRx2;
                    }
                    else if (antennaIndex == 2)
                    {
                        background = filteredBackgroundRx3;
                    }

                    // Store it inside the matrix (will be used to compute FFT per bin and the average)
                    for (int freqIndex = 0; freqIndex < spectrum.Length; freqIndex++)
                    {
                        if (background != null)
                        {
                            rangeFFTMatrix[chirpIndex, freqIndex] = spectrum[freqIndex] - background[freqIndex];
                        }
                        else
                            rangeFFTMatrix[chirpIndex, freqIndex] = spectrum[freqIndex];
                    }
                }


                // Send the last chirp samples (formatted)
                sendFormattedTimeSignalEvent(timeBuffer, antennaIndex);

                // [bin, velocity]
                System.Numerics.Complex[,] dopplerFFTMatrix =
                    new System.Numerics.Complex[spectrumLen, radarConfiguration.ChirpsPerFrame];

                double maxAmplitude = double.NaN;
                int rangeForMaxAmplitude = 0;

                // Compute FFT for each frequency bin over time (chirp repetition time)
                System.Numerics.Complex[] binContent = new System.Numerics.Complex[radarConfiguration.ChirpsPerFrame];
                for (int freqIndex = 0; freqIndex < spectrumLen; freqIndex++)
                {
                    // Get the content for this frequency bin
                    for (int chirpIndex = 0; chirpIndex < radarConfiguration.ChirpsPerFrame; chirpIndex++)
                    {
                        binContent[chirpIndex] = rangeFFTMatrix[chirpIndex, freqIndex];
                    }

                    // Compute average and remove it (remove 0 speed)
                    //System.Numerics.Complex avgComplex = ArrayUtils.getAverage(binContent);
                    //ArrayUtils.offsetInPlace(binContent, -avgComplex);

                    dopplerWindow.applyInPlaceComplex(binContent);

                    // Get FFT (transform in place)
                    FftSharp.FFT.Forward(binContent);

                    // Get the doppler FFT for the bin
                    System.Numerics.Complex[] dopplerFFTForBin = FftShift(binContent);

                    // Copy and check for maximum
                    for (int i = 0; i < dopplerFFTForBin.Length; i++)
                    {
                        dopplerFFTMatrix[freqIndex, i] = dopplerFFTForBin[i];

                        double magnitude = dopplerFFTForBin[i].Magnitude;
                        if (double.IsNaN(maxAmplitude) || (magnitude > maxAmplitude))
                        {
                            maxAmplitude = magnitude;
                            rangeForMaxAmplitude = freqIndex;
                        }
                    }
                }

                // Store
                if (antennaIndex == 0) dopplerFFTMatrixRx1 = dopplerFFTMatrix;
                else if (antennaIndex == 1) dopplerFFTMatrixRx2 = dopplerFFTMatrix;
                else if (antennaIndex == 2) dopplerFFTMatrixRx3 = dopplerFFTMatrix;

                if (antennaIndex == (RadarConfiguration.ANTENNA_COUNT - 1))
                {
                    if (dopplerFFTMatrixRx1 == null
                        || dopplerFFTMatrixRx2 == null
                        || dopplerFFTMatrixRx3 == null)
                        return;

                    OnNewDopplerFFTMatrix3?.Invoke(this, dopplerFFTMatrixRx1, dopplerFFTMatrixRx2, dopplerFFTMatrixRx3);
                }

                // Compute spectrum average
                for (int chirpIndex = 0; chirpIndex < radarConfiguration.ChirpsPerFrame; chirpIndex++)
                {
                    for (int freqIndex = 0; freqIndex < spectrumLen; freqIndex++)
                    {
                        if (antennaIndex == 0)
                        {
                            spectrumAvg0[freqIndex] += rangeFFTMatrix[chirpIndex, freqIndex];
                        }
                        else if (antennaIndex == 1)
                        {
                            spectrumAvg1[freqIndex] += rangeFFTMatrix[chirpIndex, freqIndex];
                        }
                        else if (antennaIndex == 2)
                        {
                            spectrumAvg2[freqIndex] += rangeFFTMatrix[chirpIndex, freqIndex];
                        }
                    }
                }
                for (int freqIndex = 0; freqIndex < spectrumLen; freqIndex++)
                {
                    if (antennaIndex == 0)
                    {
                        spectrumAvg0[freqIndex] /= radarConfiguration.ChirpsPerFrame;
                    }
                    else if (antennaIndex == 1)
                    {
                        spectrumAvg1[freqIndex] /= radarConfiguration.ChirpsPerFrame;
                    }
                    else if (antennaIndex == 2)
                    {
                        spectrumAvg2[freqIndex] /= radarConfiguration.ChirpsPerFrame;
                    }
                }

                // Send spectrum as event
                if (antennaIndex == 0) sendSpectrumEvent(spectrumAvg0, antennaIndex);
                else if (antennaIndex == 1) sendSpectrumEvent(spectrumAvg1, antennaIndex);
                else if (antennaIndex == 2) sendSpectrumEvent(spectrumAvg2, antennaIndex);

                OnNewEnergyOverTime?.Invoke(this, maxAmplitude, antennaIndex);
            }
        }
    }
}

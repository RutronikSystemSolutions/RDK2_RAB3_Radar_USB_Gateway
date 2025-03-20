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

        public delegate void OnNewEnergyOverTimeEventHandler(object sender, double energy, double threshold, int antennaIndex);
        public event OnNewEnergyOverTimeEventHandler? OnNewEnergyOverTime;

        public delegate void OnNewDBFOutputEventHandler(object sender, System.Numerics.Complex[,] dbfOutput);
        public event OnNewDBFOutputEventHandler? OnNewDBFOutput;

        public delegate void OnNewTargetDetectedEventHandler(object sender, bool detected, double angle, double range);
        public event OnNewTargetDetectedEventHandler? OnNewTargetDetected;

        public delegate void OnNewDopplerFFTMatrixEventHandler(object sender, System.Numerics.Complex[,] dopplerFFTMatrix, int antennaIndex);
        public event OnNewDopplerFFTMatrixEventHandler? OnNewDopplerFFTMatrix;

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

            for (int i = 0; i < radarConfiguration.AntennaCount; ++i)
            {
                backgroundRemover[i] = new BackgroundRemover((radarConfiguration.SamplesPerChirp / 2) + 1);
                gocafar[i] = new CFAR(16, 32, 0.05);
                somethingDetected[i] = false;
                threshold[i] = 1.5;
            }
        }

        public RadarConfiguration getRadarConfiguration()
        {
            return radarConfiguration;
        }

        public int getFreqBinCount()
        {
            // Since we use no zero padding
            return (radarConfiguration.SamplesPerChirp / 2) + 1;
        }

        public void setObservedRange(int minBin, int maxBin)
        {
            if (minBin >= maxBin) return;
            this.minBin = minBin;
            this.maxBin = maxBin;
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
            // Allocate only once (avoid to much reallocation) --> Might be done once in the constructor as well
            double[] timeBuffer = new double[radarConfiguration.SamplesPerChirp];

            int spectrumLen = (radarConfiguration.SamplesPerChirp / 2) + 1;

            // Store matrix (range FFT per chirp)
            System.Numerics.Complex[,] rangeFFTMatrix = new System.Numerics.Complex[radarConfiguration.ChirpsPerFrame, spectrumLen];

            // Used to compute DBF and background
            System.Numerics.Complex[] spectrumAvg0 = new System.Numerics.Complex[spectrumLen];
            System.Numerics.Complex[] spectrumAvg1 = new System.Numerics.Complex[spectrumLen];

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
                System.Numerics.Complex[,] dopplerFFTMatrix = new System.Numerics.Complex[spectrumLen, radarConfiguration.ChirpsPerFrame];

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
                }

                // Send spectrum as event
                if (antennaIndex == 0) sendSpectrumEvent(spectrumAvg0, antennaIndex);
                else if (antennaIndex == 1) sendSpectrumEvent(spectrumAvg1, antennaIndex);

                // Update background remover and remove in place
                if (antennaIndex == 0)
                {
                    backgroundRemover[antennaIndex].feed(spectrumAvg0);
                    backgroundRemover[antennaIndex].removeBackgroundInPlace(spectrumAvg0);
                }
                else if (antennaIndex == 1)
                {
                    backgroundRemover[antennaIndex].feed(spectrumAvg1);
                    backgroundRemover[antennaIndex].removeBackgroundInPlace(spectrumAvg1);
                }

                OnNewEnergyOverTime?.Invoke(this, maxAmplitude, threshold[antennaIndex], antennaIndex);
                OnNewDopplerFFTMatrix?.Invoke(this, dopplerFFTMatrix, antennaIndex);

                if (maxAmplitude > threshold[antennaIndex])
                {
                    somethingDetected[antennaIndex] = true;
                    rangeOfMax[antennaIndex] = rangeForMaxAmplitude;
                }
                else somethingDetected[antennaIndex] = false;
            }

            // Compute DBF
            // [beam/angle , range]
            System.Numerics.Complex[,] dbfOut = dbf.compute(spectrumAvg0, spectrumAvg1);
            OnNewDBFOutput?.Invoke(this, dbfOut);

            if (somethingDetected[0]  && somethingDetected[1])
            {
                // Both antennas detected something
                // Extract biggest from dbf output
                //ArrayUtils.getBiggestOfMatrix(dbfOut, out int rowIndex, out int colIndex);

                int rowIndex = 0;
                int colIndex = 0;

                colIndex = rangeOfMax[0];

                // Get the biggest row for a given column
                double max = dbfOut[0, colIndex].Magnitude;
                for(int i = 1; i < dbfOut.GetLength(0); ++i)
                {
                    double mag = dbfOut[i, colIndex].Magnitude;
                    if (mag > max)
                    {
                        max = mag;
                        rowIndex = i;
                    }
                }

                // Convert rowIndex and colIndex to range in meters and angle in degree
                OnNewTargetDetected?.Invoke(this, true, rowIndex, convertDBFColToMeters(colIndex));
            }
            else
            {
                OnNewTargetDetected?.Invoke(this, false, -1, -1);
            }
        }

        private double convertDBFColToMeters(int colIndex)
        {
            double bandWidth = radarConfiguration.EndFrequency - radarConfiguration.StartFrequency;
            double celerity = 299792458;
            double slope = bandWidth / (radarConfiguration.SamplesPerChirp * (1 / radarConfiguration.SamplingRate));
            double fftLen = (radarConfiguration.SamplesPerChirp / 2) + 1;

            double fractionFs = colIndex / ((fftLen - 1) * 2);
            double freq = fractionFs * radarConfiguration.SamplingRate;
            double rangeMeters = (celerity * freq) / (2 * slope);
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

        /// <summary>
        /// Feed the processor with data
        /// /!\ in case more than one antenna, frame looks like:
        /// frame[0]: sample 0 of antenna 0
        /// frame[1]: sample 0 of antenna 1
        /// ...
        /// </summary>
        /// <param name="frame"></param>
        public void feed(ushort[] frame)
        {
            System.Numerics.Complex[]? spectrumAvg0 = null;
            System.Numerics.Complex[]? spectrumAvg1 = null;

            // Extract per antenna
            for (int antennaIndex = 0; antennaIndex < radarConfiguration.AntennaCount; antennaIndex++)
            {
                double[] timeBuffer = new double[radarConfiguration.SamplesPerChirp];
                System.Numerics.Complex[] spectrumAvg = new System.Numerics.Complex[(radarConfiguration.SamplesPerChirp / 2) + 1];

                for (int chirpIndex = 0; chirpIndex < radarConfiguration.ChirpsPerFrame; chirpIndex++)
                {
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
                    System.Numerics.Complex[] spectrum = FftSharp.FFT.ForwardReal(timeBuffer);

                    // Add to averaging
                    for (int i = 0; i < spectrum.Length; i++)
                    {
                        spectrumAvg[i] += spectrum[i];
                    }
                }

                // Send the last chirp samples (formatted)
                sendFormattedTimeSignalEvent(timeBuffer, antennaIndex);

                // Compute average
                for (int i = 0; i < spectrumAvg.Length; i++)
                {
                    spectrumAvg[i] /= radarConfiguration.ChirpsPerFrame;
                }

                // Send spectrum (with background)
                //sendSpectrumEvent(spectrumAvg, antennaIndex);

                // Update background remover
                backgroundRemover[antennaIndex].feed(spectrumAvg);

                // Remove in place
                backgroundRemover[antennaIndex].removeBackgroundInPlace(spectrumAvg);
                //backgroundRemover[antennaIndex].removeBackgroundInPlaceIfLower(spectrumAvg);

                // Send spectrum as event (without background)
                sendSpectrumEvent(spectrumAvg, antennaIndex);

                // Copy (will be used for DBF computation)
                if (antennaIndex == 0)
                {
                    spectrumAvg0 = new System.Numerics.Complex[spectrumAvg.Length];
                    for (int i = 0; i < spectrumAvg0.Length; ++i) spectrumAvg0[i] = spectrumAvg[i];
                }
                else if (antennaIndex == 1)
                {
                    spectrumAvg1 = new System.Numerics.Complex[spectrumAvg.Length];
                    for (int i = 0; i < spectrumAvg1.Length; ++i) spectrumAvg1[i] = spectrumAvg[i];
                }

                // Compute magnitude
                double[] magnitude = new double[spectrumAvg.Length];
                for(int i = 0; i < magnitude.Length; ++i)
                {
                    magnitude[i] = spectrumAvg[i].Magnitude;
                }

                double sum = 0;
                for (int i = minBin; i < maxBin; ++i)
                {
                    sum += magnitude[i];
                }
                // Scale
                sum = sum / (maxBin - minBin);

                /*
                // Threshold computation
                gocafar[antennaIndex].feed(sum);

                if (somethingDetected[antennaIndex] == false) 
                {
                    threshold[antennaIndex] = gocafar[antennaIndex].DynamicThreshold;
                    if (sum > threshold[antennaIndex])
                    {
                        somethingDetected[antennaIndex] = true;
                    }
                }
                else
                {
                    if (sum < threshold[antennaIndex])
                    {
                        somethingDetected[antennaIndex] = false;
                    }
                }*/

                if (somethingDetected[antennaIndex] == false)
                {
                    if (sum > threshold[antennaIndex])
                    {
                        somethingDetected[antennaIndex] = true;
                    }
                }
                else
                {
                    if (sum < threshold[antennaIndex])
                    {
                        somethingDetected[antennaIndex] = false;
                    }
                }

                OnNewEnergyOverTime?.Invoke(this, sum, threshold[antennaIndex], antennaIndex);
            }

            // Compute DBF (remark -> later on only if target has been detected before)
            if (spectrumAvg0 != null && spectrumAvg1 != null)
            {
                System.Numerics.Complex[,] dbfOut = dbf.compute(spectrumAvg0, spectrumAvg1);
                OnNewDBFOutput?.Invoke(this, dbfOut);

                if (somethingDetected[0] && somethingDetected[1])
                {
                    // Both antennas detected something
                    // Extract biggest from dbf output
                    ArrayUtils.getBiggestOfMatrix(dbfOut, out int rowIndex, out int colIndex);

                    OnNewTargetDetected?.Invoke(this, true, rowIndex, colIndex);
                }
                else
                {
                    OnNewTargetDetected?.Invoke(this, false, -1, -1);
                }
            }
        }
    }
}

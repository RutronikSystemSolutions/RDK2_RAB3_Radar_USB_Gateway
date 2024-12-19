using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime;
using static System.Collections.Specialized.BitVector32;

namespace RDK2_Radar_SignalProcessing_GUI
{
    public class DatasetAnalyserNN
    {
        private FormatterForNN formatterForNN = new FormatterForNN();
        private InferenceSession inferenceSession;

        public delegate void OnInferenceResultEventHandler(object sender, float[] result);
        public event OnInferenceResultEventHandler? OnInferenceResult;

        public DatasetAnalyserNN(string path)
        {
            inferenceSession = new InferenceSession(path);
        }

        public void Analyse(List<DataCollector.DataSet> dataset)
        {
            //using var session = new InferenceSession("C:\\rutronik\\python_projects\\machine_learning_old_version\\model.onnx");
            {
                const int DATA_COUNT = 96;
                float[] sourceData = formatterForNN.Format(dataset);

                long[] dimensions = new long[] { 1, DATA_COUNT }; // and the dimensions of the input is stored here

                // Create a OrtValue on top of the sourceData array
                using var inputOrtValue = OrtValue.CreateTensorValueFromMemory(sourceData, dimensions);

                var inputs = new Dictionary<string, OrtValue> {
                    { "dense_input",  inputOrtValue }
                };

                using var runOptions = new Microsoft.ML.OnnxRuntime.RunOptions();


                // Pass inputs and request the first output
                // Note that the output is a disposable collection that holds OrtValues
                using var output = inferenceSession.Run(runOptions, inputs, inferenceSession.OutputNames);

                var output_0 = output[0];

                // Assuming the output contains a tensor of float data, you can access it as follows
                // Returns Span<float> which points directly to native memory.
                var outputData = output_0.GetTensorDataAsSpan<float>();

                float[] inferenceResult = new float[3];

                for (int i = 0; i < outputData.Length; i++)
                {
                    // System.Diagnostics.Debug.WriteLine(outputData[i]);
                    inferenceResult[i] = outputData[i];
                }

                OnInferenceResult?.Invoke(this, inferenceResult);
            }
        }

    }
}

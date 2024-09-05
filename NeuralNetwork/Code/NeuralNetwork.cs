using System;

namespace NeuralNetwork.Code;

public class NeuralNetwork{
    float[][] neurons { get; }

    //Generic Constructor
    public NeuralNetwork (int[] layers, string data) {
        neurons = CreateNeurons(layers);
        FillNeuronsFromData(data);
    }
    //Copy Constructor
    //Copies all values from the given network
    public NeuralNetwork (NeuralNetwork network) {
        int[] layers = new int[network.neurons.Length];
        for (int i = 0; i < layers.Length; i++) {
            layers[i] = network.neurons[i].Length;
        }
        neurons = CreateNeurons(layers);
        network.neurons.CopyTo(neurons, 0);
    }

    //Creates the layers and neurons
    float[][] CreateNeurons(int[] layers) {
        float[][] neurs = new float[layers.Length][];
        for (int i = 0; i < neurs.Length; i++) {
            neurs[i] = new float[layers[i]];
        }
        return neurs;
    }

    //Fills data to each neuron based on the string given
    //Data in the form of "0.000, 0.000; 0.000, 0.000, 0.000"
    void FillNeuronsFromData(string data) {
        //Splits each layer apart
        string[] layerStrings = data.Split(';');

        //Splits each neuron apart from the layer
        string[][] neuronStrings = new string[layerStrings.Length][];
        for (int i = 0; i < neuronStrings.Length; i++) {
            neuronStrings[i] = layerStrings[i].Split(',');
        }

        //Throws an error if the input data is of the wrong length
        if (neuronStrings.Length != neurons.Length) {
            Console.WriteLine("Error in FillNeuronsFromData: Input data length does not match number of neurons");
            return;
        }
        //Sets each neuron
        for (int i = 0; i < neurons.Length; i++) {
            for (int j = 0; j < neurons[i].Length; j++) {
                float neuronValue;
                //Throws an error if the input data is not the correct format
                if (!float.TryParse(neuronStrings[i][j], out neuronValue)) {
                    Console.WriteLine("Error in FillNeuronsFromData: Input data in invalid format at (" + i + ", " + j + ")");
                    return;
                }
                    neurons[i][j] = neuronValue;
            }
        }
    }

    //Changes one neuron to the given value
    public void ChangeNeuron(int layer, int neuron, float value) {
        //Throws an error if the inputted value is not valid
        if (layer < 0 || neuron < 0 || neurons.Length <= layer || neurons[layer].Length <= neuron) {
            Console.WriteLine("Error in ChangeNeuron: Input variables invalid");
        }
        neurons[layer][neuron] = value;
    }

    //Computes the input data through the network and outputs a float[] with the values
    public float[] Compute(float[] inputs) {
        float[] output = new float[neurons[neurons.Length-1].Length];

        return output; 
    }
}

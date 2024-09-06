using System;
using System.Runtime.CompilerServices;

namespace NeuralNetwork.Code;

//Rewrite for weights to be correct
//Currently each weight is in the neuron spot
//Needs to be a weight for each indiviual connection
public class NeuralNetwork{
    float[][] weights { get; }

    //Sets the clamp mode Computer will use
    // 0 == [0, 1]
    // 1 == [-1, 1]
    int clampType { get; }

    //Generic Constructor
    public NeuralNetwork (int[] layers, int clampMode, string data) {
        weights = CreateWeights(layers);
        clampType = clampMode;
        FillWeightsFromData(data);
    }
    //Copy Constructor
    //Copies all values from the given network
    public NeuralNetwork (NeuralNetwork network) {
        int[] layers = new int[network.weights.Length];
        for (int i = 0; i < layers.Length; i++) {
            layers[i] = network.weights[i].Length;
        }
        weights = CreateWeights(layers);
        network.weights.CopyTo(weights, 0);
        clampType = network.clampType;
    }

    //Creates the layers and neuron weight
    float[][] CreateWeights(int[] layers) {
        float[][] tempWeights = new float[layers.Length][];
        for (int i = 0; i < tempWeights.Length; i++) {
            tempWeights[i] = new float[layers[i]];
        }
        return tempWeights;
    }

    //Fills data to each neuron weight based on the string given
    //Data in the form of "0.000..., 0.000...; 0.000..., 0.000..., 0.000..."
    void FillWeightsFromData(string data) {
        //Splits each layer apart
        string[] layerStrings = data.Split(';');

        //Splits each weight apart from the layer
        string[][] weightStrings = new string[layerStrings.Length][];
        for (int i = 0; i < weightStrings.Length; i++) {
            weightStrings[i] = layerStrings[i].Split(',');
        }

        //Throws an error if the input data is of the wrong length
        if (weightStrings.Length != weights.Length) {
            Console.WriteLine("Error in FillWeightsFromData: Input data length does not match number of neurons");
            return;
        }
        //Sets each weight
        for (int i = 0; i < weights.Length; i++) {
            for (int j = 0; j < weights[i].Length; j++) {
                float weightValue;
                //Throws an error if the input data is not the correct format
                if (!float.TryParse(weightStrings[i][j], out weightValue)) {
                    Console.WriteLine("Error in FillWeightsFromData: Input data in invalid format at (" + i + ", " + j + ")");
                    return;
                }
                    weights[i][j] = weightValue;
            }
        }
    }

    //Changes one neuron weight to the given value
    public void ChangeNeuronWeight(int layer, int neuron, float value) {
        //Throws an error if the inputted value is not valid
        if (layer < 0 || neuron < 0 || weights.Length <= layer || weights[layer].Length <= neuron) {
            Console.WriteLine("Error in ChangeNeuronWeight: Input variables invalid");
        }
        weights[layer][neuron] = value;
    }

    //Computes the input data through the network and outputs a float[] with the values
    //FINISH
    public float[] Compute(float[] input) {
        //Checks if input[] length is correct
        //Returns an empty float[] if incorrect
        if (input.Length != weights[0].Length) {
            Console.WriteLine("Error in Compute: legth of inputs[] does not equal legth of input layer");
            return new float[0];
        }

        float[] output = new float[weights[weights.Length-1].Length];

        float[][] values = new float[weights.Length][];
        values[0] = input;
        for (int i = 1; i < values.Length; i++) {
            values[i] = new float[weights[i].Length];
        }

        return output; 
    }
}

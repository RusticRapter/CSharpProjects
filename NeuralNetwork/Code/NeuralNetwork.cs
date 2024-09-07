using System;
using System.Numerics;

namespace NeuralNetwork.Code;

//Changes Needed
//Rewrite for weights to be correct
//  Currently each weight is in the neuron spot
//  Needs to be a weight for each indiviual connection
//Add activation functions
//  1/(1+e^-x)
//  clamp[0, 1]
//  other?
//
//Later Updates
//Add biases?
public class NeuralNetwork{
    //The number of inputs the network takes
    int inputLength { get; }
    
    //The number of outputs the network gives out
    int outputLength { get; }

    //The weights for each neuron connecting backwards
    //Ex. the neuron for output 1 contains an array of floats for each weight to each neuron in the last hidden layer
    float[][][] weights { get; }

    //Sets the activation function the network will use
    //  0 == 1/(1+e^-x)
    //  1 == clamp[0, 1]
    int activationMode { get; }

    //Generic Constructor
    //Sets the weights either as 0.5f or as random floats between 0 and 1 based on initalizeRandom
    public NeuralNetwork (int inputs, int outputs, int[] hiddenLayers, int activationType, bool initalizeRandom) {
        //Creates the empty weights arrays
        weights = CreateWeights(inputLength, outputLength, hiddenLayers);

        //Sets the other variables
        inputLength = inputs;
        outputLength = outputs;
        activationMode = activationType;
        
        //Fills the weights
        FillWeights(initalizeRandom);
    }

    //Constructor from previous data
    //Sets the weights based on the inputed data
    //  Data in the form of "0.000...,0.000...;0.000...,0.000...,0.000...:0.000...; 0.000..., 0.000..."
    //  Commas separate weights, semi colons separate neurons, colons separate layers
    public NeuralNetwork (int inputs, int outputs, int[] hiddenLayers, int activationType, string data) {
        //Creates the empty weight arrays
        weights = CreateWeights(inputLength, outputLength, hiddenLayers);

        //Sets other variables
        inputLength = inputs;
        outputLength = outputs;
        activationMode = activationType;

        //Fills the weights with data
        FillWeightsFromData(data);
    }
    //Copy Constructor
    //Copies all values from the given network
    public NeuralNetwork (NeuralNetwork network) {
        //Copies the size of the hidden layers
        int[] hiddenLayers = new int[network.weights.Length - 1];
        for (int i = 0; i < hiddenLayers.Length; i++) {
            hiddenLayers[i] = network.weights[i].Length;
        }

        //Copies the input and output size
        inputLength = network.inputLength;
        outputLength = network.outputLength;

        //Creates the empty weights arrays
        weights = CreateWeights(inputLength, outputLength, hiddenLayers);
        
        //Copies the weights from the given network
        network.weights.CopyTo(weights, 0);

        //Copies the other variables
        activationMode = network.activationMode;
    }

    //Creates the layers and neuron weight
    float[][][] CreateWeights(int inputs, int outputs, int[] hiddenLayers) {
        //Create the empty outer array
        //Length is the number of layers - 1
        //  Input layer doesn't have weights
        float[][][] outWeights = new float[hiddenLayers.Length+1][][];

        //Create each empty layer array
        //Length is the number of neurons in each layer
        for (int i = 0; i < outWeights.Length-1; i++) {
            outWeights[i] = new float[hiddenLayers[i]][];
        }
        //Last layer array is the output layer
        outWeights[outWeights.Length-1] = new float[outputs][];

        //Create each weights array
        //Length is number of neurons in the layer before
        //First array connects to the input layer
        for (int j = 0; j < outWeights[0].Length; j++) {
            outWeights[0][j] = new float[inputs];
        }
        //Other arrays connect to hidden layers
        for (int i = 1; i < outWeights.Length; i++) {
            for (int j = 0; j < outWeights[i].Length; j++) {
                outWeights[i][j] = new float[outWeights[i-1].Length];
            }
        }

        return outWeights;
    }

    //Fills the weights with either 0.5f or a random float between 0 and 1
    //  initilizeRandom determines mode
    void FillWeights(bool initializeRandom) {
        if (initializeRandom) {
            Random rand = new Random();
            for (int i = 0; i < weights.Length; i++) {
                for (int j = 0; j < weights[i].Length; j++) {
                    for (int k = 0; k < weights[i][j].Length; k++) {
                        weights[i][j][k] = rand.NextSingle();
                    }
                }
            }
        }
        else {
            for (int i = 0; i < weights.Length; i++) {
                for (int j = 0; j < weights[i].Length; j++) {
                    for (int k = 0; k < weights[i][j].Length; k++) {
                        weights[i][j][k] = 0.5f;
                    }
                }
            }
        }
    }

    //Fills data to each neuron weight based on the string given
    //  Data in the form of "0.000..., 0.000...; 0.000..., 0.000..., 0.000..."
    //  comma separates weights, semi colon separates neurons, colon separates layers
    void FillWeightsFromData(string data) {
        //Splits each layer apart
        string[] layerStrings = data.Split(':');
        //Checks if number of layers is correct
        if (layerStrings.Length != weights.Length) {
            Console.WriteLine("Error in FillWeightsFromData: Input data layers does not match number of layers expected");
            return;
        }

        //Splits each neuron apart from the layer
        string[][] neuronStrings = new string[layerStrings.Length][];
        for (int i = 0; i < neuronStrings.Length; i++) {
            neuronStrings[i] = layerStrings[i].Split(';');

            //Checks if number of neurons is correct
            if (neuronStrings[i].Length != weights[i].Length) {
                Console.WriteLine("Error in FillWeightsFromData: Input data number of neurons in layer " + i + " does not match number of neurons expected");
                return;
            }
        }

        //Splits each weight apart from the neuron
        //Then sets the weights
        string[][][] weightStrings = new string[layerStrings.Length][][];
        for (int i = 0; i < weightStrings.Length; i++) {
            for (int j = 0; j < weightStrings[i].Length; j++) {
                weightStrings[i][j] = neuronStrings[i][j].Split(',');

                //Checks if number of weights is correct
                if (weightStrings[i][j].Length != weights[i][j].Length) {
                Console.WriteLine("Error in FillWeightsFromData: Input data number of weights in layer " + i + ", neuron " + j + " does not match number of weights expected");
                return;
                }

                //Sets each weight
                for (int k = 0; k < weightStrings[i][j].Length; k++) {
                    float weightValue;
                    //Throws an error if the input data is not the correct format
                    if (!float.TryParse(weightStrings[i][j][k], out weightValue)) {
                        Console.WriteLine("Error in FillWeightsFromData: Input data weight in invalid format in layer " + i + ", neuron " + j + ", weight " + k);
                        return;
                    }
                    weights[i][j][k] = weightValue;
                }
            }
        }
    }

    //Impliments the activation function
    //
    //Create
    float activation(float input) {
        float output = input;
        return output;
    }

    //Changes one neuron weight to the given value
    public void ChangeNeuronWeight(int layer, int neuron, int connection, float value) {
        //Throws an error if the inputted value is not valid
        if (layer < 0 || neuron < 0 || connection < 0 || weights.Length <= layer || weights[layer].Length <= neuron || weights[layer][neuron].Length <= connection) {
            Console.WriteLine("Error in ChangeNeuronWeight: Input variables invalid");
        }
        weights[layer][neuron][connection] = value;
    }

    //Computes the input data through the network and outputs a float[] with the values
    //
    //FINISH
    public float[] Compute(float[] input) {
        //Checks if input[] length is correct
        //Returns an empty float[] if incorrect
        if (input.Length != inputLength) {
            Console.WriteLine("Error in Compute: length of inputs[] does not equal legth of input layer");
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

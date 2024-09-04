using System;

namespace NeuralNetwork.Code;

public class NeuralNetwork{
    float[][] neurons;

    NeuralNetwork (int[] layers) {
        neurons = CreateNeurons(layers);
        FillNeurons();
    }

    float[][] CreateNeurons(int[] layers) {
        float[][] neurs = new float[layers.Length][];
        for (int i = 0; i < neurs.Length; i++) {
            neurs[i] = new float[layers[i]];
        }
        return neurs;
    }

    void FillNeurons() {

    }
}

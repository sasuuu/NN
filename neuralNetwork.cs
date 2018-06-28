using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class neuralNetwork : MonoBehaviour {
    public nnWeights weights;
    public float[] Hl;
    float output;


    public float compute(float[] inputs)
    {
        Hl = new float[3];
        Hl[0] = 0f;
        Hl[1] = 0f;
        Hl[2] = 1.0f;
        output = 0;
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 3; j++)
            {
                if(i == 0) Hl[i] += weights.HLweights1[j] * inputs[j];
                else Hl[i] += weights.HLweights2[j] * inputs[j];
            }
        for (int i = 0; i < 2; i++)
            Hl[i] = sigmoid(Hl[i]);
        for (int i = 0; i < 2; i++)
            output += Hl[i] * weights.OLweights[i];
        output = sigmoid(output);
        return output;
    }

    public float sigmoid(float x) { return 1 / (1 + Mathf.Exp(-x)); }
}

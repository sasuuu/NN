using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class nnWeights:ScriptableObject{
    public float[] HLweights1;
    public float[] HLweights2;
    public float[] OLweights;

    public void saveWeights()
    {
        string json = JsonUtility.ToJson(this);
        System.IO.File.WriteAllText("weights.json", json);
    }

    public nnWeights()
    {
        HLweights1 = new float[3];
        HLweights2 = new float[3];
        OLweights = new float[3];
    }
}

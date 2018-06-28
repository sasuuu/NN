using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class gameController : MonoBehaviour {

    public GameObject bird;
    public GameObject pipes;
    public Text score;
    public Text bestScore;
    GameObject obj;
    public int birdsCount = 20;
    public List<controll> birdsList;
    public List<nnWeights> newWeights;
    public string[] json;
    public Slider sld;
    float mutationRate = 0.15f;

    int compare(controll x, controll y)
    {
        if (x.score < y.score) return 1;
        else if (x.score == y.score) return 0;
        else return -1;
    }

	void Start () {
        json = new string[birdsCount];
        if (System.IO.File.Exists("weights.json"))
        {
            json = System.IO.File.ReadAllLines("weights.json");
        }
        for (int i = 0; i < birdsCount; i++)
        {
            obj = Instantiate(bird, new Vector3(-13, 0, -1), Quaternion.identity);
            if (System.IO.File.Exists("weights.json"))
            {
                obj.GetComponent<neuralNetwork>().weights = ScriptableObject.CreateInstance<nnWeights>();
                JsonUtility.FromJsonOverwrite(json[i], obj.GetComponent<neuralNetwork>().weights);
            }
            else
            {
                obj.GetComponent<neuralNetwork>().weights = ScriptableObject.CreateInstance<nnWeights>();
                for (int j = 0; j < 3; j++)
                {
                    obj.GetComponent<neuralNetwork>().weights.OLweights[j] = Random.Range(-2f, 2f);
                }
                for (int k = 0; k < 3; k++)
                {
                    obj.GetComponent<neuralNetwork>().weights.HLweights1[k] = Random.Range(-2f, 2f);
                    obj.GetComponent<neuralNetwork>().weights.HLweights2[k] = Random.Range(-2f, 2f);
                }
                json[i] = JsonUtility.ToJson(obj.GetComponent<neuralNetwork>().weights);
            }
        }
        if (!System.IO.File.Exists("weights.json"))
        {
            System.IO.File.WriteAllLines("weights.json",json);
        }

        Instantiate(pipes, new Vector3(17,0,-1), Quaternion.identity);
        score.text = "Score: 0";
        string bs = System.IO.File.ReadAllText("bestScore.txt");
        bestScore.text = "Best Score: "+bs;
    }

    void Update()
    {
        sld.value = Time.timeScale;
        if (Input.GetKey("a")) Time.timeScale -= 0.2f;
        else if (Input.GetKey("d")) Time.timeScale += 0.2f;
        Mathf.Clamp(Time.timeScale, 0f, 5f);
        var birds = FindObjectsOfType<controll>();
        for (int i = 0; i < birds.Length; i++) birdsList.Add(birds[i]);
        birdsList.Sort(compare);
        if(birds.Length != 0)score.text = "Score: " + Mathf.Round(birdsList[0].GetComponent<controll>().score).ToString();
        birdsList.Clear();
        if (birds.Length == 0)
        {
            var allBirds = Resources.FindObjectsOfTypeAll(typeof(controll)) as controll[];
            for (int i = 0; i < allBirds.Length; i++) birdsList.Add(allBirds[i]);
            birdsList.Sort(compare);
            int f=0;
            int m=0;
            float r;
            float mutation;
            string[] weights;
            weights = new string[birdsCount];
            weights = System.IO.File.ReadAllLines("weights.json");
            weights[1] = JsonUtility.ToJson(birdsList[1].GetComponent<neuralNetwork>().weights);
            weights[2] = JsonUtility.ToJson(birdsList[2].GetComponent<neuralNetwork>().weights);
            weights[3] = JsonUtility.ToJson(birdsList[3].GetComponent<neuralNetwork>().weights);
            weights[4] = JsonUtility.ToJson(birdsList[4].GetComponent<neuralNetwork>().weights);
            weights[5] = JsonUtility.ToJson(birdsList[5].GetComponent<neuralNetwork>().weights);
            for (int i = 6; i < birdsCount; i++)
            {
                newWeights.Add(ScriptableObject.CreateInstance<nnWeights>());
                r = Random.Range(0, 1f);
                if (r < 0.4) f = 1;
                else if (r >= 0.4 && r < 0.6) f = 2;
                else if (r >= 0.6 && r < 0.7) f = 3;
                else if (r >= 0.7 && r < 0.8) f = 4;
                else if (r >= 0.8) f = 5;
                do
                {
                    r = Random.Range(0, 1f);
                    if (r < 0.4) m = 1;
                    else if (r >= 0.4 && r < 0.6) m = 2;
                    else if (r >= 0.6 && r < 0.7) m = 3;
                    else if (r >= 0.7 && r < 0.8) m = 4;
                    else if (r >= 0.8) m = 5;
                } while (f == m);
                for(int j = 0; j < 3; j++)
                {
                    mutation = Random.Range(0, 1f);
                    if (mutation < mutationRate)
                    {
                        newWeights[i-6].OLweights[j] = Random.Range(-3f,3f);
                    }
                    else
                    {
                        r = Random.Range(0, 1f);
                        if (r < 0.5f) newWeights[i-6].OLweights[j] = birdsList[f].GetComponent<neuralNetwork>().weights.OLweights[j];
                        else newWeights[i-6].OLweights[j] = birdsList[m].GetComponent<neuralNetwork>().weights.OLweights[j];
                    }
                }
                for (int j = 0; j < 3; j++)
                {
                    mutation = Random.Range(0, 1f);
                    if (mutation < mutationRate)
                    {
                        newWeights[i-6].HLweights1[j] = Random.Range(-3f, 3f);
                    }
                    else
                    {
                        r = Random.Range(0, 1f);
                        if (r < 0.5f) newWeights[i-6].HLweights1[j] = birdsList[f].GetComponent<neuralNetwork>().weights.HLweights1[j];
                        else newWeights[i-6].HLweights1[j] = birdsList[m].GetComponent<neuralNetwork>().weights.HLweights1[j];
                    }
                }
                for (int j = 0; j < 3; j++)
                {
                    mutation = Random.Range(0, 1f);
                    if (mutation < mutationRate)
                    {
                        newWeights[i-6].HLweights2[j] = Random.Range(-3f, 3f);
                    }
                    else
                    {
                        r = Random.Range(0, 1f);
                        if (r < 0.5f) newWeights[i-6].HLweights2[j] = birdsList[f].GetComponent<neuralNetwork>().weights.HLweights2[j];
                        else newWeights[i-6].HLweights2[j] = birdsList[m].GetComponent<neuralNetwork>().weights.HLweights2[j];
                    }
                }
                /*mutation = Random.Range(0, 1f);
                if (mutation < 0.15)
                {
                    m = Mathf.RoundToInt(Random.Range(0, 2f));
                    switch (m)
                    {
                        case 0:
                            m = Mathf.RoundToInt(Random.Range(0, 1f));
                            newWeights[i].OLweights[m] += Random.Range(-2f, 2f);
                            break;
                        case 1:
                            m = Mathf.RoundToInt(Random.Range(0, 1f));
                            newWeights[i].HLweights1[m] += Random.Range(-2f, 2f);
                            break;
                        case 2:
                            m = Mathf.RoundToInt(Random.Range(0, 1f));
                            newWeights[i].HLweights2[m] += Random.Range(-2f, 2f);
                            break;
                    }
                }*/
                weights[i] = JsonUtility.ToJson(newWeights[i-6]);
            }
            if (birdsList[0].score > System.Int32.Parse(System.IO.File.ReadAllText("bestScore.txt")))
            {
                System.IO.File.WriteAllText("bestScore.txt", Mathf.Round(birdsList[0].score).ToString());
                weights[0] = JsonUtility.ToJson(birdsList[0].GetComponent<neuralNetwork>().weights);
            }
            System.IO.File.WriteAllLines("weights.json", weights);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}

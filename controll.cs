using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class controll : MonoBehaviour {

    Rigidbody rb;
    Vector3 position;
    Vector3 newPosition;
    neuralNetwork nn;
    float fall = 8f;
    public float score=0;
    float nnOutput;
    float[] input;
    public List<pipeMove> pipes;
    pipeMove closestPipe;

	void Start (){
        input = new float[3];
        rb = GetComponent<Rigidbody>();
        nn = GetComponent<neuralNetwork>();
	}
	
	// Update is called once per frame
	void Update () {
        score += 1f * Time.deltaTime;
        //rb.AddForce(new Vector3(0f, -9.81f, 0f), ForceMode.Force);
        if(fall<18f) fall += 0.2f*Time.deltaTime;
        position = rb.position;
        Vector3 lol = new Vector3(0, fall * Time.deltaTime, 0);
        newPosition = position - lol;
        rb.MovePosition(newPosition);
        var allPipes = FindObjectsOfType<pipeMove>();
        if (allPipes.Length > 0)
        {
            foreach (var pipe in allPipes)
            {
                if (pipe.GetComponent<Transform>().position.x > rb.position.x && !pipes.Exists(x => x.GetInstanceID()==pipe.GetInstanceID())) pipes.Add(pipe);
            }
            closestPipe = pipes[0];
            pipes.RemoveAll(x => x.GetComponent<Transform>().position.x < rb.position.x);
            foreach (pipeMove pipe in pipes)
            {
                if (pipe.GetComponent<Transform>().position.x < closestPipe.GetComponent<Transform>().position.x) closestPipe = pipe;
            }
            
        }
        input[0] = rb.transform.position.y;
        if (closestPipe) input[1] = closestPipe.GetComponent<Transform>().position.y;
        else input[1] = 0f;
        input[2] = 1f;
        nnOutput = nn.compute(input);
        if (nnOutput>0.5)
        {
            position = rb.position;
            Vector3 jump = new Vector3(0,2f,0);
            newPosition = rb.position + jump;
            rb.MovePosition(newPosition);
            fall = 8f;
            //rb.AddForce(new Vector3(0f, 15f, 0f), ForceMode.Impulse);
        }
        if (rb.position.y < -7f) this.gameObject.SetActive(false);
        if (rb.position.y > 7f) rb.position = new Vector3(-13, 7f, -1);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "1" || other.name =="2") this.gameObject.SetActive(false);
    }

}

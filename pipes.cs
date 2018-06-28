using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipes : MonoBehaviour {

    public GameObject pipe;
    float count = 3.0f;
    float rand;
    Vector3 position;
	
	void Update () {
        count -= 3f*Time.deltaTime;
        if (count <= 0)
        {
            rand = Random.Range(-4f, 5f);
            position = new Vector3(transform.position.x, transform.position.y + rand, transform.position.z);
            Instantiate(pipe, position, Quaternion.identity);
            count = 3.0f;
        }
	}
}

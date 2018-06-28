using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeMove : MonoBehaviour {

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 move = new Vector3(-8, 0, 0);
        rb.MovePosition(rb.position+move*Time.deltaTime);
        if (transform.position.x <= -19f)
        {
            DestroyImmediate(this.gameObject);
        }
	}
}

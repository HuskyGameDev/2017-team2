using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {


    public float speed;

	// Use this for initialization
	void Start () {
        print("Running!");
        GetComponent<Rigidbody2D>().velocity = -transform.up * speed;
    }
}

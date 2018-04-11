using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slideBackground : MonoBehaviour {

    private float savedTime;
    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start () {
        savedTime = Time.time;
        rb2d = GetComponent<Rigidbody2D>();

        
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time - savedTime <= 7)
        {
            rb2d.velocity = new Vector2(10, 0);
        }
        else if (Time.time - savedTime >= 5 && Time.time - savedTime <= 14)
        {
            rb2d.velocity = new Vector2(-10, 0);
        }
        else if (Time.time - savedTime > 14)
        {
            savedTime = Time.time;
        }
    }
}

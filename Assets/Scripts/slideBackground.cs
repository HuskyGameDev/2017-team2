using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slideBackground : MonoBehaviour {

    float savedTime;

    // Use this for initialization
    void Start () {
        savedTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time - savedTime <= 7)
        {
            transform.Translate(10 * Time.deltaTime, 0, 0);
        }
        else if (Time.time - savedTime >= 5 && Time.time - savedTime <= 14)
        {
            transform.Translate(-10 * Time.deltaTime, 0, 0);
        }
        else if (Time.time - savedTime > 14)
        {
            savedTime = Time.time;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        GetComponent<Animation>().Play();
	}
}

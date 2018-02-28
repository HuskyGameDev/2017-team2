using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopForeground : MonoBehaviour {

    int loopHeight = 2400;
    int buildingHeight = 3456;
        	
	// Update is called once per frame
	void Update () {
        transform.Translate(0, 50 * Time.deltaTime, 0);

        if (transform.position.y > loopHeight)
        {
            Vector2 offset = new Vector2(0, buildingHeight);
            transform.position = (Vector2)transform.position - offset;
        }
    }
}

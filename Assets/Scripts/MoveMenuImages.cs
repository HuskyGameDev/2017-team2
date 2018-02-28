using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveMenuImages : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, 2 * Time.deltaTime);        
    }
}

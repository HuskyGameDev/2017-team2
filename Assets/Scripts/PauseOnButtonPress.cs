using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOnButtonPress : MonoBehaviour {

    public Transform canvas;
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(!canvas.gameObject.activeInHierarchy)
            {
                canvas.gameObject.SetActive(true);
            }
            else
            {
                canvas.gameObject.SetActive(false);
            }

        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject player;
    public Vector3 offset;
    public bool isStarted;

    //Was at one point -4.2, -5. No idea why
    void Start() {
        offset = transform.position - player.transform.position + new Vector3(-5, -5);
    }

    void LateUpdate() {
        if (isStarted)
            transform.position = player.transform.position + offset;

    }
}

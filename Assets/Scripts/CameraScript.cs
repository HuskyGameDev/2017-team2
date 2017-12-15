using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject player;
    public Vector3 offset;
    public bool isStarted;

    void Start() {
        offset = transform.position - player.transform.position + new Vector3(-4.2f, -5);
    }

    void LateUpdate() {
        if (isStarted)
            transform.position = player.transform.position + offset;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject player;
    public Vector3 offset;
    public bool isStarted;

    void Start(GameObject player) {
        this.player = player;
        offset = transform.position - player.transform.position;
        isStarted = true;
    }

    void LateUpdate() {
        if (isStarted)
            transform.position = player.transform.position + offset;
    }
}

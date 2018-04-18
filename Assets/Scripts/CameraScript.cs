using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject player;
    public Vector3 offset;
    private Vector3 crossoffset;
    public bool isStarted;
    public GameObject cross;

    //Was at one point -4.2, -5. No idea why
    void Start() {
        offset = transform.position - player.transform.position + new Vector3(-5, -5);
        crossoffset = new Vector3(0, 0, 10);
    }

    void LateUpdate() {
        if (isStarted) {
            transform.position = player.transform.position + offset;
            Vector3 v3 = Input.mousePosition;
            v3.z = 10f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            cross.transform.position = v3;
            Cursor.visible = false;
        }

    }
}

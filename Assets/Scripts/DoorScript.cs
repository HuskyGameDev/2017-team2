using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
    public GameObject player;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player) {
            if (gameObject.transform.rotation == Quaternion.AngleAxis(180, Vector3.back)) {
                gameObject.transform.position += new Vector3(0.5f, .5f, 0);
                gameObject.transform.rotation = Quaternion.AngleAxis(270, Vector3.back);
            } else if (gameObject.transform.rotation == Quaternion.AngleAxis(270, Vector3.back)) {
                gameObject.transform.position += new Vector3(-0.5f, -.5f, 0);
                gameObject.transform.rotation = Quaternion.AngleAxis(180, Vector3.back);
            }
        }
    }
}

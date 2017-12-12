using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {
    public GameManager gm;
    public GameObject player;
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == player)
            gm.nextFloor();
    }

}

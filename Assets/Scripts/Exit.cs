using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {
    public GameManager gm;
    void OnTriggerEnter2D(Collider2D collision) {
        gm.nextFloor();
    }

}

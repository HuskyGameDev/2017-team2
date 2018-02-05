using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoorScript : MonoBehaviour {
    public GameObject player;
    public bool isClosed;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player) {
            DataBetweenScenes.completedGame = true;
            SceneManager.LoadScene(2);
        }
    }
}

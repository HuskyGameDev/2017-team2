using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoorScript : MonoBehaviour {
    public GameObject player;
    public bool isClosed;
    public Animator anim;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player) {
            DataBetweenScenes.completedGame = true;
            anim.SetTrigger("DoorOpened");
            StartCoroutine(waitAndEnd());     
        }
    }
    private IEnumerator waitAndEnd() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
    }
}

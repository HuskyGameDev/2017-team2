using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    public GameObject winScreen;
    public GameObject loseScreen;

	void Start () {
		if (DataBetweenScenes.completedGame) {
            loseScreen.SetActive(false);
            winScreen.SetActive(true);
        }
 	}
}

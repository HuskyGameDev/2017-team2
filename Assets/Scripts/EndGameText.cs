using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameText : MonoBehaviour {

    public Text floors;
    public Text score;

	// Use this for initialization
	void Start () {
        floors.text = "You reached floor " + (DataBetweenScenes.floorLastOn);
        score.text = "Your final score was " + DataBetweenScenes.points;
	}
}

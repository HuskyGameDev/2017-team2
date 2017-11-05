using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Andrew Stanley
 * created single method to update the player's score based on what enemy was defeated
 * Created additional text variables to update end game messages
 */
 
public class AddToCount : PlayerController {

    public Text scoreText;
    public Text floorText;

    //public int endScore;
    //public int endFloor;
    //public Text endScoreText;
    //public Text endFloorText;

    void Start () {
        score = 0;
        floor = 1;

        setScoreText();
        setFloorText();
	}
	
	void Update () {
        // Update the player's progress, both score and floors cleared
        updateProgress();
        //endScore = score;
        //endFloor = floor;
     }

    void updateProgress()
    {
        // Player defeated a small enemy
        if (Input.GetKeyDown(KeyCode.K))
        {
            score += 100;
        }

        // Player defeated a medium enemy
        if (Input.GetKeyDown(KeyCode.N))
        {
            score += 250;
        }

        // Player defeated a large enemy
        if (Input.GetKeyDown(KeyCode.V))
        {
            score += 500;
        }

        // Player cleared a floor
        if (Input.GetKeyDown(KeyCode.E))
        {
            floor += 1;
            score += 1000;
            setFloorText();
        }

        // Update text displayed to show player's progress
        setScoreText();
    }

    void setScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
        //endScoreText.text = "Your final score was " + endScore.ToString();
    }

    void setFloorText()
    {
        floorText.text = "Floor: " + floor.ToString();
        //endFloorText.text = "You completed " + endFloor.ToString() + " floor(s)";
    }
}

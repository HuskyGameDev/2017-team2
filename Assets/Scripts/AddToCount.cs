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

    void Start () {
        SetScoreText();
        SetFloorText();
	}
	
	void Update () {
        // Update the player's progress, both score and floors cleared
        UpdateProgress();
     }

    void UpdateProgress()
    {
        // Update text displayed to show player's progress
        SetScoreText();
        SetFloorText();
    }

    void SetScoreText()
    {        
        if (health <= 0)
        {
            scoreText.text = "Your final score was " + score.ToString();
        }
        else
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    void SetFloorText()
    {
        if (health <= 0)
        {
            floorText.text = "You completed " + floor.ToString() + " floor(s)"; 
        }
        else
        {
            floorText.text = "Floor: " + floor.ToString();
        }
    }
}

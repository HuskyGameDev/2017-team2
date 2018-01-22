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
            SetFloorText();
        }

        // Update text displayed to show player's progress
        SetScoreText();
    }

    void SetScoreText()
    {
        //scoreText.text = "Score: " + score.ToString();
        
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
        //floorText.text = "Floor: " + floor.ToString();
        
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

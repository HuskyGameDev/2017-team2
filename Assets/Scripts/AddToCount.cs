using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddToCount : MonoBehaviour {

    public int score;
    public int floor;
    public Text scoreText;
    public Text floorText;
   
    void Start () {
        score = 0;
        floor = 0;
        SetScoreText();
        setFloorText();
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            score = score + 10;
            SetScoreText();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            floor = floor + 1;
            setFloorText();
        }

     }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void setFloorText()
    {
        floorText.text = "Floor: " + floor.ToString();
    }
}

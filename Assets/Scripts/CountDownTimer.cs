using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountDownTimer : MonoBehaviour {


    public float time;
    public Text timerText;
    public int seconds;
    public int minutes;
    public float milliseconds;

	// Use this for initialization
	void Start () {
        time = 120.0f;
        milliseconds = 0;
        minutes = 0;
        seconds = 0;
	}
	
	// Update is called once per frame
	void Update () {

        time -= Time.deltaTime;
        minutes = (int)time / 60;
        seconds = (int) time % 60;

        if(milliseconds <= 0)
        {
            milliseconds = 99;
        }

        milliseconds =  time % 1;
        milliseconds = milliseconds * 100;
        SetCountText();

        // If time runs out, end the game
        if (minutes <= 0 && seconds <= 0 && milliseconds <= 0)
        {
            SceneManager.LoadScene(2);
        }
	}

    void SetCountText()
    {
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
    }

}

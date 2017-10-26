using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {


    public float time;
    public Text timerText;
    public int seconds;
    public int minutes;
    public float miliseconds;

	// Use this for initialization
	void Start () {
        time = 120.0f;
        miliseconds = 0;
        minutes = 0;
        seconds = 0;
	}
	
	// Update is called once per frame
	void Update () {

        time -= Time.deltaTime;
        minutes = (int)time / 60;
        seconds = (int) time % 60;

        if(miliseconds <= 0)
        {
            miliseconds = 99;
        }

        miliseconds =  time % 1;
        miliseconds = miliseconds * 100;
        SetCountText();
	}

    void SetCountText()
    {
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + miliseconds.ToString("00");
    }

}

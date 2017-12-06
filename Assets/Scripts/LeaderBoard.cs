using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeaderBoard : MonoBehaviour {

    public class Entry
    {
        public string name;
        public int score;
        public float time;
    }

    Entry[] leaderboard = new Entry[10];
    
    //initialize arrays to hold information parsed from text file
    string[] names = new string[10];
    int[] scores = new int[10];
    double[] times = new double[10];

    // variables used to properly format the time when displayed on the leaderboard
    public int milliseconds = 99;
    public int minutes = 59;
    public int seconds = 59;

    StreamReader sR = new StreamReader("Assets/leaderboard.txt");

	// Use this for initialization
	void Start () {
        // read the file until there is nothing left to read
		while (!sR.EndOfStream)
        {
            // fill teh leaderboard with the data from the text file
            for (int i = 0; i < leaderboard.Length; i++)
            {
                leaderboard[i].name = sR.ReadLine();
                leaderboard[i].score = int.Parse(sR.ReadLine());
                leaderboard[i].time = float.Parse(sR.ReadLine());
            }
        }

        sR.Close();

        for (int i = 0; i < leaderboard.Length; i++)
        {
            names[i] = leaderboard[i].name;
            scores[i] = leaderboard[i].score;

            // determine minutes, seconds, milliseconds from time values
            minutes = (int)leaderboard[i].time / 60;
            seconds = (int)leaderboard[i].time % 60;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

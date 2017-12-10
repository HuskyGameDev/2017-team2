using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour {

    public class Entry
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public float Time { get; set; }
        public Entry(string name, int score, float time)
        {
            Name = name;
            Score = score;
            Time = time;
        }
    }

    Entry[] leaderboard = new Entry[10];

    // variables used to properly format the time when displayed on the leaderboard
    private int mSeconds = 99;
    private int minutes = 59;
    private int seconds = 59;
    private string[] formatTime = new string[10];

    public Text[] names = new Text[10];
    public Text[] scores = new Text[10];
    public Text[] times = new Text[10];

    static private string path = "Assets/leaderboard.txt";

    StreamReader sR = new StreamReader(path);

	// Use this for initialization
	void Start () {

        // read the file until there is nothing left to read
		while (!sR.EndOfStream)
        {            
            // fill the virtual leaderboard with the data from the text file
            for (int i = 0; i < leaderboard.Length; i++)
            {
                string eName = sR.ReadLine();
                int eScore = int.Parse(sR.ReadLine());
                float eTime = float.Parse(sR.ReadLine());
                leaderboard[i] = new Entry(eName, eScore, eTime);
            }
        }

        sR.Close();

        // format times so that they can be read more easily on leaderboard
        for (int i = 0; i < leaderboard.Length; i++)
        {
            // determine minutes, seconds, milliseconds from time values
            minutes = (int)leaderboard[i].Time / 60;
            seconds = (int)leaderboard[i].Time % 60;
            mSeconds = (int)leaderboard[i].Time % 1;

            formatTime[i] = minutes + ":" + seconds + "." + mSeconds;
        }

        //temp printing loop
        for (int i = 0; i < leaderboard.Length; i++)
        {
            System.Diagnostics.Debug.WriteLine(formatTime[i]);
        }

        // fill the textboxes on the leaderboard with the data in the virtual leaderboard
        for (int i = 0; i < leaderboard.Length; i++)
        {
            names[i].text = leaderboard[i].Name;
            scores[i].text = leaderboard[i].Score.ToString();
            times[i].text = formatTime[i];
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

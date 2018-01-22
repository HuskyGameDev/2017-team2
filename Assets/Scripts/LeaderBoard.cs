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
    private int minutes = 59;
    private int seconds = 59;
    private int mSeconds = 999;
    private string[] formatTime = new string[10];

    // variable to take player's input from an InputField
    // InputField iFieldName;

    // arrays to print values from virtual leaderboard to screen
    public Text[] names = new Text[10];
    public Text[] scores = new Text[10];
    public Text[] times = new Text[10];

    // variables that store the passed in values of the player's final score and time
    public string newName = "test";
    public int newScore;
    public float newTime;

    // location of leaderboard text file
    static private string path = "Assets/leaderboard.txt";

    // initialize Stream Reader and StreamWriter
    StreamReader sR = new StreamReader(path);
    StreamWriter sW = new StreamWriter(path);

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

            formatTime[i] = System.String.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, mSeconds);
        }
        

        // fill the textboxes on the leaderboard with the data in the virtual leaderboard
        for (int i = 0; i < leaderboard.Length; i++)
        {
            names[i].text = leaderboard[i].Name;
            scores[i].text = leaderboard[i].Score.ToString();
            times[i].text = formatTime[i];
        }

        // get name from InputField and create a new Entry
        Entry playerEntry = new Entry(newName, newScore, newTime);

        // check to see if the new entry's score is high enough to go on the leaderboared
        if (playerEntry.Score < leaderboard[9].Score)
        {
            // score wasn't high enough, go to leaderboard without saving score
            // (do nothing)
        } else {
            // player's score was high enough to go on leaderboard
            // overwrite last Entry with new Entry
            leaderboard[9].Name = playerEntry.Name;
            leaderboard[9].Score = playerEntry.Score;
            leaderboard[9].Time = playerEntry.Time;

            // temporary entry used for swapping data during sort
            Entry tempEntry = new Entry("", 0, 0);

            // move new entry up the leaderboard until it is in the correct spot
            for (int i = leaderboard.Length; i >= 0; i--)
            {
                if (leaderboard[i].Score > leaderboard[i-1].Score)
                {
                    // put value to be replaced into temporary entry
                    tempEntry.Name = leaderboard[i - 1].Name;
                    tempEntry.Score = leaderboard[i - 1].Score;
                    tempEntry.Time = leaderboard[i - 1].Time;

                    // put new Entry into correct position
                    leaderboard[i - 1].Name = leaderboard[i].Name;
                    leaderboard[i - 1].Score = leaderboard[i].Score;
                    leaderboard[i - 1].Time = leaderboard[i].Time;

                    // put replaced Entry into correct position
                    leaderboard[i].Name = tempEntry.Name;
                    leaderboard[i].Score = tempEntry.Score;
                    leaderboard[i].Time = tempEntry.Time;
                }
            }
        }

        /*
        // begin writing new data to the text file
        for (int i = 0; i < leaderboard.Length; i++)
        {
            sW.WriteLine(leaderboard[i].Name);
            sW.WriteLine(leaderboard[i].Score.ToString());
            sW.WriteLine(leaderboard[i].Time.ToString());
        }

        sW.Close();
        */
	}
}

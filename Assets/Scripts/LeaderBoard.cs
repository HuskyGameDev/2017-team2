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
        public float Floor { get; set; }
        public Entry(string name, int score, int floor)
        {
            Name = name;
            Score = score;
            Floor = floor;
        }
    }

    Entry[] leaderboard = new Entry[10];

    // arrays to print values from virtual leaderboard to screen
    public Text[] names = new Text[10];
    public Text[] scores = new Text[10];
    public Text[] floors = new Text[10];

    // variables that store the passed in values of the player's final score and time
    string newName;
    int newScore;
    int newFloor;

    // location of leaderboard text file
    string path = "Assets/leaderboard.txt";

	// Use this for initialization
	void Start () {

        newName = GetText.entryName;
        newScore = DataBetweenScenes.points;
        newFloor = DataBetweenScenes.floorLastOn;

        // initialize Stream Reader and StreamWriter
        StreamReader sR = new StreamReader(path);

        // read the file until there is nothing left to read
        while (!sR.EndOfStream)
        {            
            // fill the virtual leaderboard with the data from the text file
            for (int i = 0; i < leaderboard.Length; i++)
            {
                string eName = sR.ReadLine();
                int eScore = int.Parse(sR.ReadLine());
                int eFloor = int.Parse(sR.ReadLine());
                leaderboard[i] = new Entry(eName, eScore, eFloor);
            }
        }

        sR.Close();

        // get name from InputField and create a new Entry
        Entry playerEntry = new Entry(newName, newScore, newFloor);

        // check to see if the new entry's score is high enough to go on the leaderboared
        if (playerEntry.Score > leaderboard[9].Score)
        {
            // player's score was high enough to go on leaderboard
            // overwrite last Entry with new Entry
            leaderboard[9].Name = playerEntry.Name;
            leaderboard[9].Score = playerEntry.Score;
            leaderboard[9].Floor = playerEntry.Floor;

            // temporary entry used for swapping data during sort
            Entry tempEntry = new Entry("", 0, 0);

            // move new entry up the leaderboard until it is in the correct spot
            for (int i = leaderboard.Length - 1; i >= 1; i--)
            {
                if (leaderboard[i].Score > leaderboard[i - 1].Score)
                {
                    // put value to be replaced into temporary entry
                    tempEntry.Name = leaderboard[i - 1].Name;
                    tempEntry.Score = leaderboard[i - 1].Score;
                    tempEntry.Floor = leaderboard[i - 1].Floor;

                    // put new Entry into correct position
                    leaderboard[i - 1].Name = leaderboard[i].Name;
                    leaderboard[i - 1].Score = leaderboard[i].Score;
                    leaderboard[i - 1].Floor = leaderboard[i].Floor;

                    // put replaced Entry into correct position
                    leaderboard[i].Name = tempEntry.Name;
                    leaderboard[i].Score = tempEntry.Score;
                    leaderboard[i].Floor = tempEntry.Floor;
                }
            }
        } else {
            
        }

        // fill the textboxes on the leaderboard with the data in the virtual leaderboard
        for (int i = 0; i < leaderboard.Length; i++)
        {
            names[i].text = leaderboard[i].Name;
            scores[i].text = leaderboard[i].Score.ToString();
            floors[i].text = leaderboard[i].Floor.ToString();
        }
        
        StreamWriter sW = new StreamWriter(path, false);

        // begin writing new data to the text file
        for (int i = 0; i < leaderboard.Length; i++)
        {
            sW.WriteLine(leaderboard[i].Name);
            sW.WriteLine(leaderboard[i].Score.ToString());
            sW.WriteLine(leaderboard[i].Floor.ToString());
        }
        

        sW.Close();
    }
}

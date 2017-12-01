using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeaderBoard : MonoBehaviour {

    //initialize arrays to hold information parsed from text file
    string[] names = new string[10];
    int[] scores = new int[10];
    double[] times = new double[10];

    //Location of text file
    string path = "Assets/Resources/leaderboard.txt";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

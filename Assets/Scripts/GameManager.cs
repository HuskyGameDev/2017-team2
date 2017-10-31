﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public BuildFloor floorScript;
    public BuildRoom boardScript;
    public int roomLength;

    // Use this for initialization
    void Start () {
        floorScript = GetComponent<BuildFloor>();
        boardScript = GetComponent<BuildRoom>();
        InitGame();
	}
    //Should transition scene to load, generate a new floor, place U
    void nextFloor() {
        floorScript.floorNumber++;
        InitGame();
    }
    /**
     * Builds the next floor of the game and displays the starting room
     */
    void InitGame() {
        BuildFloor.Room[,] floor = floorScript.buildFloor(roomLength);
        for (int i = 0; i < floorScript.lengthOfFloor; i++)
            for (int j = 0; j < floorScript.heightOfFloor; j++)
                if (floor[i, j] != null)
                    boardScript.SetupScene(roomLength, floor[i, j]);
    }
}

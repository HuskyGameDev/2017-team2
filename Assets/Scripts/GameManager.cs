using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public BuildFloor floorScript;
    public BoardManager boardScript;

    // Use this for initialization
    void Start () {
        floorScript = GetComponent<BuildFloor>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
	}
    /**
     * Builds the next floor of the game and displays the starting room
     */
    void InitGame() {
        for (int i = 0; i < floorScript.lengthOfFloor; i++)
            for (int j = 0; j < floorScript.heightOfFloor; j++)
                if (floorScript.floor[i, j] != null)
                    boardScript.SetupScene(floorScript.floor[i, j]);
    }
}

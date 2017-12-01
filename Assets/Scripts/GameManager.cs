using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public BuildFloor floorScript;
    public BuildRoom boardScript;
    public CameraScript cameraScript;
    public GameObject cam;
    public GameObject player;
    public int roomLength;
    public int numFloors;
    public bool isEndless;

    // Use this for initialization
    void Start () {
        initGame();
	}
    //Should transition scene to load, generate a new floor
    void nextFloor() {
        //loads the final floor if it's the end of story mode, increments floor
        if (++floorScript.floorNumber == numFloors + 1 && !isEndless)
            buildFinalFloor();
        else
            buildFloor();
    }
    /**
     * Shows the final floor
     * -Could be hard coded or random
     */
    void buildFinalFloor() {
        BuildFloor.Room[,] floor = floorScript.buildFinalFloor(roomLength);
        for (int i = 0; i < floorScript.lengthOfFloor; i++)
            for (int j = 0; j < floorScript.heightOfFloor; j++)
                if (floor[i, j] != null)
                    boardScript.SetupScene(roomLength, floor[i, j]);
    }
    /**
     * Called to initiate the game after player presses play
     */
    void initGame() {
        floorScript = GetComponent<BuildFloor>();
        boardScript = GetComponent<BuildRoom>();
        buildFloor();
    }
    /**
     * Builds the next floor of the game and displays the starting room
     * Places U into new floor
     */
    void buildFloor() {
        BuildFloor.Room[,] floor = floorScript.buildFloor(roomLength);
        for (int i = 0; i < floorScript.lengthOfFloor; i++)
            for (int j = 0; j < floorScript.heightOfFloor; j++)
                if (floor[i, j] != null)
                    boardScript.SetupScene(roomLength, floor[i, j]);
    }
}

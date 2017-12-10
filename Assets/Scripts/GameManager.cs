using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public BuildFloor floorScript;
    public BuildRoom boardScript;
    public GameObject cam;
    public int roomLength;
    public int numFloors;
    public bool isEndless;
    private List<List<GameObject>> objects;

    // Use this for initialization
    void Start () {
        initGame();
	}
    //Should transition scene to load, generate a new floor
    public void nextFloor() {
        destroyObjects();
        objects = new List<List<GameObject>>();
        //loads the final floor if it's the end of story mode, increments floor
        if (++floorScript.floorNumber == numFloors + 1 && !isEndless)
            buildFinalFloor();
        else
            buildFloor();
    }
    private void destroyObjects() {
        foreach (List<GameObject> room in objects)
            foreach (GameObject go in room)
                Destroy(go);
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
        objects = new List<List<GameObject>>();
        buildFloor();
        cam.GetComponent<CameraScript>().isStarted = true;
    }
    /**
     * Builds the next floor of the game and displays the starting room
     * Places U into new floor
     */
    void buildFloor() {
        BuildFloor.Room[,] floor = floorScript.buildFloor(roomLength);
        for (int i = 0; i < floorScript.lengthOfFloor; i++)
            for (int j = 0; j < floorScript.heightOfFloor; j++)
                if (floor[i, j] != null) {
                    boardScript.SetupScene(roomLength, floor[i, j]);
                    objects.Add(boardScript.getList());
                }
    }
}

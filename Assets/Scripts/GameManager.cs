using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public BuildFloor floorScript;
    public BuildRoom boardScript;
    public AudioSource song;
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
    public GameObject cam;
    public GameObject gameController;
    public int roomLength;
    public Canvas canvas;
    private List<List<GameObject>> objects;

    // Use this for initialization
    void Start () {
        initGame();

        /*
        // if Controller detected, completely disable mouse input
        if (Input.GetJoystickNames().Length > 0)
        {
            GraphicRaycaster gRC = canvas.GetComponent("GraphicRaycaster") as GraphicRaycaster;
            gRC.enabled = false;
        }
        */
	}
    //Should transition scene to load, generate a new floor
    public void nextFloor() {
        destroyObjects();
        objects = new List<List<GameObject>>();
        //loads the final floor if it's the end of story mode, increments floor
        if (++floorScript.floorNumber == DataBetweenScenes.numFloors + 1 && !DataBetweenScenes.isEndless)
            buildFinalFloor();
        else
            buildFloor();
        gameController.GetComponent<CountDownTimer>().time = 120;
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
        song.Stop();
        BuildFloor.Room[,] floor = floorScript.buildFinalFloor(roomLength);
        for (int i = 0; i < floorScript.lengthOfFloor; i++)
            for (int j = 0; j < floorScript.heightOfFloor; j++)
                if (floor[i, j] != null) {
                    objects.Add(boardScript.getList());
                    boardScript.SetupScene(roomLength, floor[i, j]);
                }
    }
    /**
     * Called to initiate the game after player presses play
     */
    void initGame() {
        floorScript = GetComponent<BuildFloor>();
        boardScript = GetComponent<BuildRoom>();
        song = GetComponent<AudioSource>();
        clip3 = Resources.Load("Music/Descension3") as AudioClip;
        clip2 = Resources.Load("Music/Descension2") as AudioClip;
        clip1 = Resources.Load("Music/Descension1") as AudioClip;
        song.Play();
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
        BuildFloor.FloorColor color = BuildFloor.FloorColor.GREY;
        song.Stop();
        for (int i = 0; i < floorScript.lengthOfFloor; i++)
            for (int j = 0; j < floorScript.heightOfFloor; j++)
                if (floor[i, j] != null) {
                    boardScript.SetupScene(roomLength, floor[i, j]);
                    objects.Add(boardScript.getList());
                    if (color == BuildFloor.FloorColor.GREY)
                        color = floor[i, j].color;
                }
        if (color == BuildFloor.FloorColor.BLUE)
            song.clip = clip1;
        else if (color == BuildFloor.FloorColor.PURPLE)
            song.clip = clip2;
        else if (color == BuildFloor.FloorColor.RED)
            song.clip = clip3;
        song.Play();
    }
    void Update() {
        if (DataBetweenScenes.devMode) {
            if (Input.GetKeyDown(KeyCode.F8))
                nextFloor();
            if (Input.GetKeyDown(KeyCode.F7))
                for (int i = 0; i < 10; i++)
                    nextFloor();
            if (Input.GetKeyDown(KeyCode.F6)) {
                floorScript.floorNumber = 0;
                DataBetweenScenes.numFloors = 10;
                SceneManager.LoadScene(1);
            }
        }
    }
}

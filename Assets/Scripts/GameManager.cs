using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public BuildFloor floorScript;
    public BuildRoom boardScript;
    public AudioSource song;
    public AudioClip blueSong;
    public AudioClip purpleSong;
    public AudioClip redSong;
    public GameObject cam;
    public GameObject gameController;
    public int roomLength;
    private List<List<GameObject>> objects;
    public GameObject player;

    // Use this for initialization
    void Start () {
        initGame();
        
	}
    //Should transition scene to load, generate a new floor
    public void nextFloor() {
        float time = gameController.GetComponent<CountDownTimer>().time; //grab this first thing

        destroyObjects();
        awardPoints(time);
        objects = new List<List<GameObject>>();
        //loads the final floor if it's the end of story mode, increments floor
        if (++floorScript.floorNumber == DataBetweenScenes.numFloors + 1 && !DataBetweenScenes.isEndless) {
            gameController.GetComponent<CountDownTimer>().timerText.enabled = false;
            buildFinalFloor();
        }
        else {
            buildFloor();
            gameController.GetComponent<CountDownTimer>().time = 120;
            gameController.GetComponent<CountDownTimer>().timerText.color = Color.white;
        }
    }
    private void awardPoints(float time) {
        if (time > 90) {
            if (boardScript.color == BuildRoom.PURPLE)
                player.GetComponent<PlayerController>().points += 70;
            else if (boardScript.color == BuildRoom.RED)
                player.GetComponent<PlayerController>().points += 80;
            else if (boardScript.color == BuildRoom.BLUE)
                player.GetComponent<PlayerController>().points += 60;
        } else if (time > 60) {
            if (boardScript.color == BuildRoom.PURPLE)
                player.GetComponent<PlayerController>().points += 40;
            else if (boardScript.color == BuildRoom.RED)
                player.GetComponent<PlayerController>().points += 45;
            else if (boardScript.color == BuildRoom.BLUE)
                player.GetComponent<PlayerController>().points += 35;
        } else if (time > 30)
            if (boardScript.color != BuildRoom.GREY)
                player.GetComponent<PlayerController>().points += 20;
            else
            if (boardScript.color != BuildRoom.GREY)
                player.GetComponent<PlayerController>().points += 10;
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
		redSong = Resources.Load("Music/Track 1") as AudioClip;
        purpleSong = Resources.Load("Music/Track 3") as AudioClip;
        blueSong = Resources.Load("Music/Descension2") as AudioClip;
        song.Play();
        song.loop = true;
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
        for (int i = 0; i < floorScript.lengthOfFloor; i++)
            for (int j = 0; j < floorScript.heightOfFloor; j++)
                if (floor[i, j] != null) {
                    boardScript.SetupScene(roomLength, floor[i, j]);
                    objects.Add(boardScript.getList());
                    if (color == BuildFloor.FloorColor.GREY)
                        color = floor[i, j].color;
                }
        AudioClip temp = song.clip;
        if (color == BuildFloor.FloorColor.BLUE) { 
            song.clip = blueSong;
            song.volume = 1f;
        }
        else if (color == BuildFloor.FloorColor.PURPLE) {
            song.clip = purpleSong;
            song.volume = 1f;
        } 
        else if (color == BuildFloor.FloorColor.RED) {
            song.clip = redSong;
            song.volume = .56f;
        }
        if (temp == null || !temp.Equals(song.clip))
            song.Play();
        DataBetweenScenes.floorLastOn = floorScript.floorNumber;
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
            if (Input.GetKeyDown(KeyCode.F5)) {
                DataBetweenScenes.godMode = true;
            }
            if (Input.GetKeyDown(KeyCode.F4)) {
                player.GetComponent<PlayerController>().speed  *= 4;
            }
        }
        if (DataBetweenScenes.godMode)
            player.GetComponent<PlayerController>().health = 100;
    }
}

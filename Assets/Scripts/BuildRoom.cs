using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildRoom : MonoBehaviour {

    [Serializable]
    public class Count {
        public int minimum;
        public int maximum;

        public Count(int min, int max) {
            minimum = min;
            maximum = max;
        }
    }

    public int columns;
    public int rows;
    public Count smallCount = new Count(10, 20);
    public Count largeCount = new Count(0, 3);
    public Count longCount = new Count(5, 10);

    public GameObject exit;
    public GameObject floor;
    public GameObject door;
    public GameObject charger;

    public GameObject wall;
    public GameObject cornerWall;

    public GameObject[] smallBlue;
    public GameObject[] smallPurple;
    public GameObject[] smallRed;

    public GameObject[] longBlue;
    public GameObject[] longPurple;
    public GameObject[] longRed;

    public GameObject[] largeBlue;
    public GameObject[] largePurple;
    public GameObject[] largeRed;

    public GameObject[] specialBlue;
    public GameObject[] specialPurple;
    public GameObject[] specialRed;



    private List<GameObject> gameObjects;

    private float dx;
    private float dy;

    private List<Vector3> doorPos;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private Vector3[,] totalPositions;
    private Boolean[,] available;

    void InitializeList() {

        gridPositions.Clear();
        totalPositions = new Vector3[columns, rows];
        available = new Boolean[columns, rows];

        for (int x = 0; x < columns; x++) {

            for (int y = 0; y < rows; y++) {

                Vector3 newPos = new Vector3(x, y, 0f);
                totalPositions[x, y] = newPos;
                
                bool doorCheck = true;
                foreach (Vector3 door in doorPos) {
                    if (newPos.Equals(door)) {
                        doorCheck = false;
                    }
                }

                // print(doorCheck);

                if (doorCheck) {
                    gridPositions.Add(newPos);
                    available[x, y] = true;
                }

            }

        }

    }

    void BoardSetup() {
        gameObjects = new List<GameObject>();
        boardHolder = new GameObject("Board").transform;

        GameObject floorInstance = Instantiate(floor, new Vector3(5 + dx, 5 + dy, 0), Quaternion.identity) as GameObject;
        floorInstance.transform.SetParent(boardHolder);
    }

    private void DestroyAllObjects() {
        foreach (GameObject go in gameObjects) {
            Destroy(go);
        }
    }

    int RandomPosition() {

        return Random.Range(0, gridPositions.Count - 1);

    }

    void LayoutSmall(GameObject[] array, Vector3[,] avail, int min, int max) {

        int count = Random.Range(min, max);

        for (int i = 0; i < count; i++) {
            float rotDegrees = Random.Range(0, 3) * 90f;
            Quaternion rotation = Quaternion.AngleAxis(rotDegrees, Vector3.back);

            int randomIndex = RandomPosition();
            Vector3 randomPos = gridPositions[randomIndex];
            Vector3 actualPos = new Vector3((randomPos.x) + 0.5f + dx, (randomPos.y) + 0.5f + dy, 0f);

            gridPositions.Remove(randomPos);
            available[(int)randomPos.x, (int)randomPos.y] = false;

            GameObject choice = array[Random.Range(0, array.Length)];
            gameObjects.Add(Instantiate(choice, actualPos, rotation));

        }

    }

    void LayoutLong(GameObject[] array, Vector3[,] avail, int min, int max) {

        int count = Random.Range(min, max);

        for (int i = 0; i < count; i++) {
            float rotDegrees = Random.Range(0, 3) * 90f;
            Quaternion rotation = Quaternion.AngleAxis(rotDegrees, Vector3.back);

            int randomIndex;
            Vector3 randomPos;
            if (rotDegrees % 180 == 0) {
                do {
                    randomIndex = RandomPosition();
                    randomPos = gridPositions[randomIndex];
                } while (!(randomPos.y < rows - 1 && available[(int)randomPos.x, (int)randomPos.y + 1]));
            } else {
                do {
                    randomIndex = RandomPosition();
                    randomPos = gridPositions[randomIndex];
                } while (!(randomPos.x < columns - 1 && available[(int)randomPos.x + 1, (int)randomPos.y]));
            }

            Vector3 actualPos;
            if (rotDegrees % 180 == 0) {
                actualPos = new Vector3((randomPos.x) + 0.5f + dx, (randomPos.y) + 1f + dy, 0f);
                gridPositions.Remove(randomPos);
                gridPositions.Remove(new Vector3(randomPos.x, randomPos.y + 1, randomPos.z));
                available[(int)randomPos.x, (int)randomPos.y] = false;
                available[(int)randomPos.x, (int)randomPos.y + 1] = false;
            } else {
                actualPos = new Vector3((randomPos.x) + 1f + dx, (randomPos.y) + 0.5f + dy, 0f);
                gridPositions.Remove(randomPos);
                gridPositions.Remove(new Vector3(randomPos.x + 1, randomPos.y, randomPos.z));
                available[(int)randomPos.x, (int)randomPos.y] = false;
                available[(int)randomPos.x + 1, (int)randomPos.y] = false;
            }

            gridPositions.Remove(randomPos);

            GameObject choice = array[Random.Range(0, array.Length)];
            gameObjects.Add(Instantiate(choice, actualPos, rotation));

        }

    }

    void LayoutLarge(GameObject[] array, Vector3[,] avail, int min, int max) {

        int count = Random.Range(min, max);

        for (int i = 0; i < count; i++) {
            int randomIndex = RandomPosition();
            Vector3 randomPos = gridPositions[randomIndex];
            Vector3 actualPos = new Vector3((randomPos.x) + 0.5f + dx, (randomPos.y) + 0.5f + dy, 0f);

            gridPositions.Remove(randomPos);

            GameObject choice = array[Random.Range(0, array.Length)];
            gameObjects.Add(Instantiate(choice, actualPos, Quaternion.AngleAxis(90f * Random.Range(0, 3), Vector3.back)));

        }

    }

    /**
     * Place the exit (Stairs) in the room
     *  -if floor color is grey, load exit as special open door with light
     */
    private void LayoutExit(Vector3[,] avail) {

    }

    /**
     * Place the charger in the room
     * 
     */
    private void RandomlyLayoutCharger(Vector3[,] avail) {

    }

    /**
     * Place U in the room
     * 
     */
    private void RandomlyLayoutPlayer(Vector3[,] avail) {

    }

    Boolean pathExists() {
        
        List<Vector3> closed = new List<Vector3>();
        List<Vector3> open = new List<Vector3>();
        List<Vector3> fringe = new List<Vector3>();

        Boolean[,] test = new Boolean[columns, rows];

        open.Add(doorPos[0]);

        while (open.Count > 0) {
            foreach ( Vector3 v in open ) {
                closed.Add(v);
                test[(int)v.x, (int)v.y] = true;

                Vector3 n = new Vector3(v.x, v.y + 1, 0f);
                if (n.y < rows && available[(int)n.x, (int)n.y] && !closed.Contains(n) && !open.Contains(n) && !fringe.Contains(n)) {
                    fringe.Add(n);
                }

                Vector3 e = new Vector3(v.x + 1, v.y, 0f);
                if (e.x < columns && available[(int)e.x, (int)e.y] && !closed.Contains(e) && !open.Contains(e) && !fringe.Contains(e)) {
                    fringe.Add(e);
                }

                Vector3 s = new Vector3(v.x, v.y - 1, 0f);
                if (s.y >= 0 && available[(int)s.x, (int)s.y] && !closed.Contains(s) && !open.Contains(s) && !fringe.Contains(s)) {
                    fringe.Add(s);
                }

                Vector3 w = new Vector3(v.x - 1, v.y, 0f);
                if (w.x >= 0 && available[(int)w.x, (int)w.y] && !closed.Contains(w) && !open.Contains(w) && !fringe.Contains(w)) {
                    fringe.Add(w);
                }
            }
            open.Clear();
            foreach (Vector3 f in fringe) {
                open.Add(f);
            }
            fringe.Clear();
        }

        foreach (Vector3 door in doorPos) {
            if (!closed.Contains(door)) {
                return false;
            }
        }

        return true;

    }

    void buildWalls(BuildFloor.Room room) {
        for (int i = 0; i < rows; i++) {
            if (room.doorWest != i)
                Instantiate(wall, new Vector3(dx - .0625f, dy + i + .5f, 0), Quaternion.identity);
            else
                Instantiate(door, new Vector3(dx - .0625f, dy + i + .5f, 0), Quaternion.identity);
            if (room.doorEast != i)
                Instantiate(wall, new Vector3(dx + 10.0625f, dy + i + .5f, 0), Quaternion.identity);
        }
        Quaternion rotation = Quaternion.AngleAxis(90, Vector3.back);
        for (int i = 0; i < columns; i++) {
            if (room.doorNorth != i)
                Instantiate(wall, new Vector3(dx + .5f + i, dy + 10.0625f, 0), rotation);
            else
                Instantiate(door, new Vector3(dx + .5f + i, dy + 10.0625f, 0), rotation);
            if (room.doorSouth != i)
                Instantiate(wall, new Vector3(dx + i + .5f, dy - .0625f, 0), rotation);
        }
        Instantiate(cornerWall, new Vector3(dx - .0625f, dy - .0625f, 0), Quaternion.identity);
        Instantiate(cornerWall, new Vector3(dx + 10.0625f, dy - .0625f, 0), Quaternion.identity);
        Instantiate(cornerWall, new Vector3(dx - .0625f, dy + 10.0625f, 0), Quaternion.identity);
        Instantiate(cornerWall, new Vector3(dx + 10.0625f, dy + 10.0625f, 0), Quaternion.identity);
    }

    /**
     * Sets the number of enemies in a room
     * currently returns : 1/20 no enemies
     *                     3/20 Little Enemies
     *                     5/20 Average Enemies
     *                     1/20 Big Enemies
     *                     2/20 Big and Little Enemies
     *                     5/20 Average and Little Enemies
     *                     3/20 All Enemy types
     * Floor Color : Blue = x1 enemies
     *               Purple = x1.66 enemies
     *               Red = x2.5 enemies
     * could be tweaked to modify difficulty if desired
     */
    private BuildFloor.Room setEnemies(BuildFloor.Room room) {
        int littleEnemies = 0, averageEnemies = 0, bigEnemies = 0;
        int roomType = Random.Range(0, 19);
        if (roomType > 0 && roomType < 4)  //Little enemies only
            littleEnemies = Random.Range(10, 20);
        else if (roomType > 3 && roomType < 9) //Avg enemies only
            averageEnemies = Random.Range(4, 9);
        else if (roomType == 9) //Big enemies only
            bigEnemies = Random.Range(1, 3);
        else if (roomType > 9 && roomType < 12) { //Big and Little enemies only
            bigEnemies = Random.Range(1, 2);
            littleEnemies = Random.Range(7, 16);
        } else if (roomType > 11 && roomType < 17) { //Avg and Little enemies only
            averageEnemies = Random.Range(3, 6);
            littleEnemies = Random.Range(5, 12);
        } else if (roomType > 16) { //All enemy types
            averageEnemies = Random.Range(2, 5);
            littleEnemies = Random.Range(4, 13);
            bigEnemies = 1;
        }
        if (room.color == BuildFloor.FloorColor.PURPLE) { //moderate increase in enemy number
            averageEnemies = averageEnemies * 5 / 3;
            littleEnemies = littleEnemies * 5 / 3;
            bigEnemies = bigEnemies * 5 / 3;
        }
        if (room.color == BuildFloor.FloorColor.RED) { //large increase in enemy number
            averageEnemies = averageEnemies * 5 / 2;
            littleEnemies = littleEnemies * 5 / 2;
            bigEnemies = bigEnemies * 5 / 2;
        }
        if (room.color == BuildFloor.FloorColor.GREY) { //no enemies
            averageEnemies = 0;
            littleEnemies = 0;
            bigEnemies = 0;
        }
        return room;
    }

    public void SetupScene(int roomLength, BuildFloor.Room room) {

        doorPos = new List<Vector3>();

        if (room.doorNorth != -1) {
            doorPos.Add(new Vector3(room.doorNorth, rows - 1, 0f));
        }
        if (room.doorEast != -1) {
            doorPos.Add(new Vector3(columns - 1, room.doorEast, 0f));
        }
        if (room.doorSouth != -1) {
            doorPos.Add(new Vector3(room.doorSouth, 0, 0f));
        }
        if (room.doorWest != -1) {
            doorPos.Add(new Vector3(0, room.doorWest, 0f));
        }

        dx = room.pos.x * 10.25f;
        dy = room.pos.y * 10.25f;

        BoardSetup();
        do {

            DestroyAllObjects();

            InitializeList();

            LayoutLong(longBlue, totalPositions, longCount.minimum, longCount.maximum);
            LayoutSmall(smallBlue, totalPositions, smallCount.minimum, smallCount.maximum);

            if (room.isExit)
                LayoutExit(totalPositions);
            if (room.hasCharger)
                RandomlyLayoutCharger(totalPositions);
            if (room.isEntrance)
                RandomlyLayoutPlayer(totalPositions);


            foreach (Vector3 door in doorPos) {
                available[(int)door.x, (int)door.y] = true;
            }

        } while (!pathExists());

        buildWalls(room);
    }
}
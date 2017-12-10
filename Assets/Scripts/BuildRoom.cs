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
    public GameObject charger;

    //Indices for each colored wall and corner 
    private const int BLUE = 0, PURPLE = 1, RED = 2, GREY = 3;
    public GameObject[] wall; // Holds all wall types, access by color above
    public GameObject[] cornerWall; // Holds all corner types, access by color above
    public GameObject[] door; // Holds all corner types, access by color above
    public GameObject[] floor; // Holds all corner types, access by color above

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


    private int color;

    private List<GameObject> gameObjects;

    private float dx;
    private float dy;

    private List<Vector3> doorPos;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private Boolean[,] available;

    void InitializeList() {

        gridPositions.Clear();
        available = new Boolean[columns, rows];

        for (int x = 0; x < columns; x++) {

            for (int y = 0; y < rows; y++) {

                Vector3 newPos = new Vector3(x, y, 0f);

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

        GameObject floorInstance = Instantiate(floor[color], new Vector3(5 + dx, 5 + dy, 0), Quaternion.identity) as GameObject;
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

    void LayoutSmall(GameObject[] array, int min, int max) {

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

    void LayoutLong(GameObject[] array, int min, int max) {

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

    void LayoutLarge(GameObject[] array, int min, int max) {

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
                } while (!LargeClear(randomPos, true));
            } else {
                do {
                    randomIndex = RandomPosition();
                    randomPos = gridPositions[randomIndex];
                } while (!LargeClear(randomPos, false));
            }

            Vector3 actualPos;
            if (rotDegrees % 180 == 0) {
                actualPos = new Vector3((randomPos.x) + 1f + dx, (randomPos.y) + 1.5f + dy, 0f);

                for (int x = (int)randomPos.x; x <= (int)randomPos.x + 1; x++) {
                    for (int y = (int)randomPos.y; y <= (int)randomPos.y + 2; y++) {
                        gridPositions.Remove(new Vector3(x, y, randomPos.z));
                        available[x, y] = false;
                    }
                }

            } else {
                actualPos = new Vector3((randomPos.x) + 1.5f + dx, (randomPos.y) + 1f + dy, 0f);

                for (int x = (int)randomPos.x; x <= (int)randomPos.x + 2; x++) {
                    for (int y = (int)randomPos.y; y <= (int)randomPos.y + 1; y++) {
                        gridPositions.Remove(new Vector3(x, y, randomPos.z));
                        available[x, y] = false;
                    }
                }
            }

            gridPositions.Remove(randomPos);

            GameObject choice = array[Random.Range(0, array.Length)];
            gameObjects.Add(Instantiate(choice, actualPos, rotation));

        }

    }

    Boolean LargeClear(Vector3 pos, Boolean vert) {
        // pos is bottom left corner of object
        if (vert) {
            if (pos.y > rows - 3 || pos.x > columns - 2) {
                return false;
            }

            for (int x = (int)pos.x; x <= (int)pos.x + 1; x++) {
                for (int y = (int)pos.y; y <= (int)pos.y + 2; y++) {
                    if (!available[x, y]) {
                        return false;
                    }
                }
            }

            return true;
        } else {
            if (pos.y > rows - 2 || pos.x > columns - 3) {
                return false;
            }

            for (int x = (int)pos.x; x <= (int)pos.x + 2; x++) {
                for (int y = (int)pos.y; y <= (int)pos.y + 1; y++) {
                    if (!available[x, y]) {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    /**
     * Place the charger in the room
     * 
     */
    private void RandomlyLayoutCharger(GameObject[] array) {
        float rotDegrees = Random.Range(0, 3) * 90f;
        Quaternion rotation = Quaternion.AngleAxis(rotDegrees, Vector3.back);

        int randomIndex;
        Vector3 randomPos;
        do {
            randomIndex = RandomPosition();
            randomPos = gridPositions[randomIndex];
            print(randomPos.ToString());
        } while (!SpecialClear(randomPos));

        Vector3 actualPos;
        actualPos = new Vector3((randomPos.x) + 1.5f + dx, (randomPos.y) + 1.5f + dy, 0f);

        for (int x = (int)randomPos.x; x <= (int)randomPos.x + 2; x++) {
            for (int y = (int)randomPos.y; y <= (int)randomPos.y + 2; y++) {
                gridPositions.Remove(new Vector3(x, y, randomPos.z));
                available[x, y] = false;
            }
        }

        GameObject choice = array[Random.Range(0, array.Length)];
        gameObjects.Add(Instantiate(choice, actualPos, rotation));

    }

    Boolean SpecialClear(Vector3 pos) {
        // pos is bottom left corner of object
        if (pos.y > rows - 3 || pos.x > columns - 3) {
            return false;
        }

        for (int x = (int)pos.x; x <= (int)pos.x + 2; x++) {
            for (int y = (int)pos.y; y <= (int)pos.y + 2; y++) {
                if (!available[x, y]) {
                    return false;
                }
            }
        }

        return true;
    }

    /**
     * Place U in the room
     * 
     */
    private void RandomlyLayoutPlayer(Vector3[,] avail) {

    }

    /**
     * Place the exit (Stairs) in the room
     *  -if floor color is grey, load exit as special open door with light
     */
    private void LayoutExit(Vector3[,] avail) {

    }


    Boolean pathExists() {

        List<Vector3> closed = new List<Vector3>();
        List<Vector3> open = new List<Vector3>();
        List<Vector3> fringe = new List<Vector3>();

        Boolean[,] test = new Boolean[columns, rows];

        open.Add(doorPos[0]);

        while (open.Count > 0) {
            foreach (Vector3 v in open) {
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

        Quaternion eastRotation = Quaternion.identity;
        Quaternion westRotation = Quaternion.AngleAxis(180, Vector3.back);
        Quaternion southRotation = Quaternion.AngleAxis(90, Vector3.back);
        Quaternion northRotation = Quaternion.AngleAxis(270, Vector3.back);

        for (int i = 0; i < rows; i++) {
            if (room.doorWest != i) {
                // Left
                Instantiate(wall[color], new Vector3(dx - .0625f, dy + i + .5f, 0), westRotation);
            } else {
                // Left
                Instantiate(door[color], new Vector3(dx - .0625f, dy + i + .5f, 0), westRotation);
            }
            if (room.doorEast != i) {
                // Right
                Instantiate(wall[color], new Vector3(dx + 10.0625f, dy + i + .5f, 0), eastRotation);
            }
        }

        for (int i = 0; i < columns; i++) {
            if (room.doorNorth != i) {
                // Top
                Instantiate(wall[color], new Vector3(dx + .5f + i, dy + 10.0625f, 0), northRotation);
            } else {
                // Top
                Instantiate(door[color], new Vector3(dx + .5f + i, dy + 10.0625f, 0), northRotation);
            }
            if (room.doorSouth != i) {
                // Bottom
                Instantiate(wall[color], new Vector3(dx + i + .5f, dy - .0625f, 0), southRotation);
            }
        }
        Instantiate(cornerWall[color], new Vector3(dx - .0625f, dy - .0625f, 0), Quaternion.AngleAxis(180, Vector3.back)); // Bottom Left
        Instantiate(cornerWall[color], new Vector3(dx + 10.0625f, dy - .0625f, 0), Quaternion.AngleAxis(90, Vector3.back)); // Bottom Right
        Instantiate(cornerWall[color], new Vector3(dx - .0625f, dy + 10.0625f, 0), Quaternion.AngleAxis(270, Vector3.back)); // Top Left
        Instantiate(cornerWall[color], new Vector3(dx + 10.0625f, dy + 10.0625f, 0), Quaternion.identity); // Top Right
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
        int littleEnemies = 0;
        int averageEnemies = 0;
        int bigEnemies = 0;

        int roomType = Random.Range(0, 19);

        if (roomType > 0 && roomType < 4) { //Little enemies only
            littleEnemies = Random.Range(10, 20);
        } else if (roomType > 3 && roomType < 9) { //Avg enemies only
            averageEnemies = Random.Range(4, 9);
        } else if (roomType == 9) { //Big enemies only
            bigEnemies = Random.Range(1, 3);
        } else if (roomType > 9 && roomType < 12) { //Big and Little enemies only
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
        GameObject[] smalls;
        GameObject[] longs;
        GameObject[] larges;
        GameObject[] specials;

        if (room.color == BuildFloor.FloorColor.BLUE) {
            color = BLUE;
        } else if (room.color == BuildFloor.FloorColor.PURPLE) {
            color = PURPLE;
        } else if (room.color == BuildFloor.FloorColor.RED) {
            color = RED;
        }

        color = PURPLE;

        if (color == BLUE) {
            smalls = smallBlue;
            longs = longBlue;
            larges = largeBlue;
            specials = specialBlue;
        } else if (color == PURPLE) {
            smalls = smallPurple;
            longs = longPurple;
            larges = largePurple;
            specials = specialPurple;
        } else if (color == RED) {
            smalls = smallRed;
            longs = longRed;
            larges = largeRed;
            specials = specialRed;
        } else {
            smalls = smallRed;
            longs = longRed;
            larges = largeRed;
            specials = specialRed;
        }

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

            if (room.hasCharger) {
                RandomlyLayoutCharger(specials);
            }

            LayoutLarge(larges, largeCount.minimum, largeCount.maximum);
            LayoutLong(longs, longCount.minimum, longCount.maximum);
            LayoutSmall(smalls, smallCount.minimum, smallCount.maximum);

            if (room.isExit) {
                // LayoutExit();
            }
            if (room.isEntrance) {
                // RandomlyLayoutPlayer();
            }

            foreach (Vector3 door in doorPos) {
                available[(int)door.x, (int)door.y] = true;
            }

        } while (!pathExists());

        buildWalls(room);
    }
}

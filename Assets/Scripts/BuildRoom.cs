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

    private class Graph {

        protected Vertex start;
        protected Vertex end;
        protected List<Vertex> vertices;
        protected List<Edge> edges;

        protected class Edge {
            List<Vertex> nodes;

            protected Edge(Vertex x, Vertex y) {
                nodes = new List<Vertex> { x, y };
            }

            protected Vertex GetNeighbor(Vertex v) {
                return v.Equals(nodes[0]) ? (Vertex)nodes[1] : (Vertex)nodes[0];
            }
        }

        protected class Vertex {
            List<Edge> edges;
            String id;

            protected Vertex(int x, int y) {
                id = x + " " + y;
                edges = new List<Edge>();
            }

            protected void AddEdge(Edge e) {
                edges.Add(e);
            }
        }

        public Graph() {

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
    public GameObject wall;
    public GameObject cornerWall;
    public GameObject[] smallObjects;
    public GameObject[] largeObjects;
    public GameObject[] longObjects;
    public GameObject[] specialObjects;

    private float dx;
    private float dy;

    private List<int> doorPos;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private Vector3[,] totalPositions;
    private Boolean[,] available;

    void InitializeList() {

        gridPositions.Clear();
        totalPositions = new Vector3[columns, rows];
        available = new Boolean[columns, rows]; //didn't compile until I added this

        for (int x = 0; x < columns; x++) {

            for (int y = 0; y < rows; y++) {
                
                Vector3 newPos = new Vector3(x, y, 0f);
                gridPositions.Add(newPos);
                totalPositions[x, y] = newPos;
                available[x, y] = true;

            }

        }

    }

    void BoardSetup() {
        boardHolder = new GameObject("Board").transform;

        GameObject floorInstance = Instantiate(floor, new Vector3(5 + dx, 5 + dy, 0), Quaternion.identity) as GameObject;
        floorInstance.transform.SetParent(boardHolder);
    }

    int RandomPosition() {

        return Random.Range(0, gridPositions.Count);

    }

    void RandomlyLayoutSmall(GameObject[] array, Vector3[,] avail, int min, int max) {

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
            Instantiate(choice, actualPos, rotation);

        }

    }

    void RandomlyLayoutLong(GameObject[] array, Vector3[,] avail, int min, int max) {

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
                } while (randomPos.y >= rows - 1);
            } else {
                do {
                    randomIndex = RandomPosition();
                    randomPos = gridPositions[randomIndex];
                } while (randomPos.x >= columns - 1);
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
            Instantiate(choice, actualPos, rotation);

        }

    }

    void RandomlyLayoutLarge(GameObject[] array, Vector3[,] avail, int min, int max) {

        int count = Random.Range(min, max);

        for (int i = 0; i < count; i++) {
            int randomIndex = RandomPosition();
            Vector3 randomPos = gridPositions[randomIndex];
            Vector3 actualPos = new Vector3((randomPos.x) + 0.5f + dx, (randomPos.y) + 0.5f + dy, 0f);

            gridPositions.Remove(randomPos);

            GameObject choice = array[Random.Range(0, array.Length)];
            Instantiate(choice, actualPos, Quaternion.AngleAxis(90f * Random.Range(0, 3), Vector3.back));

        }

    }

    void RandomlyLayoutSpecial(GameObject[] array, Vector3[,] avail, int min, int max) {

        int count = Random.Range(min, max);

        for (int i = 0; i < count; i++) {
            int randomIndex = RandomPosition();
            Vector3 randomPos = gridPositions[randomIndex];
            Vector3 actualPos = new Vector3((randomPos.x) + 0.5f, (randomPos.y) + 0.5f, 0f);

            gridPositions.Remove(randomPos);

            GameObject choice = array[Random.Range(0, array.Length)];
            Instantiate(choice, actualPos, Quaternion.AngleAxis(90f * Random.Range(0, 3), Vector3.back));

        }

    }

    Graph constructGraph() {
        Graph graph = new Graph();
        return graph;
    }

    Boolean pathExists() {

        Graph graph = constructGraph();

        return true;

    }

    public void SetupScene(int roomLength, BuildFloor.Room room) {

        columns = roomLength;
        rows = roomLength;

        if (room.doorNorth != -1) {

        }

        dx = room.pos.x * 10.25f;
        dy = room.pos.y * 10.25f;

        BoardSetup();

        do {

            InitializeList();
            RandomlyLayoutLong(longObjects, totalPositions, longCount.minimum, longCount.maximum);
            RandomlyLayoutSmall(smallObjects, totalPositions, smallCount.minimum, smallCount.maximum);

        } while (!pathExists());

        buildWalls(room);

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
}

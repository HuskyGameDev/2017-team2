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

        private Vertex start;
        private Vertex end;
        private List<Vertex> vertices;
        private Vertex[,] vertArray;

        public class Vertex{
            List<Vertex> neighbors;
            public int x;
            public int y;

            public Vertex(int x_, int y_) {
                x = x_;
                y = y_;

                neighbors = new List<Vertex>();
            }

            public List<Vertex> Neighbors() {
                return neighbors;
            }

            public void AddNeighbor(Vertex v) {
                neighbors.Add(v);
            }
        }

        public Graph(Boolean[,] a, int r, int c) {

            vertices = new List<Vertex>();
            vertArray = new Vertex[r, c];

            for (int i = 0; i < r; i++) {
                for (int j = 0; j < c; j++) {
                    if (a[i, j]) {
                        Vertex v = new Vertex(i, j);
                        vertices.Add(v);
                        vertArray[i, j] = v;
                    }
                }
            }

            foreach (Vertex v in vertices) {
                if (v.y != c-1 && a[v.x, v.y + 1]) {
                    Vertex north = vertArray[v.x, v.y + 1];
                    v.AddNeighbor(north);
                }
                if (v.y != 0 && a[v.x, v.y - 1]) {
                    Vertex south = vertArray[v.x, v.y - 1];
                    v.AddNeighbor(south);
                }
                if (v.x != r-1 && a[v.x + 1, v.y]) {
                    Vertex east = vertArray[v.x + 1, v.y];
                    v.AddNeighbor(east);
                }
                if (v.x != 0 && a[v.x - 1, v.y]) {
                    Vertex west = vertArray[v.x - 1, v.y];
                    v.AddNeighbor(west);
                }
            }
        }

        public List<Vertex> Vertices() {
            return vertices;
        }

        public Vertex[,] VertArray() {
            return vertArray;
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
    public GameObject[] smallObjects;
    public GameObject[] largeObjects;
    public GameObject[] longObjects;
    public GameObject[] specialObjects;

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

    Graph.Vertex findMin(List<Graph.Vertex> l, Vector3 s) {
        int min = int.MaxValue;
        Graph.Vertex minVertex = l[0];

        foreach (Graph.Vertex v in l) {
            if (System.Math.Abs((int)s.x - v.x) + System.Math.Abs((int)s.y - v.y) < min) {
                min = System.Math.Abs((int)s.x - v.x) + System.Math.Abs((int)s.y - v.y);
                minVertex = v;
            }
        }

        return minVertex;
    }

    Boolean pathExists(Vector3 source, Vector3 target) {

        Graph graph = new Graph(available, rows, columns);

        List<Graph.Vertex> list = new List<Graph.Vertex>();

        int[,] dist = new int[rows, columns];
        Graph.Vertex[,] prev = new Graph.Vertex[rows, columns];

        foreach (Graph.Vertex v in graph.Vertices()) {
            dist[v.x, v.y] = int.MaxValue;
            prev[v.x, v.y] = null;
            list.Add(v);
        }

        dist[(int)source.x, (int)source.y] = 0;

        while (list.Count != 0) {
            Graph.Vertex u = findMin(list, source);

            list.Remove(u);

            if (u.x == (int)target.x && u.y == (int)target.y) {
                return true;
            }

            foreach (Graph.Vertex n in u.Neighbors()) {
                int alternative = dist[u.x, u.y] + 1;
                if (alternative < dist[n.x, n.y]) {
                    dist[n.x, n.y] = alternative;
                    prev[n.x, n.y] = u;
                }
            }
        }

        return false;

    }

    Boolean pathExists() {

        Boolean path = true;

        foreach (Vector3 door in doorPos) {
            print(door.x + ", " + door.y);
        }

        foreach (Vector3 door1 in doorPos) {
            foreach (Vector3 door2 in doorPos) {
                
                if (door1.Equals(door2)) {
                    continue;
                } else {
                    if (!pathExists(door1, door2)) {
                        path = false;
                        break;
                    }
                }
            }
            if (!path) {
                break;
            }
        }

        return path;

    }

    public void SetupScene(int roomLength, BuildFloor.Room room) {

        columns = roomLength;
        rows = roomLength;

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

            InitializeList();
            RandomlyLayoutLong(longObjects, totalPositions, longCount.minimum, longCount.maximum);
            RandomlyLayoutSmall(smallObjects, totalPositions, smallCount.minimum, smallCount.maximum);

        } while (!pathExists());

    }
}

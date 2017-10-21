using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count {
        public int minimum;
        public int maximum;

        public Count(int min, int max) {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 10;
    public int rows = 10;
    public Count smallCount = new Count(10, 50);
    public Count largeCount = new Count(0, 3);
    public Count longCount = new Count(5, 25);

    public GameObject exit;
    public GameObject floor;
    public GameObject[] smallObjects;
    public GameObject[] largeObjects;
    public GameObject[] longObjects;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private Vector3[,] totalPositions;

    void InitializeList() {

        gridPositions.Clear();
        totalPositions = new Vector3[columns, rows];

        for (int x = 0; x < columns; x++) {

            for (int y = 0; y < rows; y++) {

                Vector3 newPos = new Vector3(x, y, 0f);
                gridPositions.Add(newPos);
                totalPositions[x, y] = newPos;

            }

        }

    }

    void BoardSetup() {
        boardHolder = new GameObject("Board").transform;

        GameObject preBoard = floor;
        GameObject floorInstance = Instantiate(preBoard, new Vector3(10, 10, 0), Quaternion.identity) as GameObject;
        floorInstance.transform.SetParent(boardHolder);
    }

    int RandomPosition() {

        return Random.Range(0, gridPositions.Count);

    }

    void RandomlyLayout(GameObject[] array, Vector3[,] avail, int min, int max) {

        int count = Random.Range(min, max);

        for (int i = 0; i < count; i++) {
            int randomIndex = RandomPosition();
            Vector3 randomPos = gridPositions[randomIndex];
            Vector3 actualPos = new Vector3((2 * randomPos.x) + 1, (2 * randomPos.y) + 1, 0f);

            gridPositions.Remove(randomPos);

            GameObject choice = array[Random.Range(0, array.Length)];
            Instantiate(choice, actualPos, Quaternion.identity);

        }

    }

    public void SetupScene(int level) {

        BoardSetup();
        InitializeList();

        RandomlyLayout(smallObjects, totalPositions, smallCount.minimum, smallCount.maximum);

        print(smallCount.minimum);

    }
}

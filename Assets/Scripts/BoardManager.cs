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
    public Count smallCount = new Count(10, 20);
    public Count largeCount = new Count(0, 3);
    public Count longCount = new Count(5, 10);

    public GameObject exit;
    public GameObject floor;
    public GameObject[] smallObjects;
    public GameObject[] largeObjects;
    public GameObject[] longObjects;
    public GameObject[] specialObjects;

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
        GameObject floorInstance = Instantiate(preBoard, new Vector3(5, 5, 0), Quaternion.identity) as GameObject;
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
            Vector3 actualPos = new Vector3((randomPos.x)+0.5f, (randomPos.y)+0.5f, 0f);

            gridPositions.Remove(randomPos);

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
                actualPos = new Vector3((randomPos.x) + 0.5f, (randomPos.y) + 1f, 0f);
                gridPositions.Remove(randomPos);
                gridPositions.Remove(new Vector3(randomPos.x, randomPos.y + 1, randomPos.z));
            } else {
                actualPos = new Vector3((randomPos.x) + 1f, (randomPos.y) + 0.5f, 0f);
                gridPositions.Remove(randomPos);
                gridPositions.Remove(new Vector3(randomPos.x + 1, randomPos.y, randomPos.z));
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
            Vector3 actualPos = new Vector3((randomPos.x) + 0.5f, (randomPos.y) + 0.5f, 0f);

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

    public void SetupScene(int level) {

        BoardSetup();
        InitializeList();

        RandomlyLayoutLong(longObjects, totalPositions, longCount.minimum, longCount.maximum);
        RandomlyLayoutSmall(smallObjects, totalPositions, smallCount.minimum, smallCount.maximum);
        
    }
}

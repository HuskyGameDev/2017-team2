using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    //Load game with a given number of floors
    public void LoadByIndex(int numFloors)
    {
        DataBetweenScenes.numFloors = numFloors;
        SceneManager.LoadScene(1);
    }
    //Load game in endless mode
    public void LoadEndless() {
        DataBetweenScenes.isEndless = true;
        SceneManager.LoadScene(1);
    }
}
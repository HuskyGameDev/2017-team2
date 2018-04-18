using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{

    //For loading back into the menus
    public void LoadMenu() {
        SceneManager.LoadScene(0);
    }

    //For reloading the game with previous parameters
    public void ReloadGame() {
        SceneManager.LoadScene(1);
    }

    //Load game with a given number of floors
    public void LoadByIndex(int numFloors)
    {
        DataBetweenScenes.isEndless = false;
        DataBetweenScenes.numFloors = numFloors;
        
        SceneManager.LoadScene(1);
    }

    //Load game in endless mode
    public void LoadEndless() {
        DataBetweenScenes.isEndless = true;
        
        SceneManager.LoadScene(1);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V) && Input.GetKeyDown(KeyCode.B) && Input.GetKeyDown(KeyCode.N))
            DataBetweenScenes.devMode = true;
    }
}
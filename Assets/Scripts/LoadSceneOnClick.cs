using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{

    public GameObject fadingObject;
    public Image black;
    public Animator anim;

    //For loading back into the menus
    public void LoadMenu() {
        //Fade();
        SceneManager.LoadScene(0);
    }
    //For reloading the game with previous parameters
    public void ReloadGame() {
        Fade();
        SceneManager.LoadScene(1);
    }
    //Load game with a given number of floors
    public void LoadByIndex(int numFloors)
    {
        DataBetweenScenes.isEndless = false;
        DataBetweenScenes.numFloors = numFloors;

        Fade();
        SceneManager.LoadScene(1);
    }
    //Load game in endless mode
    public void LoadEndless() {
        DataBetweenScenes.isEndless = true;

        Fade();
        SceneManager.LoadScene(1);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V) && Input.GetKeyDown(KeyCode.B) && Input.GetKeyDown(KeyCode.N))
            DataBetweenScenes.devMode = true;
    }

    void Fade ()
    {
        StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        fadingObject.SetActive(false);
    }
}
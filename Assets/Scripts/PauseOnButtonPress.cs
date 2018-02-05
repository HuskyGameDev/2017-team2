using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOnButtonPress : MonoBehaviour
{

    public Transform canvas;
    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Pause"))
        {
            if (!canvas.gameObject.activeInHierarchy)
            {
                Time.timeScale = 0;
                canvas.gameObject.SetActive(true);
                player.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                canvas.gameObject.SetActive(false);
                player.SetActive(true);
            }

        }

        if (!canvas.gameObject.activeInHierarchy)
        {
            Time.timeScale = 1;
            player.SetActive(true);
        }
    }
}


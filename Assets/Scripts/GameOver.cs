using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameOver : AddToCount {

    //updates the counter displayed at the top of the screen showing how much HP the player has remaining
    public Text lifeText;

    //Messages displayed on the game over screen that show the progress made
    public Text endScoreText;
    public Text endFloorText;

    private void Start()
    {
        life = 100;
        setLifeText();
    }

    // Update is called once per frame
    void Update ()
    {
        setLifeText();
        updateHP();
	}

    void setLifeText()
    {
        lifeText.text = "HP: " + life.ToString();
    }

    void updateHP()
    {
        // For testing purposes, the player's life can be controlled using keys to simulate being healed and damaged by each of the three enemy types
        // Player is hit by a small enemy
        if (Input.GetKeyDown(KeyCode.L))
        {
            life -= 5;
        }

        // Player is hit by medium-sized enemy or its bullet 
        if (Input.GetKeyDown(KeyCode.M))
        {
            life -= 10;
        }

        // Player is hit by large enemy or its beam
        if (Input.GetKeyDown(KeyCode.B))
        {
            life -= 25;
        }

        // Player steps on/near a healing tile
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (100 - life <= 50)
            {
                life = 100;
            }
            // else clause makes sure player can't have more than 100 HP
            else
            {
                life += 50;
            }
        }

        setLifeText();

        // check for death
        if (life <= 0)
        {
            gameOver();
        }
    }

    // This method is called when the player's HP is reduced to 0
    void gameOver()
    {
        /*
        // May remove next 2 lines
        // Forces the player to face downward
        mouse_pos.x = 0;
        mouse_pos.y = -10;
        */

        SceneManager.LoadScene(2);
    }
}

﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/*
 * Christina Anderson
 * Codey Walker
 * Main controller for player behavior. Currently, it allows the player to move the sprite around and follows mouse direction
 * Added gun and melee attack functions to this script - Codey
 */


/* Structure for storing a bullet with the mouse position at the time the bullet is created */
struct bulletStruct
{
    private GameObject bullet;
    private Vector3 pos;
    private Collider2D bulletAtk;

    public void setObj(GameObject newBullet)
    {
        bullet = newBullet;
    }

    public void setPos(Vector3 newPos)
    {
        pos = newPos;
    }

    public GameObject getObj()
    {
        return bullet;
    }

    public Vector3 getPos()
    {
        return pos;
    }

    public void setColliderVar(Collider2D col)
    {
        bulletAtk = col;
    }
    public void setCollider(bool set)
    {
        if(set)
        {
            bulletAtk.enabled = true;
        }

        else
        {
            bulletAtk.enabled = false;
        }
    }
}

public class PlayerController : MonoBehaviour
{

    //Stores a reference to the Rigidbody2D component required to use 2D Physics.
    public Rigidbody2D rb2d;

    //Stores the position of the mouse
    private Vector3 mouse_pos;

    //Transform object for player
    public Transform Player;

    //Stores the position of the object
    private Vector3 object_pos;

    //Stores angle needed to turn
    private float angle;

    //stores amount of health character has
    public float health;

    public int score;
    public int floor;
    public Text healthText;

    public float speed;

    // Projectiles
    public GameObject bulletPrefab;
    public AnimationClip bulletAnim;
    private List<bulletStruct> bullets = new List<bulletStruct>();
    private float bulletSpeed;
    private int ableToShoot = 0;
    //public Collider2D bulletAttack;

    // Object for slashing
    private int wait = 10;
    private bool attacking;
    public Collider2D meleeAttack;


    // Use this for initialization
    void Start()
    {
        health = 100;

        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        Player = GetComponent<Transform>();
       

        score = 0;
        floor = 1;

        // Set speed of bullet
        bulletSpeed = 20;
        //bulletAttack.enabled = false;

        // Set melee attack stuff
        meleeAttack.enabled = false;
        attacking = false;

       // anim.animation = U_Walking;
    }

    //Called every frame
    void Update()
    {
		if (health < 0) {
			Destroy(gameObject);
			print ("RIP");
		}
        //Enter mouse mosition
        mouse_pos = Input.mousePosition;
        mouse_pos.z = -10;

        //Enter player position
        object_pos = Camera.main.WorldToScreenPoint(Player.position);

        //Find different coordinates between mouse and player position
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;

        //Calculate angle between mouse and player position
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;

        //Rotate player
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        //Stores horizontal and vertical coordinates
        float moveHorizontal = 0;
        float moveVertical = 0;

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Change position of player
        rb2d.MovePosition(rb2d.position + speed * movement * Time.fixedDeltaTime);

        UpdateHP();

        /* Call methods to handle shooting and slashing */
        shoot();
        slash();
    }

    // Method used to handle shooting projectiles
    private void shoot()
    {

        // Create a new bullet with the current mouse position
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (ableToShoot == 0 && !attacking)
            {

                bulletStruct newBullet = new bulletStruct();
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                bullet.AddComponent<BoxCollider2D>();
                bullet.GetComponent<BoxCollider2D>().isTrigger = true;
                bullet.AddComponent<bulletAttack>();
                bullet.GetComponent<bulletAttack>().shooter = Player.gameObject;

                Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 pos = (Input.mousePosition - sp).normalized;

                newBullet.setPos(pos);
                newBullet.setObj(bullet);
                newBullet.setColliderVar(bullet.GetComponent<BoxCollider2D>());
                newBullet.setCollider(true);
                bullets.Add(newBullet);
                ableToShoot++;
            }
        }

        // Used to limit the amount of bullets *Needs to update when animation implemented*
        if (ableToShoot == 0 || ableToShoot == 10)
        {
            ableToShoot = 0;
        }

        else
        {
            ableToShoot++;
        }

        // For every bullet on screen move towards the mouse position it was shot at
        for (int i = 0; i < bullets.Count; i++)
        {
            GameObject movingBullet = bullets[i].getObj();

			if (movingBullet != null) {

				movingBullet.transform.Translate (bullets [i].getPos () * Time.deltaTime * bulletSpeed);
				//bulletAttack.enabled = true;
           

				Vector3 bulletPos = Camera.main.WorldToScreenPoint (movingBullet.transform.position);

				// Remove bullet if off screen
				if (bulletPos.y >= Screen.height || bulletPos.y <= 0 || bulletPos.x >= Screen.width || bulletPos.x <= 0) {
					DestroyObject (movingBullet);
					bullets.Remove (bullets [i]);
				}
			}
				
        }
    }

    private void slash()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1) && !attacking)
        {
            attacking = true;
            meleeAttack.enabled = true;
        }

        if (attacking)
        {

            if (wait > 0)
            {
               
                wait--;
            }

            else
            {
                attacking = false;
                meleeAttack.enabled = false;
                wait = 20;
            }
        }
    }

    void UpdateHP()
    {
        // For testing purposes, the player's health can be controlled using keys to simulate being healed and damaged by each of the three enemy types
        // Player is hit by a small enemy
        if (Input.GetKeyDown(KeyCode.L))
        {
            health -= 5;
        }

        // Player is hit by medium-sized enemy or its bullet 
        if (Input.GetKeyDown(KeyCode.M))
        {
            health -= 10;
        }

        // Player is hit by large enemy or its beam
        if (Input.GetKeyDown(KeyCode.B))
        {
            health -= 25;
        }

        // Player steps on/near a healing tile
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (100 - health <= 50)
            {
                health = 100;
            }
            // else clause makes sure player can't have more than 100 HP
            else
            {
                health += 50;
            }
        }

        SetHealthText();

        // check for death
        if (health <= 0)
        {
            GameOver();
        }
    }

    void SetHealthText()
    {
        healthText.text = "HP: " + health.ToString();
    }

    // This method is called when the player's HP is reduced to 0
    void GameOver()
    {
        SceneManager.LoadScene(2);
    }
	void Hit(int dmg)
	{
		health -= dmg;
	}
}
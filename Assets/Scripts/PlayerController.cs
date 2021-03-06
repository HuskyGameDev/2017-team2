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
 * Added controller support to player actions (movement, attacking, aiming) - Andrew S
 */


/* Structure for storing a bullet with the mouse position at the time the bullet is created */
struct bulletStruct {
    private GameObject bullet;
    private Vector3 pos;
    private Collider2D bulletAtk;

    public void setObj(GameObject newBullet) {
        bullet = newBullet;
    }

    public void setPos(Vector3 newPos) {
        pos = newPos;
    }

    public GameObject getObj() {
        return bullet;
    }

    public Vector3 getPos() {
        return pos;
    }

    public void setColliderVar(Collider2D col) {
        bulletAtk = col;
    }
    public void setCollider(bool set) {
        if (set) {
            bulletAtk.enabled = true;
        } else {
            bulletAtk.enabled = false;
        }
    }
}

public class PlayerController : MonoBehaviour {

    public GameManager gameManager;

    //Store life objects
    public GameObject[] lives;
    public int numLives;

    //Store Key UI element
    public GameObject key;

    //Stores a reference to the Rigidbody2D component required to use 2D Physics.
    public Rigidbody2D rb2d;

    //Stores the position of the mouse
    private Vector3 mouse_pos;

    // stores direction of the right stick for aiming purposes
    private Vector2 rStick;

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
	public bool freeze;

    // Projectiles
    public GameObject bulletPrefab;
    private List<bulletStruct> bullets = new List<bulletStruct>();
    private float bulletSpeed;
    private int ableToShoot = 0;
    //public Collider2D bulletAttack;

    // Object for slashing
    private int wait = 10;
    private bool attacking;
    public Collider2D meleeAttack;
    public Transform bulletSpawn;

    public AudioClip bulletSound;
    public AudioClip slashSound;
    public AudioClip damageSound;

    private AudioSource audioSource;
    //private AudioClip bulletSound;

    // if the player has the key for the level
    public bool hasKey;

    //Number of points the player has
    public int points;

    public Text pointsText;

	//Animation script
	private AnimationSetter anim;

	private bool canAttack = true;

    // Use this for initialization
    void Start() {
        if (DataBetweenScenes.isEndless) {
            Destroy(lives[0]);
            Destroy(lives[1]);
            Destroy(lives[2]);
            lives[0] = null;
            lives[1] = null;
            lives[2] = null;
            numLives = 0;
        }
        else
            numLives = 3;
        key.SetActive(false);
        points = 0;
        health = 100;
		freeze = false;
		anim = GetComponent<AnimationSetter> ();
        
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D>();
        Player = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();


        score = 0;
        floor = 1;

        // Set speed of bullet
        bulletSpeed = 20;
        //bulletAttack.enabled = false;

        // Set melee attack stuff
        meleeAttack.enabled = false;
        attacking = false;

        // anim.animation = U_Walking;

        //Set bullet sound
        bulletSound = Resources.Load("SFX/Plasma Gun") as AudioClip;
    }

    //Called every frame
    void Update() {

		pointsText.text = points.ToString();

        if (DataBetweenScenes.gamePad == true) {
            Cursor.visible = false;

            rStick.x = Input.GetAxis("rStickX");
            rStick.y = Input.GetAxis("rStickY");

            if (rStick.magnitude > 0.1f) {
                // get new angle of the player based on position of the right analog stick
                angle = Mathf.Atan2(rStick.y, rStick.x) * Mathf.Rad2Deg;

                // Rotate player based on angle, also keep player rotated in new direction until it is changed again
                transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            }

        } else {
            Cursor.visible = true;

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
        }

        //Stores horizontal and vertical coordinates
        float moveHorizontal = 0;
        float moveVertical = 0;

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Change position of player
        rb2d.position += speed * movement * Time.fixedDeltaTime;

        pointsText.text = points.ToString();

        UpdateHP();

        /* Call methods to handle shooting and slashing */
        Shoot();
        Slash();
    }

    // Method used to handle shooting projectiles
    private void Shoot() {
        if (DataBetweenScenes.gamePad) {
            if (Input.GetAxis("primaryAtk") == 1) {
                if (ableToShoot == 0 && !attacking) {

                    GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, this.transform.rotation);

                    ableToShoot++;

                    GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                    GetComponent<AudioSource>().PlayOneShot(bulletSound);
                }
            }
        } else {
            if (Input.GetKey(KeyCode.Mouse0)) {
                if (ableToShoot == 0 && !attacking) {

                    GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, this.transform.rotation);

                    ableToShoot++;

                    GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                    GetComponent<AudioSource>().PlayOneShot(bulletSound);
                }
            }

        }

        // Create a new bullet with the current mouse position


        // Used to limit the amount of bullets *Needs to update when animation implemented*
        if (ableToShoot == 0 || ableToShoot == 10) {
            ableToShoot = 0;
        } else {
            ableToShoot++;

        }

    }

    private void Slash() {
        if (DataBetweenScenes.gamePad) {
            if (Input.GetAxis("secondaryAtk") == 1 && !attacking) {
                attacking = true;
                meleeAttack.enabled = true;

                GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
                GetComponent<AudioSource>().PlayOneShot(slashSound);

            }
        } else {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !attacking) {
                attacking = true;
                meleeAttack.enabled = true;

                GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
                GetComponent<AudioSource>().PlayOneShot(slashSound);

            }
        }


        if (attacking) {

            if (wait > 0) {

                wait--;
            } else {
                attacking = false;
                meleeAttack.enabled = false;
                wait = 20;
            }

        }
    }

    void UpdateHP() {
        SetHealthText();

        // check for death
        if (health <= 0) {
            if (numLives == 3) {
                lives[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI_empty_battery");
                health = 100;
                gameObject.transform.SetPositionAndRotation(gameManager.GetComponent<BuildRoom>().getStartingPos(), Quaternion.identity);
                numLives--;
            } else if (numLives == 2) {
                lives[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI_empty_battery");
                health = 100;
                gameObject.transform.SetPositionAndRotation(gameManager.GetComponent<BuildRoom>().getStartingPos(), Quaternion.identity);
                numLives--;
            } else if (numLives == 1) {
                lives[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI_empty_battery");
                health = 100;
                gameObject.transform.SetPositionAndRotation(gameManager.GetComponent<BuildRoom>().getStartingPos(), Quaternion.identity);
                numLives--;
            } else
                GameOver();
        }
    }

    void SetHealthText() {
        healthText.text = "HP: " + health.ToString();
    }

    // This method is called when the player's HP is reduced to 0
    void GameOver() {
        DataBetweenScenes.points = points;
		freeze = true;
		speed = 0.0f;
		Player.position = new Vector3 (-500, -500, 0);
		anim.SendMessage ("Die");
    }
    void Hit(int dmg) {
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().PlayOneShot(damageSound);
        health -= dmg;
		anim.SendMessage ("Damage");
    }

	void Done() {
		SceneManager.LoadScene (2);
	}
}
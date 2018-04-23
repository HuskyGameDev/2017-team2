using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	protected float speedMax;
	public float moveSpeed;
    public float chaseSpeed;

    public GameObject playerGO;
	public bool canAttack = true;

	protected float xMax;
	protected float yMax;
	protected float xMin;
	protected float yMin;

	protected float x;
	protected float y;
	protected float time;
	protected float angle;

	public int health;
	protected int totalHealth;

	protected Rigidbody2D rb2d;
	protected CircleCollider2D circleCollider;
	public GameObject player;
	public Transform player_pos;
	public GameObject playerCheck;

    protected AudioSource audioSource;
    public GameObject gameManager;

    private int attention = 0;
    public const int DEFAULT_HEALTH = 50; 
  	public GameObject healthBar;

    private bool lostSight = true;

	public Animator animator;

	protected bool freeze = false;

    // Use this for initialization
    protected virtual void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		circleCollider = GetComponent<CircleCollider2D> ();
		audioSource = GetComponent<AudioSource> ();
		animator = GetComponent<Animator> ();

//		Vector3 screenMax = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, Camera.main.nearClipPlane));
//		Vector3 screenMin = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane));


		xMax = transform.position.x + 10;
		xMin = transform.position.x - 10;
		yMax = transform.position.y + 10;
		yMin = transform.position.y - 10;
		speedMax = moveSpeed / 30f;

		x = Random.Range(-speedMax, speedMax);
		y = Random.Range(-speedMax, speedMax);
	}

	// Update is called once per frame
	void Update () {

		if (health > 0 && !freeze) {
			if (attention == 0 && lostSight)
				MoveAtRandom ();
			else {
				if (canAttack) {
					Chase ();
				}
			}
			if (attention > 0)
				attention--;
		}

		freeze = player.GetComponent<PlayerController> ().freeze;

		if (freeze) {
			animator.enabled = false;
		}
	}

	protected virtual void MoveAtRandom() {

		time += Time.deltaTime;

		if (transform.localPosition.x > xMax) {
			x = Random.Range (-speedMax, 0.0f);
			time = 0.0f; 
		}
		if (transform.localPosition.x < xMin) {
			x = Random.Range (0.0f, speedMax);
			time = 0.0f; 
		}
		if (transform.localPosition.y > yMax) {
			y = Random.Range (-speedMax, 0.0f);
			time = 0.0f; 
		}
		if (transform.localPosition.y < yMin) {
			y = Random.Range (0.0f, speedMax);
			time = 0.0f; 
		}

		angle = Mathf.Atan2 (y, x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, 0, angle - 90);

		if (time > 1.0f) {
			x = Random.Range (-speedMax, speedMax);
			y = Random.Range (-speedMax, speedMax);
			time = 0.0f;
		}

		transform.localPosition = new Vector2 (transform.localPosition.x + x, transform.localPosition.y + y);
	}

	void OnTriggerEnter2D (Collider2D other) {

		if (!freeze) {
			if (other.gameObject.CompareTag ("Player") && hasLOS (other)) {

                lostSight = false;
                player = other.gameObject;
				player_pos = player.GetComponent<Transform> ();
				attention = 200;
			}
		}
	}

    void OnTriggerExit2D(Collider2D collision) {
        if (!freeze) {
            if (collision.gameObject.CompareTag("Player")) {
                lostSight = true;
            }
        }
    }
    //for call by other
    public void setHealth(int i) {
        health = i;
        totalHealth = i;
    }

    //Should detect if the enemy can see the player currently or not
    bool hasLOS(Collider2D other) {
		bool canSee = false;

		if (health > 0 && !freeze) {
			canSee = true;
		}

		return canSee;
    }

	protected virtual void Chase() {

		transform.position = Vector2.MoveTowards(transform.position, player_pos.position, moveSpeed * Time.deltaTime);

		angle = Mathf.Atan2 (player_pos.position.y - transform.position.y, player_pos.position.x - transform.position.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, 0, angle);
	}

	void Hit(int dmg)
	{
		health -= dmg;
        attention = 200;
		if (health <= 0) {
			canAttack = false;
			Die ();
		} else {
			healthBar.SetActive (true);
			healthBar.SendMessage ("Damage", (float)health / (float)totalHealth);
		} 
    } 

	//Should be overridden by each enemy that inherits to handle awarding of points
	public virtual void Die() {
        Destroy(gameObject);
	}
		
  }

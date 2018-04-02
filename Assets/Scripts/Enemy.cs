using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	protected float speedMax;
	public float speed = 2f;

	protected float xMax;
	protected float yMax;
	protected float xMin;
	protected float yMin;

	protected float x;
	protected float y;
	protected float time;
	protected float angle;

	protected int health;
	protected int totalHealth;

	protected Rigidbody2D rb2d;
	protected CircleCollider2D circleCollider;
	public GameObject player;
	protected Transform player_pos;

    protected AudioSource audioSource;
    public AudioClip deathSound;

    private int attention = 0;

  	public GameObject healthBar;

    // Use this for initialization
    protected virtual void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		circleCollider = GetComponent<CircleCollider2D> ();
		audioSource = GetComponent<AudioSource> ();

		health = 50;
		totalHealth = health;

//		Vector3 screenMax = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, Camera.main.nearClipPlane));
//		Vector3 screenMin = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane));


		xMax = transform.position.x + 10;
		xMin = transform.position.x - 10;
		yMax = transform.position.y + 10;
		yMin = transform.position.y - 10;
		speedMax = speed / 30f;

		x = Random.Range(-speedMax, speedMax);
		y = Random.Range(-speedMax, speedMax);
	}

	// Update is called once per frame
	void Update () {

        if (attention == 0)
            MoveAtRandom();
        else  {
			Chase ();

		}
        if (attention > 0)
            attention--;
	}

	protected virtual void MoveAtRandom() {
		time += Time.deltaTime;

		if (transform.localPosition.x > xMax) {
			x = Random.Range(-speedMax, 0.0f);
			time = 0.0f; 
		}
		if (transform.localPosition.x < xMin) {
			x = Random.Range(0.0f, speedMax);
			time = 0.0f; 
		}
		if (transform.localPosition.y > yMax) {
			y = Random.Range(-speedMax, 0.0f);
			time = 0.0f; 
		}
		if (transform.localPosition.y < yMin) {
			y = Random.Range(0.0f, speedMax);
			time = 0.0f; 
		}

		angle = Mathf.Atan2 (y, x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, 0, angle-90);

		if (time > 1.0f) {
			x = Random.Range(-speedMax, speedMax);
			y = Random.Range(-speedMax, speedMax);
			time = 0.0f;
		}

		transform.localPosition = new Vector2(transform.localPosition.x + x, transform.localPosition.y + y);
	}

	void OnTriggerEnter2D (Collider2D other) {

		if (other.gameObject.CompareTag ("Player") && hasLOS(other)) {

			player = other.gameObject;
			player_pos = player.GetComponent<Transform> ();
            attention = 200;
		}
	}

    //Should detect if the enemy can see the player currently or not
    bool hasLOS(Collider2D other) {
        bool canSee = true;

        return canSee;
    }

	protected virtual void Chase() {


		transform.position = Vector2.MoveTowards(transform.position, player_pos.position, speed * Time.deltaTime);

		angle = Mathf.Atan2 (player_pos.position.y - transform.position.y, player_pos.position.x - transform.position.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, 0, angle);

	}

	void Hit(int dmg)
	{
		health -= dmg;
        if (health <= 0) 
            Die();
    } else {
    			healthBar.SetActive (true);
 		healthBar.SendMessage ("Damage", (float)health / (float)totalHealth);
    } 
  }
    //Should be overridden by each enemy that inherits to handle awarding of points
    public virtual void Die() {
        Destroy(gameObject);
        //audioSource.PlayOneShot(deathSound);
    }
}

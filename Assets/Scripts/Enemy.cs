using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float speedMax;
	public float speed = 2f;

	public float xMax;
	public float yMax;
	public float xMin;
	public float yMin;

	private float x;
	private float y;
	private float time;

    private int health;

	private Rigidbody2D rb2d;
	private CircleCollider2D circleCollider;
	private GameObject player;
	private Transform player_pos;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		circleCollider = GetComponent<CircleCollider2D> ();

        health = 50;

		x = Random.Range(-speedMax, speedMax);
		y = Random.Range(-speedMax, speedMax);
	}

	// Update is called once per frame
	void Update () {

		if (player == null)
			MoveAtRandom ();
		else
			Chase (player_pos);

        if (health < 0)
        {
           Destroy(gameObject);
        }
	}

	void MoveAtRandom() {
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


		if (time > 1.0f) {
			x = Random.Range(-speedMax, speedMax);
			y = Random.Range(-speedMax, speedMax);
			time = 0.0f;
		}

		transform.localPosition = new Vector2(transform.localPosition.x + x, transform.localPosition.y + y);
	}

	void OnTriggerEnter2D (Collider2D other) {


		if (other.gameObject.CompareTag ("Player")) {
			player = other.gameObject;
			player_pos = player.GetComponent<Transform> ();
		}
	}

	void Chase(Transform obj_pos) {


		transform.position = Vector2.MoveTowards(transform.position, player_pos.position, speed * Time.deltaTime);


	}

    void Hit(int dmg)
    {
        health -= dmg;
    }
}

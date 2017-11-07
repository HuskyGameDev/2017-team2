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

	protected Rigidbody2D rb2d;
	protected CircleCollider2D circleCollider;
	protected GameObject player;
	protected Transform player_pos;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		circleCollider = GetComponent<CircleCollider2D> ();

		Vector3 screenMax = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, Camera.main.nearClipPlane));
		Vector3 screenMin = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane));


		xMax = screenMax.x;
		xMin = screenMin.x;
		yMax = screenMax.y;
		yMin = screenMin.y;
		speedMax = speed / 30f;

		x = Random.Range(-speedMax, speedMax);
		y = Random.Range(-speedMax, speedMax);
	}

	// Update is called once per frame
	void Update () {

		if (player == null)
			MoveAtRandom ();
		else
			Chase (player_pos);
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
		transform.rotation = Quaternion.Euler (0, 0, angle);

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

	protected virtual void Chase(Transform obj_pos) {


		transform.position = Vector2.MoveTowards(transform.position, player_pos.position, speed * Time.deltaTime);

		angle = Mathf.Atan2 (player_pos.position.y - transform.position.y, player_pos.position.x - transform.position.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, 0, angle);

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float speedMax;

	public float xMax;
	public float yMax;
	public float xMin;
	public float yMin;

	private float x;
	private float y;
	private float time;

	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();

		x = Random.Range(-speedMax, speedMax);
		y = Random.Range(-speedMax, speedMax);
	}

	// Update is called once per frame
	void Update () {

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
}

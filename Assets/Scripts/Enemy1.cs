using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy {

	protected override void MoveAtRandom() {
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

		Vector2 movement = new Vector2 (transform.localPosition.x + x, transform.localPosition.y + y);

		if (time - Mathf.Floor(time) <= 0.25 || (time - Mathf.Floor(time) > 0.5 && time - Mathf.Floor(time) < 0.75)) {
			transform.localPosition = movement;
		}

	}

	protected override void Chase(Transform obj_pos) {

		time += Time.deltaTime;

		if (time - Mathf.Floor(time) <= 0.25 || (time - Mathf.Floor(time) > 0.5 && time - Mathf.Floor(time) < 0.75)) {
			transform.position = Vector2.MoveTowards(transform.position, player_pos.position, speed * Time.deltaTime);
		}

		angle = Mathf.Atan2 (player_pos.position.y - transform.position.y, player_pos.position.x - transform.position.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, 0, angle);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {

	Quaternion rotation;
	Transform fill;
	float fillSize;
	float fillPos;
	SpriteRenderer fillRend;
	float x;
	float y;
	float h;


	void Awake(){
		rotation = transform.rotation;
		fill = transform.GetChild (0);
		fillSize = fill.localScale.x;
		fillPos = fill.localPosition.x;
		fillRend = fill.GetComponent<SpriteRenderer> ();
		x = transform.localPosition.x;
		y = transform.localPosition.y;
		h = Mathf.Sqrt (Mathf.Pow (x, 2) + Mathf.Pow (y, 2));
	}

	public void Damage(float health) {
		fill.localScale = new Vector3 (health, fill.localScale.y, fill.localScale.z);
		fill.localPosition = new Vector3 (-((1 - health) / 2), fill.localPosition.y, fill.localPosition.z);

		if (health <= 0.5f && health > 0.15f) {
			fillRend.color = Color.yellow;
		} else if (health > 0.5f) {
			fillRend.color = Color.green;
		} else {
			fillRend.color = Color.red;
		}
	}
	
	void Update () {

		transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.transform.parent.rotation.z * -1.0f);
	
	}

	public void Position(float angle) {
		float newX;
		float newY;

		print (x + ", " + y + ", " + h + ", " + angle);
	}
}

//position = (0, 1)
//Parent rotates 90 degrees
//position = (-1, 0)
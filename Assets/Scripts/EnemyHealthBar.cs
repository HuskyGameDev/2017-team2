using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {

	Quaternion rotation;
	Transform fill;
	float fillSize;
	float fillPos;
	SpriteRenderer fillRend;

	void Awake(){
		rotation = transform.rotation;
		fill = transform.GetChild (0);
		fillSize = fill.localScale.x;
		fillPos = fill.localPosition.x;
		fillRend = fill.GetComponent<SpriteRenderer> ();
	}

	public void Damage(float health) {
		print (health);
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
}

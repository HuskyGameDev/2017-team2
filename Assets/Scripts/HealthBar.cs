using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public float health; //current progress
	public Slider healthSlider;
	public PlayerController player;
	public Image fill;

	void Start() {
		health = 100;
	}

	void Update() {
		healthSlider.value = player.health;

		if (healthSlider.value <= 50 && healthSlider.value > 15) {
			fill.color = Color.yellow;
		} else if (healthSlider.value > 50) {
			fill.color = Color.green;
		} else {
			fill.color = Color.red;
		}



	}

}

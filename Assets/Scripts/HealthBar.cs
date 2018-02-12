using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public float health; //current progress
	public Slider healthSlider;
	public PlayerController player;

	void Start() {
		health = 100;
	}

	void Update() {
		healthSlider.value = player.health;
	}

}

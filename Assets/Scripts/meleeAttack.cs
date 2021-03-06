﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttack : MonoBehaviour {

	private int isCol = 0;
    public int dmg = 15;
	public GameObject slasher;
	private AudioSource audioSource;
	public AudioClip hitSound;
	public AudioClip missSound;

	void Start() {
		audioSource = slasher.GetComponent<AudioSource>();
	}

	void Update() {
        //currently triggers on player attempts to swing sword while swinging sword. Commented out until fixed
        /*
		if (slasher.CompareTag("Player") && Input.GetKeyDown(KeyCode.Mouse1) && isCol == 0)
		{
			audioSource.PlayOneShot(missSound);
		}
        */
	}
    private void OnTriggerEnter2D(Collider2D col)
    {

		if (slasher.CompareTag("Player") && col.isTrigger != true && col.gameObject.CompareTag("Enemy"))
        {
			isCol++;
            col.SendMessageUpwards("Hit", dmg);

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot (hitSound);
			isCol--;
        }

		if (slasher.CompareTag("Enemy") && col.isTrigger != true && col.gameObject.CompareTag("Player"))
		{
			col.SendMessageUpwards("Hit", dmg);

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot (hitSound);
		}
    }
}

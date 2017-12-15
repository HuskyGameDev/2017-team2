﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletAttack : MonoBehaviour {

    public int dmg;
    public GameObject shooter;
    private AudioSource audioSource;
    public AudioClip hitSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D col) {


        if (shooter.tag == col.gameObject.tag)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (shooter != null) {
            if (shooter.CompareTag("Player") && col.isTrigger != true && col.gameObject.CompareTag("Enemy")) {
                col.SendMessageUpwards("Hit", dmg);
                DestroyObject(transform.gameObject);
            }

            if (shooter.CompareTag("Enemy") && col.isTrigger != true && col.gameObject.CompareTag("Player")) {
                col.SendMessageUpwards("Hit", dmg);
               // DestroyObject(transform.gameObject);
            }
        }
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {

    public int damage;

    // Use this for initialization
    private void OnCollisionEnter2D(Collision2D collision) {
        if (this.gameObject.tag == "Enemy") {
            transform.Rotate(0, 0, 180);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        //Handle the Lock
        if (this.gameObject.tag == "Lock" && !collider.isTrigger && collider.gameObject.tag == "Player") {
            print("Yup");
            if (collider.gameObject.GetComponent<PlayerController>().hasKey) {
                Destroy(this.gameObject);
                collider.gameObject.GetComponent<PlayerController>().hasKey = false;
            }
            
        }
        //Handle the Key
        if (this.gameObject.tag == "Key" && collider.gameObject.tag == "Player") {
            collider.gameObject.GetComponent<PlayerController>().hasKey = true;
            Destroy(this.gameObject);
        }

        if (this.gameObject.tag == "Bullet" && !collider.isTrigger) {
            if (collider.gameObject.tag == "Enemy") {
               collider.SendMessageUpwards("Hit", damage);
            }
            if (collider.gameObject.tag != "Player") {
                Destroy(this.gameObject);
            }
        }
        if (this.gameObject.tag == "EnemyBullet" && !collider.isTrigger) {
            if (collider.gameObject.tag == "Player") {
                collider.SendMessageUpwards("Hit", damage);
            }
            if (collider.gameObject.tag != "Enemy") {
                Destroy(this.gameObject);
            }
        }
    }
}

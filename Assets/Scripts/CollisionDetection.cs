using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {

    public int damage;

    // Use this for initialization
    private void OnCollisionEnter2D(Collision2D collision) {
        if (this.gameObject.tag == "Enemy") {
            if (Random.Range(0f, 1f) < 0.5) {
                transform.Rotate(0, 0, 70);
            } else {
                transform.Rotate(0, 0, -70);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
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

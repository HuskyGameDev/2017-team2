using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy {

    /* Structure for storing a bullet with the mouse position at the time the bullet is created */
    struct bulletStruct {
        private GameObject bullet;
        private Vector3 pos;
        private Collider2D bulletAtk;


        public void setObj(GameObject newBullet) {
            bullet = newBullet;
        }

        public void setPos(Vector3 newPos) {
            pos = newPos;
        }

        public GameObject getObj() {
            return bullet;
        }

        public Vector3 getPos() {
            return pos;
        }

        public void setColliderVar(Collider2D col) {
            bulletAtk = col;
        }
        public void setCollider(bool set) {
            if (set) {
                bulletAtk.enabled = true;
            } else {
                bulletAtk.enabled = false;
            }
        }
    }

    // Projectiles
    public GameObject bulletPrefab;
    public Transform EnemyTransform;
    public Transform bulletSpawn;

    private List<bulletStruct> bullets = new List<bulletStruct>();
    private float bulletSpeed;
    private int ableToShoot = 0;

    // Use this for initialization
    protected override void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        health = 50;

        EnemyTransform = GetComponent<Transform>();

        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));


        xMax = transform.position.x + 10;
        xMin = transform.position.x - 10;
        yMax = transform.position.y + 10;
        yMin = transform.position.y - 10;
        speedMax = speed / 30f;

        x = Random.Range(-speedMax, speedMax);
        y = Random.Range(-speedMax, speedMax);

        // Set speed of bullet
        bulletSpeed = 20;
    }

    protected override void Chase() {

        transform.position = Vector2.MoveTowards(transform.position, player_pos.position, speed * Time.deltaTime);

        angle = Mathf.Atan2(player_pos.position.y - transform.position.y, player_pos.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle+90);

        shoot();
    }


    // Method used to handle shooting projectiles
    private void shoot() {
        // Create a new bullet with the current mouse position
        if (ableToShoot == 0) {
            //bulletStruct newBullet = new bulletStruct();
            GameObject ebullet = Instantiate(bulletPrefab, bulletSpawn.position, this.transform.rotation);

            ableToShoot++;
        }

        // Used to limit the amount of bullets *Needs to update when animation implemented*
        if (ableToShoot == 0 || ableToShoot == 10) {
            ableToShoot = 0;
        } else {
            ableToShoot++;
        }

    }
}

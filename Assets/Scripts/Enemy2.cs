using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Medium Enemies - Al Roboto
public class Enemy2 : Enemy {

    // Projectiles
    public GameObject bulletPrefab;
    public Transform EnemyTransform;
    public Transform bulletSpawn;

    public AudioClip enemyBulletSound;
    public AudioClip deathSound;

    private List<bulletStruct> bullets = new List<bulletStruct>();
    private float bulletSpeed;
    private int ableToShoot = 0;

    private AudioSource newAudioSource;
    
    private Vector2 movement;
    public new const int DEFAULT_HEALTH = 160;

    private float rot = 0;

    // Use this for initialization
    protected override void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        newAudioSource = player.GetComponent<AudioSource>();
		animator = GetComponent<Animator> ();

        EnemyTransform = GetComponent<Transform>();

//        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
//        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));

        xMax = transform.position.x + 10;
        xMin = transform.position.x - 10;
        yMax = transform.position.y + 10;
        yMin = transform.position.y - 10;
        speedMax = moveSpeed / 30f;

        x = Random.Range(-speedMax, speedMax);
        y = Random.Range(-speedMax, speedMax);

        // Set speed of bullet
        bulletSpeed = 20;
    }

    protected override void MoveAtRandom() {

        rot += Random.Range(-1f, 1f);

        if (rot > 3) {
            rot = 3;
        } else if (rot < -3) {
            rot = -3;
        }

        transform.Rotate(new Vector3(0, 0, rot));

        movement = -transform.up * moveSpeed;
        GetComponent<Rigidbody2D>().AddForce(movement);

    }

    protected override void Chase() {
        angle = Mathf.Atan2(player_pos.position.y - transform.position.y, player_pos.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        Vector2 force = (player_pos.position - transform.position).normalized * chaseSpeed;

        GetComponent<Rigidbody2D>().AddForce(force);

		shoot ();
    }


    // Method used to handle shooting projectiles
    private void shoot() {
		animator.SetTrigger ("RobotoShoot");

		// Create a new bullet with the current mouse position
		if (ableToShoot == 0) {

			GameObject ebullet = Instantiate (bulletPrefab, bulletSpawn.position, this.transform.rotation);
			
			newAudioSource.pitch = Random.Range(0.5f, 0.7f);
            newAudioSource.PlayOneShot(enemyBulletSound);

			ableToShoot++;
		}

		// Used to limit the amount of bullets *Needs to update when animation implemented*
		if (ableToShoot == 0 || ableToShoot == 100) {
			ableToShoot = 0;
		} else {
			ableToShoot++;
		}
    }

    public override void Die() {
        GameObject.Find("GameManager").GetComponent<AudioSource>().PlayOneShot(deathSound);
//        base.Die();
		animator.SetTrigger ("RobotoDeath");
		moveSpeed = 0.0f;
        chaseSpeed = 0.0f;
        speedMax = 0.0f;
		healthBar.SetActive (false);
		Destroy (rb2d);
		Destroy (circleCollider);
		Destroy (gameObject.GetComponent<PolygonCollider2D> ());
		gameObject.tag = "Untagged";
		player.GetComponent<PlayerController>().points += 5;
    }
}

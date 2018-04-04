using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Small Enemies - Ankle Biter
public class Enemy3 : Enemy {

	// Object for slashing
	private int wait = 10;
	private bool attacking;
	public Collider2D meleeAttack;

    private Vector2 movement2;
    private float rot = 0;

	protected override void Start() {
		
		rb2d = GetComponent<Rigidbody2D> ();
		circleCollider = GetComponent<CircleCollider2D> ();
        audioSource = GetComponent<AudioSource>();

        health = 80;

//		Vector3 screenMax = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, Camera.main.nearClipPlane));
//		Vector3 screenMin = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane));

        xMax = transform.position.x + 10;
        xMin = transform.position.x - 10;
        yMax = transform.position.y + 10;
        yMin = transform.position.y - 10;
        speedMax = moveSpeed / 30f;

		x = Random.Range(-speedMax, speedMax);
		y = Random.Range(-speedMax, speedMax);

		// Set melee attack stuff
		meleeAttack.enabled = false;
		attacking = false;

	}

    protected override void MoveAtRandom() {

        transform.Rotate(new Vector3(0, 0, rot - 45));
        movement2 = transform.up * moveSpeed;

        rot += Random.Range(-1f, 1f);
        if (rot > 3) {
            rot = 3;
        } else if (rot < -3) {
            rot = -3;
        }

        GetComponent<Rigidbody2D>().AddForce(movement2);
        transform.Rotate(new Vector3(0, 0, rot + 45));
    }

    protected override void Chase() {


		angle = Mathf.Atan2 (player_pos.position.y - transform.position.y, player_pos.position.x - transform.position.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, 0, angle - 45);
        
        Vector2 force = (player_pos.position - transform.position).normalized * chaseSpeed;

        GetComponent<Rigidbody2D>().AddForce(force);

        slash ();
	}

	private void slash()
	{
		float dist = Vector3.Distance (player_pos.position, transform.position);

		if (dist < 1 && !attacking)
		{
			attacking = true;
			meleeAttack.enabled = true;
		}

		if (attacking)
		{

			if (wait > 0)
			{

				wait--;
			}

			else
			{
				attacking = false;
				meleeAttack.enabled = false;
				wait = 10;
			}
		}
	}

    public override void Die() {
        base.Die();
        player.GetComponent<PlayerController>().points += 2;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Big Enemies - Big Guns
public class Enemy1 : Enemy {

	// Object for slashing
	private int wait = 10;
	private bool attacking;
	public Collider2D meleeAttack;

    private AudioSource newAudioSource;
    public AudioClip deathSound;
    private bool animationDone = true;
    private Vector2 movement2;
    private float rot = 0;

    public new const int DEFAULT_HEALTH = 480;
	protected override void Start() {
		rb2d = GetComponent<Rigidbody2D> ();
		circleCollider = GetComponent<CircleCollider2D> ();
        newAudioSource = player.GetComponent<AudioSource>();

		animator = GetComponent<Animator> ();

		health = 480;

		totalHealth = health;

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

        movement2 = Vector2.up*100;
    }

	protected override void MoveAtRandom() {


        rot += Random.Range(-1f, 1f);
        if (rot > 3) {
            rot = 3;
        } else if (rot < -3) {
            rot = -3;
        }
        transform.Rotate(new Vector3(0, 0, rot));
        movement2 = -transform.up * moveSpeed;
        GetComponent<Rigidbody2D>().AddForce(movement2);

        Vector2 movement = new Vector2(transform.localPosition.x + x, transform.localPosition.y + y);

    }

	protected override void Chase() {

        angle = Mathf.Atan2(player_pos.position.y - transform.position.y, player_pos.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        Vector2 force = (player_pos.position - transform.position).normalized * chaseSpeed;

        GetComponent<Rigidbody2D>().AddForce(force);

        if ((player_pos.position - transform.position).magnitude < 2)
            slash();
	}
    //private IEnumerator attack() {
     //   yield return new WaitForSeconds(.87f);
      //  meleeAttack.enabled = true;
       // meleeAttack.GetComponent<CircleCollider2D>().enabled = true;
       // yield return new WaitForEndOfFrame();
     // / / meleeAttack.enabled = false;
      //  meleeAttack.GetComponent<CircleCollider2D>().enabled = false;
       // yield return new WaitForSeconds(.83f);
   // }
	private void slash() {
        animator.SetTrigger("BigGunsSmash");
        //float dist = Vector3.Distance (player_pos.position, transform.position);
        //StartCoroutine(attack());

        //print(dist);
       // attacking = true;
		//if (dist < 2 && !attacking)
		//{
		//	attacking = true;
		//	meleeAttack.enabled = true;
		//}

		//if (attacking)
		//{

		//	if (wait > 0)
		//	{
		//		wait--;
	//		}

		//	else
		//	{
		//		attacking = false;
		//		meleeAttack.enabled = false;
		//		wait = 100;
		//	}
		//}
	}
    public void dealDamage() {
        if ((player_pos.position - transform.position).magnitude < 1.4) {
            player.SendMessageUpwards("Hit", meleeAttack.GetComponent<meleeAttack>().dmg);
        }
    }
    public override void Die() {
        GameObject.Find("GameManager").GetComponent<AudioSource>().PlayOneShot(deathSound);
//		print ("So many regrets...");
		animator.SetTrigger ("BigGunsDeath");
		moveSpeed = 0.0f;
        chaseSpeed = 0.0f;
		speedMax = 0.0f;
		healthBar.SetActive (false);
		Destroy (rb2d);
		Destroy (circleCollider);
		Destroy (gameObject.GetComponent<PolygonCollider2D> ());
		gameObject.tag = "Untagged";
        meleeAttack.enabled = false;
        player.GetComponent<PlayerController>().points += 10;
    }
}

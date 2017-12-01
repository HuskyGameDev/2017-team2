using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy {

	/* Structure for storing a bullet with the mouse position at the time the bullet is created */
	struct bulletStruct
	{
		private GameObject bullet;
		private Vector3 pos;
		private Collider2D bulletAtk;


		public void setObj(GameObject newBullet)
		{
			bullet = newBullet;
		}

		public void setPos(Vector3 newPos)
		{
			pos = newPos;
		}

		public GameObject getObj()
		{
			return bullet;
		}

		public Vector3 getPos()
		{
			return pos;
		}

		public void setColliderVar(Collider2D col)
		{
			bulletAtk = col;
		}
		public void setCollider(bool set)
		{
			if(set)
			{
				bulletAtk.enabled = true;
			}

			else
			{
				bulletAtk.enabled = false;
			}
		}
	}

	// Projectiles
	public GameObject bulletPrefab;
	private List<bulletStruct> bullets = new List<bulletStruct>();
	private float bulletSpeed;
	private int ableToShoot = 0;
	public Collider2D bulletAttack;

	// Use this for initialization
	protected override void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		circleCollider = GetComponent<CircleCollider2D> ();

		health = 50;

		Vector3 screenMax = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, Camera.main.nearClipPlane));
		Vector3 screenMin = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane));


		xMax = screenMax.x;
		xMin = screenMin.x;
		yMax = screenMax.y;
		yMin = screenMin.y;
		speedMax = speed / 30f;

		x = Random.Range(-speedMax, speedMax);
		y = Random.Range(-speedMax, speedMax);

		// Set speed of bullet
		bulletSpeed = 20;
		bulletAttack.enabled = false;
	}

	protected override void Chase() {

		transform.position = Vector2.MoveTowards(transform.position, player_pos.position, speed * Time.deltaTime);

		angle = Mathf.Atan2 (player_pos.position.y - transform.position.y, player_pos.position.x - transform.position.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, 0, angle);

		shoot ();
	}
		
	// Method used to handle shooting projectiles
	private void shoot()
	{
		// Create a new bullet with the current mouse position
		if (ableToShoot == 0)
			{
				bulletStruct newBullet = new bulletStruct();
				GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
				Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
				Vector3 pos = (Camera.main.WorldToScreenPoint(player_pos.position) - sp).normalized;
				newBullet.setPos(pos);
				newBullet.setObj(bullet);
				newBullet.setColliderVar(bulletAttack);
				newBullet.setCollider(true);
				bullets.Add(newBullet);
				ableToShoot++;
			}

		// Used to limit the amount of bullets *Needs to update when animation implemented*
		if (ableToShoot == 0 || ableToShoot == 10)
		{
			ableToShoot = 0;
		}

		else
		{
			ableToShoot++;
		}

		// For every bullet on screen move towards the mouse position it was shot at
		for (int i = 0; i < bullets.Count; i++)
		{
			GameObject movingBullet = bullets[i].getObj();

			if (movingBullet != null)
			{

				movingBullet.transform.Translate(bullets[i].getPos() * Time.deltaTime * bulletSpeed);
				bulletAttack.enabled = true;
			}

			Vector3 bulletPos = Camera.main.WorldToScreenPoint(movingBullet.transform.position);

			// Remove bullet if off screen
			if (bulletPos.y >= Screen.height || bulletPos.y <= 0 || bulletPos.x >= Screen.width || bulletPos.x <= 0)
			{
				DestroyObject(movingBullet);
				bullets.Remove(bullets[i]);
			}
		}
	}
}

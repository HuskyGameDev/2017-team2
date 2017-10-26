using UnityEngine;
using System.Collections;

/*
 * Christina Anderson
 * Main controller for player behavior. Currently, it allows the player to move the sprite around and follows mouse direction
 */

public class PlayerController : MonoBehaviour {

	//Stores a reference to the Rigidbody2D component required to use 2D Physics.
	private Rigidbody2D rb2d;

	//Stores the position of the mouse
	private Vector3 mouse_pos;

	//Transform object for player
	private Transform Player;

	//Stores the position of the object
	private Vector3 object_pos;

	//Stores angle needed to turn
	private float angle;

	//stores amount of life character has
	private float life;

	public float speed;

	// Use this for initialization
	void Start()
	{
		life = 100;

		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();

		Player = GetComponent<Transform> ();
	}

	//Called every frame
	void Update()
	{
		//Enter mouse mosition
		mouse_pos = Input.mousePosition;
		mouse_pos.z = -10;

		//Enter player position
		object_pos = Camera.main.WorldToScreenPoint (Player.position);

		//Find different coordinates between mouse and player position
		mouse_pos.x = mouse_pos.x - object_pos.x;
		mouse_pos.y = mouse_pos.y - object_pos.y;

		//Calculate angle between mouse and player position
		angle = Mathf.Atan2 (mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;

		//Rotate player
		transform.rotation = Quaternion.Euler (0, 0, angle);	

		//Stores horizontal and vertical coordinates
		float moveHorizontal = 0;
		float moveVertical = 0;

		moveHorizontal = Input.GetAxis ("Horizontal");
		moveVertical = Input.GetAxis ("Vertical");

		//Use the two store floats to create a new Vector2 variable movement.
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

		//Change position of player
		rb2d.MovePosition (rb2d.position + speed * movement * Time.fixedDeltaTime);
	}
}
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.

	private Vector3 mouse_pos;
	public Transform Player;
	private Vector3 object_pos;
	private float angle;

	// Use this for initialization
	void Start()
	{
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		mouse_pos = Input.mousePosition;
		mouse_pos.z = -10;
		object_pos = Camera.main.WorldToScreenPoint (Player.position);
		mouse_pos.x = mouse_pos.x - object_pos.x;
		mouse_pos.y = mouse_pos.y - object_pos.y;
		angle = Mathf.Atan2 (mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, 0, angle);	
	}

	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{

		float moveHorizontal = 0;
		float moveVertical = 0;

		if (Input.GetKey (KeyCode.W)) {
			moveHorizontal = 0;
			moveVertical = 1;
		} else if (Input.GetKey (KeyCode.A)) {
			moveHorizontal = -1;
			moveVertical = 0;
		} else if (Input.GetKey (KeyCode.S)) {
			moveHorizontal = 0;
			moveVertical = -1;
		} else if (Input.GetKey (KeyCode.D)) {
			moveHorizontal = 1;
			moveVertical = 0;
		}

		//Use the two store floats to create a new Vector2 variable movement.
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

		rb2d.MovePosition (rb2d.position + 2 * movement * Time.fixedDeltaTime);
	
	}
}
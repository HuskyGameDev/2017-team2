using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletAttack : MonoBehaviour {

    public int dmg = 10;
	public GameObject shooter;

	private void OnTriggerEnter2D(Collider2D col)
    {
		if (shooter.CompareTag("Player") && col.isTrigger != true && col.gameObject.CompareTag("Enemy"))
        {
			print("Pew");
			col.SendMessageUpwards("Hit", dmg);
			DestroyObject (transform.parent.gameObject);
        }

		if (shooter.CompareTag("Enemy") && col.isTrigger != true && col.gameObject.CompareTag("Player"))
		{
			print("Ouch!");
			col.SendMessageUpwards("Hit", dmg);
			DestroyObject (transform.parent.gameObject);
		}
			
    }



}

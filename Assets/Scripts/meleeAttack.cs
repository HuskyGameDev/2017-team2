using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttack : MonoBehaviour {

    public int dmg = 15;
	public GameObject slasher;
	private AudioSource audioSource;
	public AudioClip hitSound;
	public AudioClip missSound;

	void Start() {
		audioSource = GetComponent<AudioSource> ();
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
		if (slasher.CompareTag("Player") && col.isTrigger != true && col.gameObject.CompareTag("Enemy"))
        {
            col.SendMessageUpwards("Hit", dmg);
			audioSource.PlayOneShot (hitSound);
        }

		if (slasher.CompareTag("Enemy") && col.isTrigger != true && col.gameObject.CompareTag("Player"))
		{
			print("Owwie!");
			col.SendMessageUpwards("Hit", dmg);
			audioSource.PlayOneShot (hitSound);
		}
    }
}

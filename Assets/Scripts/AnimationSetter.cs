using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*
 * Codey Walker
 * Script to handle animations and U's animator
 */

public class AnimationSetter : MonoBehaviour
{

    /* Animator needed to change animations for U */
    public Animator anim;
    private int cd = 0;
    private int idleTime = 0;
    private bool idle = true;
	public Animation deathAnim;

    void Start()
    {
		deathAnim = GetComponent<Animation> ();
    }

    // Update is called once per frame
    void Update () {

        /* Shooting */
        if (Input.GetKey(KeyCode.Mouse0) || (Input.GetAxis("primaryAtk") > 0) && cd == 0)
        {
            
            anim.SetTrigger("UShoot");
            idleTime = 0;
        }

        else
        {

            /* When not shooting, transition back to either idle or walk animation */
            if (idle)
            {
                anim.SetTrigger("UBackIdle");
            }

            else
            {
                anim.SetTrigger("UBackWalk");
            }
        }

        /* Melee Attack */
        if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetAxis("secondaryAtk") == 1) && cd == 0)
        {
            cd = 20;
            anim.SetTrigger("UMelee");
            idleTime = 0;
        }

        if (cd > 0)
        {
            cd--;
        }

        if (idleTime >= 180)
        {
            anim.SetTrigger("UBlink");
            idleTime = 0;
        }

        /* Walking vs Idle */
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
                 Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                 Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0
                 ) {

            idle = false;
            anim.SetTrigger("UWalk");
            idleTime = 0;
        }

        else {
            anim.SetTrigger("UIdle");
            idle = true;
        }

        /* Increment time to blink after 3 seconds of no activity */
        idleTime++;
	}

	public void Damage() {
		anim.SetTrigger ("UDamage");
	}

	public void Die() {
		anim.SetTrigger ("UDeath");
	}
}

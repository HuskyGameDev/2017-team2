using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletAttack : MonoBehaviour {

    public int dmg = 10;
    bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
 
        if (col.isTrigger != true && col.gameObject.CompareTag("Enemy"))
        {
            print("Pew");
            // Damage enemy
            col.SendMessageUpwards("Hit", dmg);
            isTriggered = true;

           
        }

        /*if (col.isTrigger == true && col.gameObject.CompareTag("Bullet"))
        {
            // Destroy bullet
            Destroy(gameObject);
        }

        if (isTriggered && (col.gameObject.CompareTag("Player") == false))
        {
            Destroy(gameObject);
        }*/
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttack : MonoBehaviour {

    public int dmg = 15;

    private void OnTriggerEnter2D(Collider2D col)
    {

       
        if (col.isTrigger != true && col.gameObject.CompareTag("Enemy"))
        {
            print("kapow");
            col.SendMessageUpwards("Hit", dmg);
        }
    }
}

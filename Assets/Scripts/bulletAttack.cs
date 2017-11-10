using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletAttack : MonoBehaviour {

    public int dmg = 10;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.isTrigger != true && col.gameObject.CompareTag("Enemy"))
        {
            print("Pew");
            col.SendMessageUpwards("Hit", dmg);
        }
    }
}

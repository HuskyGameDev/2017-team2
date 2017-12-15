using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingStation : MonoBehaviour {
    public GameObject player;

	void Update () {
        if (player.transform.position.x > transform.position.x - 2 && player.transform.position.y > transform.position.y - 2 &&
            player.transform.position.x < transform.position.x + 2 && player.transform.position.y < transform.position.y + 2)
            if (player.GetComponent<PlayerController>().health <= 100) {
                player.GetComponent<PlayerController>().health += 10;
                if (player.GetComponent<PlayerController>().health > 100)
                    player.GetComponent<PlayerController>().health = 100;
            }
	}
}

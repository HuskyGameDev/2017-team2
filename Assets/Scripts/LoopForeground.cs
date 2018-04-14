using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopForeground : MonoBehaviour {

    private Rigidbody2D rb2d;
    private float buildingHeight = 3256;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.velocity = new Vector2(0, -100);
    }

    // Update is called once per frame
    void Update () {
        
        if (transform.position.y < -buildingHeight)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 offset = new Vector2(0, buildingHeight * 2f);

        transform.position = (Vector2)transform.position + offset;
    }
}

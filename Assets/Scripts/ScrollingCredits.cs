using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingCredits : MonoBehaviour {

    public Text thanks;
    public Text msg1;
    public Text msg2;
    public Text msg3;
    public Text msg4;

    public Button menuButton;

    float start;

    // Use this for initialization
    void Start () {
        start = Time.time;
        menuButton.gameObject.SetActive(false);
	}
    
	// Update is called once per frame
	void Update () {

        if (Time.time - start >= 3)
        {
            thanks.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 100);
            msg1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 100);
        }
        
        if (Time.time - start >= 6)
        {
            msg2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 100);
        }
        
        if (Time.time - start >= 9)
        {
            msg3.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 100);
        }
        
        if (Time.time - start >= 12.5)
        {
            msg4.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 100);
        }

        if (Time.time - start >= 25)
        {
            menuButton.gameObject.SetActive(true);
        }
    }
}

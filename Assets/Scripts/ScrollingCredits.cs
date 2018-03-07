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
            thanks.transform.Translate(0, 50 * Time.deltaTime, 0);
            msg1.transform.Translate(0, 50 * Time.deltaTime, 0);
        }

        if (Time.time - start >= 6)
        {
            msg2.transform.Translate(0, 50 * Time.deltaTime, 0);
        }

        if (Time.time - start >= 9)
        {
            msg3.transform.Translate(0, 50 * Time.deltaTime, 0);
        }

        if (Time.time - start >= 13)
        {
            msg4.transform.Translate(0, 50 * Time.deltaTime, 0);
        }

        if (thanks.transform.position.y > 800)
        {
            thanks.transform.position = new Vector3(0, 850, 0);
            thanks.gameObject.SetActive(false);
        }

        if (msg1.transform.position.y > 800)
        {
            msg1.transform.position = new Vector3(0, 850, 0);
            msg1.gameObject.SetActive(false);
        }

        if (msg2.transform.position.y > 800)
        {
            msg2.transform.position = new Vector3(0, 850, 0);
            msg2.gameObject.SetActive(false);
        }

        if (msg3.transform.position.y > 800)
        {
            msg3.transform.position = new Vector3(0, 850, 0);
            msg3.gameObject.SetActive(false);
        }

        if (msg4.transform.position.y > 850)
        {
            msg4.transform.position = new Vector3(0, 850, 0);
            msg4.gameObject.SetActive(false);
        }

        if (Time.time >= 30)
        {
            menuButton.gameObject.SetActive(true);
        }
    }
}

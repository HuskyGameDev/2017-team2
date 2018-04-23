using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (DataBetweenScenes.gamePad == true)
        {
            Cursor.visible = false;
        } else
        {
            Cursor.visible = true;
        }

        
	}
}

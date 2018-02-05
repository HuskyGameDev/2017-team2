using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class focusOnLoad : MonoBehaviour {

    public GameObject defaultButton;
    
    // string array used to see if there is currently a controller plugged in
    private string[] controllers;

    // boolean to determine if there is currently a controller
    private bool gamePad;

    // Use this for initialization
    void Start () {

        controllers = Input.GetJoystickNames();

        if (controllers.Length > 0)
        {
            if (!string.IsNullOrEmpty(controllers[0]))
            {
                print("Controller connected");
                gamePad = true;
                Cursor.visible = false;
            }
            else
            {
                print("No controller");
                gamePad = false;
            }
        }

        if (defaultButton != null && gamePad == true)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }
	}
}

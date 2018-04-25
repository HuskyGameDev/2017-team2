using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckController : MonoBehaviour {

    // string array used to see if there is currently a controller plugged in
    private string[] controllers;

    // float to determine when to check if there is a controller connected or not
    private int checkControl = 180;

    void Start() {
        controllers = Input.GetJoystickNames();
    }

    // Update is called once per frame
    void Update () {
        // Check for controller in update by counting the number of frames
        checkControl++;

        if (checkControl >= 180)
        {
            // update the Joystick Names array
            controllers = Input.GetJoystickNames();
            print(controllers.Length);
            if (controllers.Length > 0)
            {
                print(controllers[0]);
                if (!string.IsNullOrEmpty(controllers[0]))
                {
                    print("controller");
                    DataBetweenScenes.gamePad = true;
                    Cursor.visible = false;
                }
                else
                {
                    print("no controller");
                    DataBetweenScenes.gamePad = false;
                    Cursor.visible = true;
                }
            }

            checkControl = 0;
        }

    }
}

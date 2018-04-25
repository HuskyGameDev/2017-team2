using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    // string array used to see if there is currently a controller plugged in
    private string[] controllers;

    // float to determine when to check if there is a controller connected or not
    private int checkControl = 180;

    // stores direction of the right stick for aiming purposes
    private Vector2 rStick;

    //Stores angle needed to turn
    private float angle;

    // Use this for initialization
    void Start()
    {
        controllers = Input.GetJoystickNames();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for controller in update by counting the number of frames
        checkControl++;

        if (checkControl >= 180)
        {
            // update the Joystick Names array
            controllers = Input.GetJoystickNames();
            if (controllers.Length > 0)
            {
                if (!string.IsNullOrEmpty(controllers[0]))
                {
                    DataBetweenScenes.gamePad = true;

                }
                else
                {
                    DataBetweenScenes.gamePad = false;
                }
            }

            checkControl = 0;
        }


        if (DataBetweenScenes.gamePad == true)
        {
            Cursor.visible = false;

            rStick.x = Input.GetAxis("rStickX");
            rStick.y = Input.GetAxis("rStickY");

            if (rStick.magnitude > 0.1f)
            {
                // get new angle of the player based on position of the right analog stick
                angle = Mathf.Atan2(rStick.y, rStick.x) * Mathf.Rad2Deg;

                // Rotate player based on angle, also keep player rotated in new direction until it is changed again
                transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            }

        }
        else
        {
            Cursor.visible = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetText : MonoBehaviour {

    // variable to take player's input from an InputField
    public InputField fieldName;

    public static string entryName;

    public void SetGetName()
    {
        entryName = fieldName.text;
    }
}

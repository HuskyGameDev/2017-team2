using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class focusOnLoad : MonoBehaviour {

    public GameObject defaultButton;

	// Use this for initialization
	void Start () {
		if (defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }
	}
}

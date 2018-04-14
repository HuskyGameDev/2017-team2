using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonProgression : MonoBehaviour {

    private bool story = !DataBetweenScenes.isEndless;
    
    public GameObject nextStoryCanvas;
    public GameObject nextEndlessCanvas;

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(Activate);
    }

    // Use this for initialization
    void Activate () {
        
        if (story)
        {
            nextStoryCanvas.SetActive(true);
        } else
        {
            nextEndlessCanvas.SetActive(true);
        }

	}
}

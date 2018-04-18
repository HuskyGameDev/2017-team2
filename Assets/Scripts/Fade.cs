using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

    public Texture2D fadeTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDirection = -1;

    private void OnGUI()
    {
        alpha += fadeDirection * fadeSpeed * Time.unscaledDeltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);

    }

    public float BeginFade (int direction)
    {
        fadeDirection = direction;
        return (fadeSpeed);
    }

    void OnLoad()
    {
        BeginFade(-1);
    }
}

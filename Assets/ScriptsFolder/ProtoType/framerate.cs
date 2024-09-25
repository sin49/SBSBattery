using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class framerate : MonoBehaviour
{
    public int MaxFrame;

    public int fontsize = 30;
    public Color color = Color.green;
    public float width, height;

    private void Update()
    {
        Application.targetFrameRate = MaxFrame;
    }
    private void OnGUI()
    {
        Rect position=new Rect(width, height, Screen.width,Screen.height);

        float fps = 1.0f / Time.deltaTime;
        float ms = Time.deltaTime * 1000.0f;
        string text = string.Format($"[{Mathf.RoundToInt(fps)}] FPS ({Mathf.RoundToInt( ms)}ms)");

        GUIStyle style = new GUIStyle();
        style.fontSize = fontsize;
        style.normal.textColor = color;

        GUI.Label(position, text, style);
    }
}

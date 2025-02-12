using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChildCountTEst : MonoBehaviour
{
    public int counter;

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 120), "" + transform.childCount);
    }
}

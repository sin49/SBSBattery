using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItleText : MonoBehaviour
{
    public GameObject ImageHub;
    public event Action ButtonEffect;
    private void Awake()
    {
        //ImageHub.SetActive(false);
    }
    public void ActiveImageHub()
    {
        ImageHub.SetActive(true);
    }
    public void DeActiveImageHub()
    {
        ImageHub.SetActive(false);
    }
    public void removeevent()
    {
        ButtonEffect = null;
    }
    public void ButtonActive()
    {
        ButtonEffect.Invoke();
    }
}

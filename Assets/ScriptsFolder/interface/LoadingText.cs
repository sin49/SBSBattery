using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    public TextMeshProUGUI loadFont;

    private void Awake()
    {
        Debug.Log("loading image awake");
    }

    private void OnEnable()
    {
        if (LanguageManager.instance.isKor)
        {
            Debug.Log("한글");
            loadFont.text = LanguageManager.instance.loadingKor[0];
        }
        else
        {
            Debug.Log("영문");
            loadFont.text = LanguageManager.instance.loadingEng[0];
        }
    }
}

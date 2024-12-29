using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    public TextAsset languageCSV;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        
    }
}

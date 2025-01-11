using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TouchInterface : MonoBehaviour
{
    public GameObject buttonParent;
    TextMeshProUGUI[] fontList;
    public Color fontColor;

    private void Awake()
    {
        fontList = buttonParent.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var font in fontList)
        {
            font.color = fontColor;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

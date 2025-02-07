using SimpleInputNamespace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTouch : UIInteract, IPointerDownHandler, IPointerUpHandler
{
    public Sprite activeButton;
    public Sprite deactiveButton;

    TextMeshProUGUI tmp;

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = activeButton;
        tmp.color = activeFontColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = deactiveButton;
        tmp.color = deactiveFontColor;
    }

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

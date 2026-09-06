using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionEvent : OutputEvent
{
    public DeAcrtiveOnTimer Description;
    public float time;
    public string message;
    public override void output()
    {
        base.output();
        Description.time = time;
        if (Description.GetComponent<TextMeshProUGUI>())
        {
            Description.GetComponent<TextMeshProUGUI>().text = message;
        }
        Description.gameObject.SetActive(true);
    }
}

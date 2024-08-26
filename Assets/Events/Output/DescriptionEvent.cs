using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionEvent : OutputEvent
{
    public DeAcrtiveOnTimer Description;
    public float time;

    public override void output()
    {
        base.output();
        Description.time = time;
        Description.gameObject.SetActive(true);
    }
}

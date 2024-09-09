using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDeactiveOutputEvent : OutputEvent
{
    public override void output()
    {
        PlayerHandler.instance.CantHandle = true;
        base.output();   
    } 
}

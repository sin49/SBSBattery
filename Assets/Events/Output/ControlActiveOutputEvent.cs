using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlActiveOutputEvent : OutputEvent
{
    public override void output()
    {
        PlayerHandler.instance.outputCantHandle = false;
        base.output();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class signalactiveinput : InputEvent
{
    public signalSender sender;
    public override void initialize()
    {
        sender.active = false;
    }

    public override bool input(object o = null)
    {
        if (sender!=null&&sender.active)
            return true;
        else
            return false;
    }

}

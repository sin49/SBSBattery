using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAreaOutputEvent : OutputEvent
{
    public Collider killCollider;
    public override void output()
    {
        base.output();
    }
}

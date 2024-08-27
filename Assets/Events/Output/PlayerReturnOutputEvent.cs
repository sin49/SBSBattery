using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReturnOutputEvent : OutputEvent
{
    public override void output()
    {
        ReturnCheckPoint();
        base.output();
    }

    public void ReturnCheckPoint()
    {
        PlayerSpawnManager.Instance.Spawn();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : InteractiveObject
{
    public bool onLadder;

    public void InitPlayerPosition()
    {
        if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentPlayer != null)
        {
            Vector3 fixPos = transform.position + transform.forward * 0.5f;
            PlayerHandler.instance.CurrentPlayer.transform.position = fixPos;
        }
    }

    public override void Active(direction direct)
    {
        base.Active(direct);
    }
}

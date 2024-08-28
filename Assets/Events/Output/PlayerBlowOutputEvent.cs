using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlowOutputEvent : OutputEvent
{
    public override void output()
    {
        PlayerBlow();
        base.output();
    }

    public void PlayerBlow()
    {
        if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentPlayer != null)
        {
            PlayerHandler.instance.CurrentPlayer.Damaged(1);
        }
    }
}

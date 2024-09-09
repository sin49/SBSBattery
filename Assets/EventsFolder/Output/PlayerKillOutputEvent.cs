using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillOutputEvent : OutputEvent
{
    public override void output()
    {
        PlayerKill();
        base.output();
    }

    public void PlayerKill()
    {
        if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentPlayer != null)
        {
            PlayerHandler.instance.CurrentPlayer.Dead();
        }
    }
}

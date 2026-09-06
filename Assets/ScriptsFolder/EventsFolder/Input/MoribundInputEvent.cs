using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoribundInputEvent : InputEvent
{    
    public bool moribund;

    public override void initialize()
    {
        moribund = false;
    }

    public override bool input(object o)
    {
        return moribund;
    }

    private void Update()
    {
        if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentPlayer != null)
        {
            if (PlayerStat.instance.hp <= 1)
            {
                moribund = true;
            }
            else
            {
                moribund = false;
            }
        }
    }
}

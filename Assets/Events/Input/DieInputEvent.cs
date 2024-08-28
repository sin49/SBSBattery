using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieInputEvent : InputEvent
{
    public GameObject character;
    public bool cDie;
    public override void initialize()
    {
        cDie = false;
    }

    public override bool input(object o)
    {
        return cDie;
    }

    public void CharacterDie()
    {

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CharacterSkillInputEvent : InputEvent
{
public  bool tf;
    [Header("지금은 플레이어만 있음")]
    public int index = 1;

    public override void initialize()
    {
        tf = false;
    }

    public override bool input(object o = null)
    {
        return tf;
    }
    void activeskill()
    {
        tf = true;
    }
    private void Start()
    {
        if (index == 1)
        {
            PlayerHandler.instance.CurrentPlayer.registerskilleventhandler(activeskill);
        }
    }
}

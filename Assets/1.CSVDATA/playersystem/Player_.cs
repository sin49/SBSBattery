using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ : Player
{
    public PlayerSkills DownAttackEvent;

    public PlayerSkills AttackEvent;

    public PlayerSkills SpKeyEvent;

    public override void attackAction()
    {
        AttackEvent?.Invoke();

    }
    public override void DownAttackAction()
    {
       DownAttackEvent?.Invoke();
    }

    public override void Skill1()
    {
        SpKeyEvent?.Invoke();
    }
}

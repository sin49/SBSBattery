using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ : Player
{
    public PlayerSkills DownAttackEvent;

    public PlayerSkills AttackEvent;

    public PlayerSkills SpKeyEvent;

    public bool Onattack_;
    
    protected override void Start()
    {
        base.Start();
        if (AttackEvent != null)
            AttackEvent.initEvent();
    }
   
    public override void attackAction()
    {
        if(Onattack_)
        StartCoroutine(attackkeyeventinvoke());
    }

    IEnumerator attackkeyeventinvoke()
    {
        Onattack_ = true;
        AttackEvent?.Invoke();
        yield return new WaitForSeconds(PlayerStat.instance.attackDelay);
        Onattack_ = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyAction : EnemyAction
{
    //enemy 스크립트를 끌어서야할 필요성을 느껴서 사용
    public Enemy e;

    public virtual void register(Enemy e)
    {
        this.e = e;
    }
    public override void Invoke(Transform target = null)
    {
        base.Invoke(target);
        registerActionHandler(e.DelayTime);
        registerActionHandler(e.InitAttackCoolTime);
    }
}



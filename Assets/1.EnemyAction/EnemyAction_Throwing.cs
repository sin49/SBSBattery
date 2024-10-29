using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction_Throwing : NormalEnemyAction
{
    Enemy e;
    public override void register(Enemy e)
    {
        base.register(e);
        this.e = e;
    }
    public override void Invoke(Transform target = null)
    {
        base.Invoke(target);
        Transform fire = e.attackCollider.transform;
        if (PoolingManager.instance != null)
            PoolingManager.instance.GetPoolObject("EnemyBullet", fire.transform);
        e.PlayAttackSound();
        DisableActionMethod();
    }

   

}

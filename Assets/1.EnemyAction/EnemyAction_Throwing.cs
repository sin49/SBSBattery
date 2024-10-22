using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction_Throwing : NormalEnemyAction
{

    public override void Invoke(Transform target = null)
    {
        base.Invoke(target);
        Transform fire = e.attackCollider.transform;
        if (PoolingManager.instance != null)
            PoolingManager.instance.GetPoolObject("EnemyBullet", fire.transform);

        DisableActionMethod();
    }

   

}

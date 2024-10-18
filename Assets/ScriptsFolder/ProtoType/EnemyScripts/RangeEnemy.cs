using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    public GameObject rangePrefab;
    public Transform fire;

    public override void Attack()
    {
        base.Attack();
        if(PoolingManager.instance != null)
        PoolingManager.instance.GetPoolObject("EnemyBullet", fire.transform);

        StartCoroutine(WaitNextBehavior());
    }

    IEnumerator WaitNextBehavior()
    {
        yield return new WaitForSeconds(eStat.attackDelay);

        InitAttackCoolTime();
    }

    public override void StartEmmissionHitMat()
    {
        Material[] materials = mae.skinRenderer.materials;
        materials[0] = mae.emmissionBackMat;
        materials[1] = mae.emmissionHittedMat;
        mae.skinRenderer.materials = materials;
        if (mae.skinHead != null)
            mae.skinHead.material = mae.emmissionHeadMat;
    }

    public override void EndEmmissionHitMat()
    {
        base.EndEmmissionHitMat();
        if (mae.skinHead != null)
            mae.skinHead.material = mae.headMat;
    }

    public override void EndHitMat()
    {
        base.EndHitMat();
        if (mae.skinHead != null)
            mae.skinHead.material = mae.backMat;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    public GameObject rangePrefab;
    public Transform fire;

    public override void Attack()
    {
        if(PoolingManager.instance != null)
        PoolingManager.instance.GetPoolObject("EnemyBullet", fire.transform);

        StartCoroutine(WaitNextBehavior());
    }

    IEnumerator WaitNextBehavior()
    {
        yield return new WaitForSeconds(eStat.attackDelay);

        InitAttackCoolTime();
    }
}

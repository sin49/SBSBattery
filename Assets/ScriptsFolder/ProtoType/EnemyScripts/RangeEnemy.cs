using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    public GameObject rangePrefab;
    public Transform fire;

    [Header("이미션 머티리얼")]
    public Material emmissionHittedMat;
    public Material emmissionHeadMat;
    public Material emmissionBackMat;
    public Renderer skinHead;

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
        Material[] materials = skinRenderer.materials;
        materials[0] = emmissionBackMat;
        materials[1] = emmissionHittedMat;
        skinRenderer.materials = materials;
        if (skinHead != null)
            skinHead.material = emmissionHeadMat;
    }

    public override void EndEmmissionHitMat()
    {
        base.EndEmmissionHitMat();
        if (skinHead != null)
            skinHead.material = headMat;
    }

    public override void EndHitMat()
    {
        base.EndHitMat();
        if (skinHead != null)
            skinHead.material = backMat;
    }
}

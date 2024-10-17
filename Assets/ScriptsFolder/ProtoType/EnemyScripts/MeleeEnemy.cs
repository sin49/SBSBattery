using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("이미션 머티리얼")]
    public Material emmissionHittedMat;
    public Material emmissionHeadMat;
    public Material emmissionBackMat;
    public Renderer skinHead;

    public override void Attack()
    {
        attackCollider.SetActive(true);
        base.Attack();
        attackCollider.GetComponent<EnemyMeleeAttack>().AttackReady(this, eStat.attackDelay);
    }

    public override void StartEmmissionHitMat()
    {
        Material[] materials = mae.skinRenderer.materials;
        materials[0] = emmissionBackMat;
        materials[1] = emmissionHittedMat;
        mae.skinRenderer.materials = materials;
        if (skinHead != null)
            skinHead.material = emmissionHeadMat;
    }

    public override void EndEmmissionHitMat()
    {
        base.EndEmmissionHitMat();
        if (skinHead != null)
            skinHead.material = mae.headMat;
    }

    public override void EndHitMat()
    {
        base.EndHitMat();
        if (skinHead != null)
            skinHead.material = mae.backMat;
    }
}

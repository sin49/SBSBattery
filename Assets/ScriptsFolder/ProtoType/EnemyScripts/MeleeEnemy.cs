using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public override void Attack()
    {
        attackCollider.SetActive(true);
        base.Attack();
        attackCollider.GetComponent<EnemyMeleeAttack>().AttackReady(this, eStat.attackDelay);
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

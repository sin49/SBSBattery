using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BulbEnemy : Enemy
{
    public ParticleSystem explosion;
    public bool lightCheck;

    private void Update()
    {
        if (!mae.attackColliderRange.wallCheck && !mae.searchCollider.wallCheck)
        {
            ReadyAttackTime();
        }
    }

    // 0번째: 필라멘트, 1번째: 바디, 2번째: 헤드
    public override void StartEmmissionHitMat()
    {
        Material[] materials = mae.skinRenderer.materials;
        materials[0] = mae.emmissionBackMat; // 필라멘트
        materials[1] = mae.emmissionHittedMat; // 바디
        materials[2] = mae.emmissionHeadMat; //헤드

        mae.skinRenderer.materials = materials;
    }

    public override void EndEmmissionHitMat()
    {
        Material[] materials = mae.skinRenderer.materials;
        materials[0] = mae.backMat;
        materials[1] = mae.idleMat;
        materials[2] = mae.headMat;

        mae.skinRenderer.materials = materials;
    }

    public override void EndHitMat()
    {
        return;
    }

    public override void Attack()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player;
            if (collision.gameObject.TryGetComponent<Player>(out player))
            {
                if (!player.onInvincible)
                {
                    player.Damaged(eStat.atk);
                    Dead();
                }
            }
        }
    }
}

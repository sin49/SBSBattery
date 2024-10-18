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

    // 0��°: �ʶ��Ʈ, 1��°: �ٵ�, 2��°: ���
    public override void StartEmmissionHitMat()
    {
        Material[] materials = mae.skinRenderer.materials;
        materials[0] = mae.emmissionBackMat; // �ʶ��Ʈ
        materials[1] = mae.emmissionHittedMat; // �ٵ�
        materials[2] = mae.emmissionHeadMat; //���

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

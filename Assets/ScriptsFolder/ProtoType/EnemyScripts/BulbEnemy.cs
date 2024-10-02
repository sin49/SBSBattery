using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BulbEnemy : Enemy
{
    public ParticleSystem explosion;
    public bool lightCheck;

    private void Update()
    {
        if (!wallCheck)
        {
            ReadyAttackTime();
        }

        if (reachCheck)
        {
            Debug.Log("����");
            Dead();
        }
    }

    // 0��°: �ʶ��Ʈ, 1��°: �ٵ�, 2��°: ���
    public override void StartEmmissionHitMat()
    {
        Material[] materials = skinRenderer.materials;
        materials[0] = emmissionBackMat; // �ʶ��Ʈ
        materials[1] = emmissionHittedMat; // �ٵ�
        materials[2] = emmissionHeadMat; //���

        skinRenderer.materials = materials;
    }

    public override void EndEmmissionHitMat()
    {
        Material[] materials = skinRenderer.materials;
        materials[0] = backMat;
        materials[1] = idleMat;
        materials[2] = headMat;

        skinRenderer.materials = materials;
    }

    public override void EndHitMat()
    {
        return;
    }

    public override void Attack()
    {
        
    }    
}

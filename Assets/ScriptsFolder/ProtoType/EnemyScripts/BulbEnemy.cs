using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BulbEnemy : Enemy
{
    public ParticleSystem explosion;
    public bool lightCheck;
    [Header("�̹̼� ��Ƽ����")]
    public Material emmissionHittedMat;
    public Material emmissionHeadMat;
    public Material emmissionBackMat;

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
        Material[] materials = mae.skinRenderer.materials;
        materials[0] = emmissionBackMat; // �ʶ��Ʈ
        materials[1] = emmissionHittedMat; // �ٵ�
        materials[2] = emmissionHeadMat; //���

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
}

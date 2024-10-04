using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BulbEnemy : Enemy
{
    public ParticleSystem explosion;
    public bool lightCheck;
    [Header("이미션 머티리얼")]
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
            Debug.Log("자폭");
            Dead();
        }
    }

    // 0번째: 필라멘트, 1번째: 바디, 2번째: 헤드
    public override void StartEmmissionHitMat()
    {
        Material[] materials = skinRenderer.materials;
        materials[0] = emmissionBackMat; // 필라멘트
        materials[1] = emmissionHittedMat; // 바디
        materials[2] = emmissionHeadMat; //헤드

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

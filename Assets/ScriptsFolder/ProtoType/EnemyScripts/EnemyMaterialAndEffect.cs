using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMaterialAndEffect : MonoBehaviour
{
   


    [Header("기본 머티리얼")]
    [Header("기본 몹 얼굴, 전구몹 / 점프몹 베이스, 나머지 기본 머티리얼")] public Material idleMat;
    [Header("기본 몹 등부분, 전구 몬스터 필라멘트")] public Material backMat;
    [Header("기본 몹 바디 머티리얼, 전구몹 유리 / 점프몹 유리")] public Material headMat;
    [Header("기본몹 깝놀, 전구몹 유리, ")] public Material hittedMat;
    [Header("기본몹, 전구몹")]public Renderer skinRenderer;

    [Header("Emmission 머티리얼(피격 시 다른 색깔 ==> 붉은 머티리얼)")] 
    [Header("기본몹 얼굴, 점프몹 바디, 전구몹 필라멘트")]public Material emmissionBackMat;
    [Header("기본몹 바디, 전구몹 유리")]public Material emmissionHeadMat;
    [Header("기본몹 베이스, 전구몹 바디")]public Material emmissionHittedMat;
    [Header("일반몹만 사용")]public Renderer skinHead; //  일반 몬스터만 씀
    // 몬스터 머티리얼 부분(모델링 상태)
    // 3개 => 전구몬스터(몸통, 전구유리, 필라멘트)
    // 2개 / 1개(==스킨 렌더러 2개)  => 일반몬스터(얼굴,등), (머리 막대기)
    // 2개 => 점프 몬스터(몸통, 얼굴 투명관)
    // 1개 => 돌진 몬스터, 불 몬스터

    [Header("사망이펙트")] public ParticleSystem deadEffect;

    public void StartEmmissionHitMat()
    {
        //if(emmissionBackMat !=null)
        Material[] materials = skinRenderer.materials;
        switch (materials.Length)
        {
            case 1:
                materials[0] = emmissionHittedMat;
                skinRenderer.materials = materials;
                break;
            case 2:
                materials[0] = emmissionBackMat;
                materials[1] = emmissionHittedMat;
                skinRenderer.materials = materials;
                break;
            case 3:
                materials[0] = emmissionBackMat;
                materials[1] = emmissionHeadMat;
                materials[2] = emmissionHittedMat;
                skinRenderer.materials = materials;
                break;
            default:
                break;
        }

        if (skinHead != null)
            skinHead.material = emmissionBackMat;
    }

    public void EndEmmissionHitMat()
    {
        Material[] materials = skinRenderer.materials;
        switch (materials.Length)
        {
            case 1:
                materials[0] = hittedMat;
                skinRenderer.materials = materials;
                break;
            case 2:
                materials[0] = backMat;
                materials[1] = hittedMat;
                skinRenderer.materials = materials;
                break;
            case 3:
                materials[0] = backMat;
                materials[1] = headMat;
                materials[2] = hittedMat;
                skinRenderer.materials = materials;
                break;
            default:
                break;
        }

        if (skinHead != null)
            skinHead.material = backMat;
    }

    public void EndHitMat()
    {
        Material[] materials = skinRenderer.materials;
        switch (materials.Length)
        {
            case 1:
                materials[0] = idleMat;
                skinRenderer.materials = materials;
                break;
            case 2:
                materials[0] = idleMat;
                materials[1] = backMat;
                skinRenderer.materials = materials;
                break;
            case 3:
                materials[0] = idleMat;
                materials[1] = headMat;
                materials[2] = backMat;
                skinRenderer.materials = materials;
                break;
            default:
                break;
        }

        if (skinHead != null)
            skinHead.material = backMat;
    }
}

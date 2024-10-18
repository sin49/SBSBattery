using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaterialAndEffect : MonoBehaviour
{
   


    [Header("기본 머티리얼")]
    public Material idleMat;
    public Material backMat;
    public Material headMat;
    public Material hittedMat;
    public Renderer skinRenderer;

    public ParticleSystem deadEffect;
    public ParticleSystem moveEffect;

    [Header("Emmission 머티리얼")] 
    public Material emmissionBackMat;
    public Material emmissionHeadMat;
    public Material emmissionHittedMat;
    [Header("일반몹만 사용")]public Renderer skinHead; //  일반 몬스터만 씀
    // 몬스터 머티리얼 부분(모델링 상태)
    // 3개 => 전구몬스터(몸통, 전구유리, 필라멘트)
    // 2개 / 1개(==스킨 렌더러 2개)  => 일반몬스터(얼굴,등), (머리 막대기)
    // 2개 => 점프 몬스터(몸통, 얼굴 투명관)
    // 1개 => 돌진 몬스터, 불 몬스터
}

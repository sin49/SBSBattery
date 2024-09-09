using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseTransform : Player
{
    [Header("돌진 준비시간")]
    public float rushReady;
    [Header("돌진 이동속도")]
    public float rushSpeed;
    [Header("돌진 지속시간")]
    public float rushTimeMax;
    float rushTimer;
    [Header("돌진 재사용 대기시간")]
    public float rushCoolTime;
    float coolTimer;
    [Header("돌진 공격 피해량")]
    public float rushDamage;
    [Header("돌진 공격 주기")]
    public float rushAtkCycle;

    private void Update()
    {
        BaseBufferTimer();
    }

    public void RushStart()
    {

    }
}

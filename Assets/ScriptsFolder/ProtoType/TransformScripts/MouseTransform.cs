using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseTransform : Player
{
    [Header("���� �غ�ð�")]
    public float rushReady;
    [Header("���� �̵��ӵ�")]
    public float rushSpeed;
    [Header("���� ���ӽð�")]
    public float rushTimeMax;
    float rushTimer;
    [Header("���� ���� ���ð�")]
    public float rushCoolTime;
    float coolTimer;
    [Header("���� ���� ���ط�")]
    public float rushDamage;
    [Header("���� ���� �ֱ�")]
    public float rushAtkCycle;

    private void Update()
    {
        BaseBufferTimer();
    }

    public void RushStart()
    {

    }
}

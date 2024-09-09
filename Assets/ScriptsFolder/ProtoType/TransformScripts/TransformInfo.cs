using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpecialAttackInfo
{
    public string saName; // 특수공격 이름
    public float saDamage; // 특수공격 대미지
    public float saDesc; // 특수공격 설명
    public float saPowerEnergy; // 소모 에너지
    public GameObject saPrefab;
}

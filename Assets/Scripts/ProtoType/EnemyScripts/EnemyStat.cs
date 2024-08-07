using System;
using UnityEngine;

[Serializable]
public class Drop
{
    public GameObject parts; // 재화로 활용할 부품 오브젝트
    public int partValue; // 부품 수를 나타낼 변수 (골드 개념)
}

public class EnemyStat : CharacterStat
{
    [Header("적 상태변화")]
    public EnemyState eState;
    [Header("드랍 아이템 임의로 작성(사용할지 모름)")]
    public Drop dropGroup; // 사망 시, 드랍할 재화에 대한 클래스 변수
    [Header("적 무적관련 변수")]
    public bool onInvincible;
    public float invincibleTimer;
}

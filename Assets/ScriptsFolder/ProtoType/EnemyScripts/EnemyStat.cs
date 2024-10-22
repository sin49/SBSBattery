using System;
using UnityEngine;

//드랍 부분 완성하기
[Serializable]
public class Drop
{
    public GameObject parts; // 재화로 활용할 부품 오브젝트
    public int partValue; // 부품 수를 나타낼 변수 (골드 개념)
}

public class EnemyStat : CharacterStat
{
    [Header("몬스터의 이동 AI")] public EnemyMovePattern movepattern;
    [Header("몬스터의 이동 방식")] public EnemyMoveType movetype;
    [Header(("몬스터의 공격 방식"))] public EnemyAttackType attacktype;
    [Header("드랍 아이템 임의로 작성(사용할지 모름)")]
    public Drop dropGroup; // 사망 시, 드랍할 재화에 대한 클래스 변수

}

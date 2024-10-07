using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Status : MonoBehaviour
{
    [Header("보스 스탯")]
    [Header("보스 모니터 체력(노기능)")]
    public int lifeCountMax;
    [Header("랜덤 패턴(끄면 순서대로)")]
    public bool randomPattern;
    [Header("휩쓸기 패턴만 사용")]
    public bool OnlySweapPattern;
    [Header("레이저 패턴만 사용")]
    public bool OnlylaserPattern;
    [Header("낙하물 패턴만 사용")]
    public bool OnlyfallPattern;
    [Header("손 체력(플레이모드중 바꿔도 적용 안됨)")]
    public float HandHP;
    [Header("손 파괴시 이동 위치")]
    public Transform LhandDefeatTransform;
    public Transform RhandDefeatTransform;
    [Header("보스 낙하물")]
    [Header("보스 낙하물리스트(플레이모드중 바꿔도 적용 안됨)")]
    public List<Boss1FallObj> fallingObj2;
    [Header("낙하 상자 오브젝트 생성(플레이모드중 바꿔도 적용 안됨)")]
    public List<Boss1BoxFallCreateObj> fallingBoxCreateObj;
    [Header("데미지")]
    public float damage;
    [Header("생성시간, 최소/최대속도")]
    public float createTime;
    public float minSpeed;
    public float maxSpeed;
    [Header("낙하물 카운트 제한, 낙하 높이")]
    public int createCountMax;
    public float fallingHeight;
    [Header("낙하 범위 조정")]
    public float fallingRange;
    [Header("낙하 범위 기즈모 색")]
    public Color GizmoColor;
    [Header("보스 휩쓸기")]
    [Header("손 크기(손 y축 오프셋 전용)")]
    public float handsize = 1;
    [Header("휩쓸기 한 번 후 다음 휩쓸기 까지의 간격")]
    public float SweaperPatternDealy;
    [Header("테스트 중 숫자가 0이되면 고장남")]
    [Header("휩쓸기 위해 손이 휩쓸기 시작 지점까지 가는 시간")]
    public float SweaperStartMoveTime;
    [Header("시작지정에서 대기하는 시간")]
    public float sweaperwaitTime;
    [Header("시작지점에서 목표지점까지 가는 시간")]
    public float SweaperEndMoveTime;
    [Header("목표지점에서 대기하는 시간")]
    public float SweaperEndWaitTime;
    [Header("목표지점까지 이동 후 다시 원위치하는 시간")]
    public float sweaperReturnTime;
    [Header("휩쓸기 기즈모 색깔")]
    public Color SweapColor;
    [Header("보스 레이저")]
    [Header("레이저 지속 시간")]
    public float laserlifetime = 1.5f;
    [Header("레이저 Y축 위치")]
    public float LaserYpos = -6.4f;

    [Header("레이저 속도")]
    public float LaserSpeed;
    [Header("레이저 활성화 까지의 시간")]
    public float laserActiveTimer = 1.5f;
    [Header("장판 범위")]
    public float TrailColScale = 1;
    [Header("장판 지속 시간")]
    public float TrailDuration;
    [Header("장판 공격 판정 생성 간격")]
    public float ColliderSpawnTime;


    Boss1Laser laser;
    BossTv boss;
    BossFalling BossFalling;
    Boss1Sweap sweap;
    private void Awake()
    {
        laser=transform.parent.GetComponent<Boss1Laser>();
        boss= transform.parent.GetComponent<BossTv>();
        BossFalling=transform.parent.GetComponent<BossFalling>();
        sweap = transform.parent.GetComponent<Boss1Sweap>();
        updateStatus();
    }
    private void Update()
    {
        updateStatus();
    }
    void updateStatus()
    {
        if (boss != null)
        {
            boss.lifeCountMax = lifeCountMax;
            boss.HandHP = HandHP;
            boss.randomPattern = randomPattern;
            if (OnlySweapPattern)
            {
                boss.OnlyTestPattern = true;
                boss.TestAction = sweap;
            }else if (OnlyfallPattern)
            {
                boss.OnlyTestPattern = true;
                boss.TestAction = BossFalling;
            }else if (OnlylaserPattern)
            {
                boss.OnlyTestPattern = true;
                boss.TestAction = laser;
            }
            else
            {
                boss.OnlyTestPattern = false;
            }
            sweap.LhandDefeatTransform = LhandDefeatTransform;
            sweap.RhandDefeatTransform= RhandDefeatTransform;
            sweap.handsize = handsize;
            sweap.SweaperEndMoveTime= SweaperEndMoveTime;
            sweap.SweaperEndWaitTime= SweaperEndWaitTime;
            sweap.sweaperReturnTime= sweaperReturnTime;
            sweap.SweaperStartMoveTime= SweaperStartMoveTime;
            sweap.sweaperwaitTime= sweaperwaitTime;
            sweap.SweaperPatternDealy = SweaperPatternDealy;
            sweap.sweapColor = SweapColor;

            laser.ActionLifeTIme = laserlifetime;
            laser.LaserYpos = LaserYpos;
            laser.laserActiveTimer = laserActiveTimer;
            laser.TrailColScale = TrailColScale;
            laser.TrailDuration = TrailDuration;
            laser.ColliderSpawnTime = ColliderSpawnTime;

            BossFalling.fallingObj2 = fallingObj2;
            BossFalling.fallingBoxCreateObj = fallingBoxCreateObj;

            BossFalling.damage = damage;
            BossFalling.createTime = createTime;
            BossFalling.minSpeed = minSpeed;
            BossFalling.maxSpeed = maxSpeed;

            BossFalling.createCountMax = createCountMax;
            BossFalling.fallingRange = fallingRange;
            BossFalling.fallingHeight = fallingHeight;
            BossFalling.GizmoColor = GizmoColor;
        }
    }
}

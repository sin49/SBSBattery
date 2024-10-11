using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Status : MonoBehaviour
{
    [Header("보스 스탯")]
    [Header("보스 모니터 체력(노기능)")]
    public int lifeCountMax;
    [Header("패턴 후 딜레이시간")]
    public float patterndelay;
    [Header("랜덤 패턴(끄면 순서대로)")]
    public bool randomPattern;
    [Header("휩쓸기 패턴만 사용")]
    public bool OnlySweapPattern;
    [Header("레이저 패턴만 사용")]
    public bool OnlylaserPattern;
    [Header("레이저2D 패턴만 사용")]
    public bool Onlylaser2DPattern;
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
    [Header("내려찍기 횟수")]
    public int stombcount;
    [Header("내려찍을 때 y축 높이 조정")]
    public float stombYPlus;
    [Header("내려찍기 준비 시간")]
    public float stombinittime;

    [Header("내려찍기 전 대기 시간")]
    public float stombwaitTIme;
    [Header("내려찍는 시간")]
    public float stombtime;
    [Header("내려찍기 찍고 대기 시간")]
    public float stombendwaitTIme;
    [Header("내려찍고 위로 돌아오는 시간")]
    public float stombreturntime;
    [Header("위에서 제자리로 돌아오는 시간")]
    public float stombreturntime2;
    [Header("내려찍는 y좌표")]
    public float  stombYpos ;


    [Header("보스 휩쓸기(이거 빼면 2페이즈 부분에 값 새로 넣어야 되가지고 일단 놔둠)")]
  
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


  
    [Header("레이저2D 경고 시간")]
    public float laserwarngingTIme;
    [Header("레이저2D 활성화 시간")]
    public float laseractiveTIme;


    Boss1Laser laser;
    BossTv boss;
    BossFalling BossFalling;
    Boss1Sweap sweap;
    public Boss1Laser2D laser2D;
    private void Awake()
    {
        laser = transform.parent.GetComponent<Boss1Laser>();
        boss = transform.parent.GetComponent<BossTv>();
        BossFalling = transform.parent.GetComponent<BossFalling>();
        sweap = transform.parent.GetComponent<Boss1Sweap>();

        updateStatus();
    }
    private void Update()
    {
        if(!boss.Phase2)
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
            }
            else if (OnlyfallPattern)
            {
                boss.OnlyTestPattern = true;
                boss.TestAction = BossFalling;
            }
            else if (OnlylaserPattern)
            {
                boss.OnlyTestPattern = true;
                boss.TestAction = laser;
            }
          else if (Onlylaser2DPattern) {
                boss.OnlyTestPattern = true;
                boss.TestAction = laser2D;
            }

            else
            {
                boss.OnlyTestPattern = false;
            }
            boss.patterndelay = patterndelay;
            sweap.LhandDefeatTransform = LhandDefeatTransform;
            sweap.RhandDefeatTransform = RhandDefeatTransform;
            sweap.handsize = handsize;
            sweap.SweaperEndMoveTime = SweaperEndMoveTime;
            sweap.SweaperEndWaitTime = SweaperEndWaitTime;
            sweap.sweaperReturnTime = sweaperReturnTime;
            sweap.SweaperStartMoveTime = SweaperStartMoveTime;
            sweap.sweaperwaitTime = sweaperwaitTime;
            sweap.SweaperPatternDealy = SweaperPatternDealy;
            sweap.sweapColor = SweapColor;
            sweap.stombcount = stombcount;
            sweap.stombYPlus= stombYPlus;
            sweap.stombinittime = stombinittime;
            sweap.stombtime = stombtime;
            sweap.stombwaitTIme= stombwaitTIme;
            sweap.stombendwaitTIme= stombendwaitTIme;
            sweap.stombreturntime= stombreturntime;
            sweap.stombYEnd = stombYpos;
            sweap.stombwaitTIme2 = stombreturntime2;
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

            laser2D.LaserWaringTime = laserwarngingTIme;
            laser2D.laseractiveTime = laseractiveTIme;

        }
    }
}


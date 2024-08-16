using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//주석처리
//패턴 테스트 편하게
public class BossTv : RemoteObject
{
    public GameObject Monitor;
    public EnemyAction BossSweap;
    public EnemyAction BossLaser;
    public EnemyAction BossFall;
   
    public EnemyAction TestAction;
    List<EnemyAction> actions=new List<EnemyAction>();
    [HideInInspector]
    public int lifeCount;
    [Header("보스 모니터 체력(노기능)")]
    public int lifeCountMax;

    bool onPattern;


    int index;
    [Header("플레이어 추격")]
    public bool TargetPlayer;

    [Header("랜덤 패턴(끄면 순서대로)")]
    public bool randomPattern;
    [Header("테스트 패턴만 사용(우선순위 높음)")]
    public bool OnlyTestPattern;
    [Header("손 체력")]
    public float HandHP;


   public Boss1Hand LHand;

    public Boss1Hand RHand;



    public Transform target;
 
    protected override void Awake()
    {
     
        actions.Add(BossSweap);
        actions.Add(BossLaser);
        actions.Add(BossFall);
    }
    private void Start()
    {
        Debug.Log("보스 활성화 연출이 들어간다");
        LHand.HP = HandHP;
        RHand.HP = HandHP;

        LHand.active = true;
        RHand.active = true;
        LHand.HandDominateEvent += LhandDominateEvent;
        RHand.HandDominateEvent += RhandDominateEvent;
        lifeCount = lifeCountMax;
        CanControl = false;
    }
    void LhandDominateEvent()
    {
        LHand.active = false;
        HandDominateEvent();
    }
    void RhandDominateEvent()
    {
        RHand.active = false;
        HandDominateEvent();
    }
    void HandDominateEvent()
    {
        CancelAction();
        if (!LHand.active && !RHand.active)
        {
            CanControl = true;
        }
    }
   
    private void FixedUpdate()
    {
        if (TargetPlayer && PlayerHandler.instance != null)
            target = PlayerHandler.instance.CurrentPlayer.transform;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("테스트 왼팔이 때짐");
            LHand.HP = 1;
            LHand.Damaged(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("테스트 오른팔이 때짐");
            RHand.HP = 1;
            RHand.Damaged(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("테스트 모니터가 조종 당함");
            if (CanControl)
                Active();
        }
        if (CanControl)
        {
            if (PlayerHandler.instance.CurrentType == TransformType.remoteform && PlayerHandler.instance.CurrentPlayer.GetComponent<RemoteTransform>().closestObject != this.gameObject)
            {
                PlayerHandler.instance.CurrentPlayer.GetComponent<RemoteTransform>().closestObject = this.gameObject;
            }
            return;
        }
        if (!LHand.active && !RHand.active)
            return;
        DoAction();
    }
    void CancelAction()
    {
        Debug.Log("행동이 취소당함");
        TestAction.StopAction();
    }
    void DoAction()
    {
        if (!onPattern)
        {
            if (!OnlyTestPattern)
            {
                if (randomPattern)
                {
                    int rand = UnityEngine.Random.Range(0, actions.Count);
                    TestAction = actions[rand];
                }
                else
                {
                    TestAction = actions[index];

                    index++;
                    if (index >= actions.Count)
                        index = 0;
                }
            }
            TestAction.Invoke(patternComplete,target);
            Debug.Log("실행됨");
            onPattern = true;
        }

    }
    void patternComplete()
    {
        onPattern = false;
        Debug.Log("실행 완료");
        //어쩌구저쩌구
    }

    public override void Active()
    {
     base.Active();
        Debug.Log("모니터 공격 연출이 들어간다");
        CanControl = false;
        lifeCount = 0;
        Debug.Log("보스를 클리어 한다");
    }

    public override void Deactive()
    {
        throw new NotImplementedException();
    }
}

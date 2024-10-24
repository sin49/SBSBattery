using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//주석처리
//패턴 테스트 편하게
public class BossTv : RemoteObject
{

    bool BossEnable;
    Animator animator;
    public void BossActive()
    {
        BossEnable = true;
        UI.gameObject.SetActive(true);
        BossSweap.GetComponent<Boss1Sweap>().SetHandPosition();
        animator.enabled = false;
    }
    public void BossDeActive()
    {
        BossEnable = false;
        UI.gameObject.SetActive(false);

    }
    public Boss1UI UI;
    [Header("보스는 SoundEffectListPlayer와")]
    [Header("boss1SoundManager 둘다 넣으면 됨")]

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
        base.Awake();
        bossaudioplayer = GetComponent<Boss1SOundManager>();
        actions.Add(BossSweap);
        actions.Add(BossLaser);
        actions.Add(BossFall);
        animator=GetComponent<Animator>();
    }
    private void Start()
    {
        UI.gameObject.SetActive(false);
        Debug.Log("보스 활성화 연출이 들어간다");
        LHand.HP = HandHP;
        RHand.HP = HandHP;

        LHand.active = true;
        RHand.active = true;
        LHand.HandDominateEvent += LhandDominateEvent;
        RHand.HandDominateEvent += RhandDominateEvent;
        lifeCount = lifeCountMax;
        CanControl = false;
        index = actions.Count - 1;
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
        if (!BossEnable)
        {
            return;
        }
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
                animator.enabled = true;
                animator.SetBool("canactive", CanControl);
                PlayerHandler.instance.CurrentPlayer.GetComponent<RemoteTransform>().GetClosestObjectIgnoreTrigger(this.gameObject);

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

                    index--;
                    if (index <0)
                        index = actions.Count-1;
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
    Boss1SOundManager bossaudioplayer;
    public override void Active()
    {
     base.Active();
        Debug.Log("모니터 공격 연출이 들어간다");
        animator.Play("BossDefeat");
        if(bossaudioplayer!=null)
        bossaudioplayer.MonitiorHittedClipPlay();
        CanControl = false;
        PlayerHandler.instance.CurrentPlayer.GetComponent<RemoteTransform>().RemoveClosesObject();
        lifeCount = 0;
        Debug.Log("보스를 클리어 한다");
    }

    public override void Deactive()
    {
        throw new NotImplementedException();
    }
}

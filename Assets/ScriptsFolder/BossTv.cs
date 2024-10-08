using Cinemachine;
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
      ;
        animator.enabled = false;
    }
    public void BossDeActive()
    {
        BossEnable = false;
        UI.gameObject.SetActive(false);

    }
    public CinemachineImpulseSource shaker;
    public void CameraShake()
    {
        shaker.GenerateImpulse();
    }
    public void Change3DCamera()
    {
        var c_manager = PlayerHandler.instance.CurrentCamera.GetComponent<CameraManager_Switching2D3D>();
        //c_manager.trans3D = true;
        PlayerHandler.instance.DimensionChange();
        //StartCoroutine(c_manager.SwitchCameraForTransDimensionCorutinenoblending());
    }
    public void PlayerEnableCantHandle()
    {
        PlayerHandler.instance.CantHandle = true;
    }
    public void PlayerDisableCantHandle()
    {
        PlayerHandler.instance.CantHandle = false;
    }
    public Boss1UI UI;
    [Header("보스는 SoundEffectListPlayer와")]
    [Header("boss1SoundManager 둘다 넣으면 됨")]

    public GameObject Monitor;
    public EnemyAction BossSweap;
    public EnemyAction BossLaser;
    //public EnemyAction BossLaser2D;
    public EnemyAction BossFall;
   
    public EnemyAction TestAction;

    public bool Phase2;
    List<EnemyAction> actions=new List<EnemyAction>();
    [HideInInspector]
    public int lifeCount;
    [Header("보스 모니터 체력(노기능)")]
    public int lifeCountMax;

    bool onPattern;


    int index;


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
        //actions.Add(BossLaser2D);
        actions.Add(BossFall);
        animator =GetComponent<Animator>();
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
    public void animationEnd()
    {
        animator.enabled = false;
    }
    void HandDominateEvent()
    {
        CancelAction();
        if (!LHand.active && !RHand.active)
        {
            Phase2 = true;
            //CanControl = true;
            //actions.Remove(BossLaser2D);
            actions.Add(BossLaser);
            animator.enabled = true;
            animator.Play("Boss1PhaseChange");
            Debug.Log("2페이즈 전환 연출");
        }
    }
   
    private void FixedUpdate()
    {
        if (!BossEnable)
        {
            return;
        }
        if (PlayerHandler.instance != null)
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
                PlayerHandler.instance.CurrentPlayer.GetComponent<RemoteTransform>().GetClosestObjectIgnoreTrigger(this);

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

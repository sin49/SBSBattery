using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ּ�ó��
//���� �׽�Ʈ ���ϰ�
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
    [Header("������ SoundEffectListPlayer��")]
    [Header("boss1SoundManager �Ѵ� ������ ��")]

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
    [Header("���� ����� ü��(����)")]
    public int lifeCountMax;

    bool onPattern;


    int index;


    [Header("���� ����(���� �������)")]
    public bool randomPattern;
    [Header("�׽�Ʈ ���ϸ� ���(�켱���� ����)")]
    public bool OnlyTestPattern;
    [Header("�� ü��")]
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
        Debug.Log("���� Ȱ��ȭ ������ ����");
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
            Debug.Log("2������ ��ȯ ����");
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
            Debug.Log("�׽�Ʈ ������ ����");
            LHand.HP = 1;
            LHand.Damaged(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("�׽�Ʈ �������� ����");
            RHand.HP = 1;
            RHand.Damaged(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("�׽�Ʈ ����Ͱ� ���� ����");
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
        Debug.Log("�ൿ�� ��Ҵ���");
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
        Debug.Log("���� �Ϸ�");
        //��¼����¼��
    }
    Boss1SOundManager bossaudioplayer;
    public override void Active()
    {
     base.Active();
        Debug.Log("����� ���� ������ ����");
        animator.Play("BossDefeat");
        if(bossaudioplayer!=null)
        bossaudioplayer.MonitiorHittedClipPlay();
        CanControl = false;
        PlayerHandler.instance.CurrentPlayer.GetComponent<RemoteTransform>().RemoveClosesObject();
        lifeCount = 0;
        Debug.Log("������ Ŭ���� �Ѵ�");
    }

    public override void Deactive()
    {
        throw new NotImplementedException();
    }
}

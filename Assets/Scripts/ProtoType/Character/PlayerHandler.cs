using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;



public class PlayerHandler : MonoBehaviour
{
  public  event Action PlayerDeathEvent;
    public void InvokePlayerDeathEvent()
    {
        PlayerDeathEvent?.Invoke();
        PlayerDeathEvent = null;
    }
    float Skill1InputTimer;
    float Skill1InputCheck = 0.12f;
    public bool formChange;
    #region �÷��̾� ���Ű��� ����
    public float CurrentPower;
    public float MaxPower=60;
    public bool OnDeformField;
    public TransformType retoretype=TransformType.Default;
    public TransformPlace LastTransformPlace;

    public IngameUIManager ingameUIManger;

    public Camera CurrentCamera;
    #endregion
    InteractiveObject interactobject;
    float InteractTimer;
    [Header("�׽� ����")]
    [Tooltip("���� on/off���")]public bool AlwaysInvincible;
    [Tooltip("�̰� üũ�ؾ� ���� �� ����")]public bool alwaysFuncActive;
    public void GetInteratObject(InteractiveObject i)
    {
        interactobject = i;
    }
    public void InitInteratObject()
    {
        interactobject = null;
    }
    public InteractiveObject ReturnInteractObject()
    {
        return interactobject;
    }
    #region �÷��̾� ���� ��ġ,����

    GameObject Playerprefab;

    public Player CurrentPlayer;// �ൿ �۾�
    public void registerPlayer(GameObject o)//?
    {
        Playerprefab = o;
        CurrentPlayer = o.GetComponent<Player>();
    }
    public PlayerStat pStat; //���� �й� (����Ⱦ���)
    public direction lastDirection = direction.Right;
    #endregion
    #region �̱���
    public static PlayerHandler instance;
    #endregion
    private void Awake()
    {
        #region �̱���
        if (instance == null)
        {
           instance= this;
        }
        #endregion
        PlayerFormList p;
        if (TryGetComponent<PlayerFormList>(out p)){
            PlayerTransformList = p.playerformlist.Select((Value, index) => (Value, index))
                .ToDictionary(item => (TransformType)item.index, item => item.Value);
        }
        #region ĳ���� �ʱ�ȭ
   
        //CreateModelByCurrentType();
        #endregion
   
    }
    [Header("�÷��̾� ���� ����?")]
    public float characterFallLimit;
    float CalculateCharacterFallLimit;
    event Action PlayerFallEvent;
    public void registerPlayerFallEvent(Action action)
    {
        PlayerFallEvent =action;
    }
   public void PlayerFallOut()
    {
       
            Rigidbody rb=null;
          if(CurrentPlayer.TryGetComponent<Rigidbody>(out rb))
            {
                rb.velocity = Vector3.zero;
            }
            CurrentPlayer.transform.position = PlayerSpawnManager.Instance. CurrentCheckPoint.transform.position;
        //if(!AlwaysInvincible)
        CurrentPlayer.DamagedIgnoreInvincible(1);
        PlayerFallEvent?.Invoke();
      
    }
    private void FixedUpdate()
    {
        //if (CurrentType != TransformType.Default)
        //{
        //    CurrentPower -= Time.deltaTime;

        //}
        if(CurrentPlayer!=null&&CurrentPlayer.transform.position.y<-Mathf.Abs(characterFallLimit)+-5)
        PlayerFallOut();
        
        if (alwaysFuncActive)
        {
            if (AlwaysInvincible)
                CurrentPlayer.onInvincible = true;
            else
                CurrentPlayer.onInvincible = false;
        }
    

        #region ĳ���� ����
        if ((CurrentPlayer != null && !formChange) || CantHandle)
        charactermove();
        #endregion
    }
    public bool CantHandle;
    //public float CantHandleTimer;
    #region ���� �ý���
    #region ����
    public TransformType CurrentType =0;
    Dictionary<TransformType, GameObject> PlayerTransformList = new Dictionary<TransformType, GameObject>();

    Dictionary<TransformType, GameObject> CreatedTransformlist = new Dictionary<TransformType, GameObject>();

    #endregion
    public void registerRemoteUI(GameObject obj)
    {
        RemoteTransform transform;
        if (
            obj.TryGetComponent<RemoteTransform>(out transform))
        {
            transform.RemoteObjectEvent += ingameUIManger.UpdateRemoteTargetUI;
            Debug.Log("�̺�Ʈ �߰�");
        }
    }
    public void transformed(TransformType type,Action eventhandler=null)
{
     
        interactobject = null;
        #region Type ����
        if (CurrentType == type)
        return;
    CurrentType = type;
        #endregion
        CreateModelByCurrentType(eventhandler);
}
    void userestoredtype()
    {
        if (CurrentPower > 0)
        {
            transformed(retoretype);
        }
    }
    public float defromUpPosition;
 public   void Deform()
    {
        if(CurrentPlayer !=null)
            lastDirection = PlayerStat.instance.direction;        
        transformed(TransformType.Default);
        if (LastTransformPlace != null)
        {
            LastTransformPlace.transform.position = Playerprefab.transform.position;
            LastTransformPlace.gameObject.SetActive(true);
            LastTransformPlace = null;
            CurrentPlayer.transform.Translate(Vector3.up * defromUpPosition);
        }
        PlayerStat.instance.direction = lastDirection;

           
    }
    void CreateModelByCurrentType(Action eventhandler =null)
{
     
        if ((int)CurrentType < PlayerTransformList.Count)
    {
            #region �÷��̾� ������ ��ü
            Transform tf=null;
            if (Playerprefab != null)
            {
              
                tf = Playerprefab.transform;
                CurrentPlayer = null;
            }
           
            if(Playerprefab != null) 
            Playerprefab.SetActive(false);
            GameObject p;
            if (CreatedTransformlist.TryGetValue(CurrentType, out p))
            {
                p.gameObject.SetActive(true);
              
            }
            else
            {
                p = Instantiate(PlayerTransformList[CurrentType]);
                CreatedTransformlist.Add(CurrentType, p);
                if (CurrentType == TransformType.remoteform)
                {
                    registerRemoteUI(p);
                }
            }
            
            Playerprefab = p;
            #endregion
            #region ��ġ ����ȭ


            p.transform.position = tf.position;
            p.transform.rotation = tf.rotation;
            canthandle handle;
            EventHandle Ehandler;
            if (p.TryGetComponent<canthandle>(out handle))
            {
                CurrentPlayer = null;
            }
            else
            {
                CurrentPlayer = p.GetComponent<Player>();
            }
            if (p.TryGetComponent<EventHandle>(out Ehandler))
            {
               
                Ehandler.GetEvent(eventhandler);
            }
        
            #endregion
            if(formChange)
                CurrentPlayer.Humonoidanimator.Play("TransformEnd");
        }
        else
        Debug.Log("ListOutofRangeError");
}
    #endregion
    #region �÷��̾� �⺻ ����
    public float DeTransformtime = 2;
    float DeTransformtimer = 0;
   
    public void RegisterChange3DEvent(Action a)
    {
        Dimensionchangeevent += a;
    }

    private void Update()
    {
        if (inputTimer > 0)
        {
            inputTimer -= Time.deltaTime;
        }

        if (doubleUpInput || doubleDownInput)
            onAttack = false;
        else
            onAttack = true;

        if (interactobject != null)
            ingameUIManger.UpdateInteractUI(interactobject.gameObject);
        else
        {
            ingameUIManger. InteractTargetUI.SetActive(false);
        }
    }
    [Header("Ű �ι� �Է¿� ���� ó��")]
    public bool firstUpInput;
    public bool doubleUpInput;
    float inputTimer;
    public float inputTime;
    public bool firstDownInput;
    public bool doubleDownInput;
    [Header("�Ϲ� or ��ų üũ")]
    public bool onAttack;

    [HideInInspector]
    public bool DImensionChangeDisturb;
    event Action Dimensionchangeevent;
    event Action CAmeraChangeevent;
    event Action CorutineRegisterEvent;
    IEnumerator CameraRotateCorutine;
    public void registerCameraChangeAction(Action a)
    {
        CAmeraChangeevent += a;
    }
  public void registerCorutineRegisterEvent(Action CorutineRegister)
    {
        this.CorutineRegisterEvent += CorutineRegister;
    }
    public void RegisterCameraRotateCorutine(IEnumerator corutine)//�ܺο��� �޴� ��
    {
        CameraRotateCorutine = corutine;

    }
    IEnumerator InvokeDimensionEvent()
    {
        Dimensionchangeevent?.Invoke();
        yield return null;
    }

    bool Changing;
    IEnumerator ChangeDimension()
    {
        Changing = true;
        CorutineRegisterEvent?.Invoke();
        //3D�� �� ���� ī�޶� ���� �� �� �̺�Ʈ
        //2D�� �� ���� �ݴ�� �̺�Ʈ �� �� ī�޶�
        if (PlayerStat.instance.MoveState != PlayerMoveState.Trans3D)
        {//3D���� 2D��
            yield return StartCoroutine(InvokeDimensionEvent());

            //�̺�Ʈ ó��

            if (CameraRotateCorutine != null)
            {
                CAmeraChangeevent?.Invoke();
                yield return StartCoroutine(CameraRotateCorutine);
            }
            //ī�޶�ó��
        }
        else
        {

            if (CameraRotateCorutine != null)
            {
                CAmeraChangeevent?.Invoke();
                yield return StartCoroutine(CameraRotateCorutine);
            }
            //ī�޶�ó��
            yield return StartCoroutine(InvokeDimensionEvent());

            //�̺�Ʈ ó��

        }
    
        //�̺�Ʈ ��
        Changing = false;
       
        
    }
    void charactermove()
    {
        if (!CurrentPlayer.downAttack)
        {
            CurrentPlayer.Move();
        }
        if (Input.GetKeyDown(KeyCode.Space)&&CurrentPlayer.onGround&& !Changing&& !DImensionChangeDisturb)
        {
  
            StartCoroutine(ChangeDimension());
            //Dimensionchangeevent?.Invoke();
           
        }

        if (InteractTimer > 0)
            InteractTimer -= Time.deltaTime;


        if (interactobject != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && InteractTimer <= 0)
            {
                interactobject.Active(PlayerStat.instance.direction);
                interactobject = null;

                InteractTimer = PlayerStat.instance.InteractDelay;
            }
        }
        if (Input.GetKey(KeyCode.C))
        {
          
                Debug.Log("����Ű �Է� ��");
            CurrentPlayer.GetJumpBuffer();
            
            
        }
        else
        {
            CurrentPlayer.jumpLimitInput = false;
            /*if(CurrentPlayer.onGround || CurrentPlayer.isJump)
                CurrentPlayer.jumpLimitInput = false;*/
        }
        if (!Input.GetKey(KeyCode.C))
        {
            CurrentPlayer.jumphold();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CurrentPlayer. SwapAttackType();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (!firstUpInput && !doubleUpInput && inputTimer<=0)
            {
                firstUpInput = true;
                inputTimer = inputTime;                
            }

            if (!doubleUpInput)
            {
                if (!firstUpInput && inputTimer > 0)
                {
                    doubleUpInput = true;
                }
            }

            if (doubleUpInput)
            {
                switch (CurrentType)
                {
                    case TransformType.remoteform:
                        DeTransformtimer += Time.deltaTime;
                        if (DeTransformtimer > DeTransformtime)
                        {
                            DeTransformtimer = 0;
                            //Deform();
                        }
                        break;
                    default:
                        break;

                }
            }
        }
        else
        {
            firstUpInput = false;
            doubleUpInput = false;
            DeTransformtimer = 0;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (!firstDownInput && !doubleDownInput && inputTimer <= 0)
            {
                firstDownInput = true;
                inputTimer = inputTime;
            }

            if (!doubleDownInput)
            {
                if (!firstDownInput && inputTimer > 0)
                {
                    doubleDownInput = true;
                }
            }

            if (Input.GetKey(KeyCode.X) && !CurrentPlayer.onGround  && doubleDownInput/*&&
                PlayerInventory.instance.checkessesntialitem("item01")*/)
            {                         
                CurrentPlayer.DownAttack();
            }            
        }
        else
        {
            firstDownInput = false;
            doubleDownInput = false;
        }
        if (doubleUpInput && Input.GetKey(KeyCode.X)&&CurrentType!=TransformType.Default)
        {
            CurrentPlayer.Skill1();
         
                Skill1InputTimer = Skill1InputCheck;
        }    
        else
        if (Input.GetKey(KeyCode.X)&& Skill1InputTimer<=0/* &&
                PlayerInventory.instance.checkessesntialitem("item01")*/)
        {
            //CurrentPlayer.Attack();
            CurrentPlayer.attackBufferTimer = CurrentPlayer.attackBufferTimeMax;
        }
        if (Skill1InputTimer > 0)
            Skill1InputTimer -= Time.fixedDeltaTime;
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (CurrentType == TransformType.Default)
        //        userestoredtype();
        //    else
        //    
        //}


        //CurrentPlayer.Skill2();

    }
    #endregion


}


public enum TransformType { Default, remoteform,mouseform,transform1,testtransform}


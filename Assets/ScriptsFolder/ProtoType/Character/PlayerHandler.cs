using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;



public class PlayerHandler : MonoBehaviour
{
    public bool ladderCheck;
    public bool ladderInteract;
    public event Action PlayerDeathEvent;
    public event Action changedimentiosnsfxEvent;
    public void registerchangedimentiosnsfxEvent(Action a)
    {
        changedimentiosnsfxEvent += a;
    }
    public void InvokePlayerDeathEvent()
    {

        PlayerDeathEvent?.Invoke();
        PlayerDeathEvent = null;
    }
    float Skill1InputTimer;
    float Skill1InputCheck = 0.12f;

    public GameObject Fog;


    public bool formChange;
    #region 플레이어 변신관련 스탯
    public float CurrentPower;
    public float MaxPower = 60;
    public bool OnDeformField;
    public TransformType retoretype = TransformType.Default;
    public TransformPlace LastTransformPlace;
    bool jumprestrict;
    public IngameUIManager ingameUIManger;
    public void playerjumprestirct()
    {
        jumprestrict = true;
        CurrentPlayer.jumpBufferTimer = 0;
    }
    public void playerjumpaccept()
    {
        jumprestrict = false;
    }
    public Camera CurrentCamera;
    #endregion
    InteractiveObject interactobject;
    float InteractTimer;
    [Header("항시 무적")]
    [Tooltip("무적 on/off기능")] public bool AlwaysInvincible;
    [Tooltip("이거 체크해야 위에 것 가능")] public bool alwaysFuncActive;
    public void GetInteratObject(InteractiveObject i)
    {
        if (i.CanInteract)
            interactobject = i;
    }
    public void InitInteratObject(InteractiveObject i)
    {
        if (interactobject != null && i == interactobject)
            interactobject = null;
    }
    public InteractiveObject ReturnInteractObject()
    {
        return interactobject;
    }
    #region 플레이어 현재 위치,상태

    GameObject Playerprefab;

    public Player CurrentPlayer;// 행동 작업
    public void registerPlayer(GameObject o)//?
    {
        Playerprefab = o;
        CurrentPlayer = o.GetComponent<Player>();
    }
    public PlayerStat pStat; //스탯 분배 (스페셜어택)
    public direction lastDirection = direction.Right;
    #endregion
    public event Action transformevent;
    public event Action interactevent;
    public event Action keyinputevent;
    //변신
    public void registertransformevent(Action a)
    {
        transformevent += a;
    }
    //상호작용
    public void registerinteractevent(Action a)
    {
        interactevent += a;
    }

    public void registerkeyinputevent(Action a)
    {
        keyinputevent += a;
    }
    #region 싱글톤
    public static PlayerHandler instance;
    #endregion
    private void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
        }

        #endregion
        PlayerFormList p;
        if (TryGetComponent<PlayerFormList>(out p))
        {
            PlayerTransformList = p.playerformlist.Select((Value, index) => (Value, index))
                .ToDictionary(item => (TransformType)item.index, item => item.Value);
        }
        #region 캐릭터 초기화

        //CreateModelByCurrentType();
        #endregion

    }
    [Header("플레이어 낙사 높이?")]
    public float characterFallLimit;
    float CalculateCharacterFallLimit;
    public bool CharacterAutoFallEvent;
    event Action PlayerFallEvent;
    public void registerPlayerFallEvent(Action action)
    {
        PlayerFallEvent += action;
    }
    public void PlayerFallOut()
    {
        if (PlayerStat.instance.hp > 1)
        {
            Rigidbody rb = null;
            if (CurrentPlayer.TryGetComponent<Rigidbody>(out rb))
            {
                rb.velocity = Vector3.zero;
            }
            CurrentPlayer.transform.position = PlayerSpawnManager.Instance.CurrentCheckPoint.transform.position;
            //if(!AlwaysInvincible)
            CurrentPlayer.DamagedIgnoreInvincible(1);
            PlayerFallEvent?.Invoke();

        }
        else
        {
            CurrentPlayer.DamagedIgnoreInvincible(1);
            CurrentPlayer = null;
        }




    }
    private void FixedUpdate()
    {
        //if (CurrentType != TransformType.Default)
        //{
        //    CurrentPower -= Time.deltaTime;

        //}
        if (CurrentPlayer != null && CharacterAutoFallEvent && CurrentPlayer.transform.position.y < -Mathf.Abs(characterFallLimit) + -5)
            PlayerFallOut();


        if (alwaysFuncActive)
        {
            if (AlwaysInvincible)
                CurrentPlayer.onInvincible = true;
            else
                CurrentPlayer.onInvincible = false;
        }


        #region 캐릭터 조작
        if (KeySettingManager.instance == null)
        {
            if ((CurrentPlayer != null && !formChange) && !CantHandle)
                charactermove();
        }
        else
        {
            if ((CurrentPlayer != null && !formChange) && !CantHandle)
                KeysettingCharactermove();
        }
        #endregion
    }
    public bool CantHandle;
    //public float CantHandleTimer;
    #region 변신 시스템
    #region 변수
    public TransformType CurrentType = 0;
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
            Debug.Log("이벤트 추가");
        }
    }
    public void transformed(TransformType type, Action eventhandler = null)
    {

        interactobject = null;
        transformevent?.Invoke();
        #region Type 변경
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
    public void Deform()
    {
        if (CurrentPlayer != null)
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
    void CreateModelByCurrentType(Action eventhandler = null)
    {

        if ((int)CurrentType < PlayerTransformList.Count)
        {
            #region 플레이어 프리팹 교체
            Transform tf = null;
            if (Playerprefab != null)
            {

                tf = Playerprefab.transform;
                CurrentPlayer = null;
            }

            if (Playerprefab != null)
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
            #region 위치 동기화


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
            if (formChange)
                CurrentPlayer.Humonoidanimator.Play("TransformEnd");
        }
        else
            Debug.Log("ListOutofRangeError");
    }
    #endregion
    #region 플레이어 기본 조작
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
        {
            if (ladderCheck)
            {
                if (interactobject.GetComponent<Ladder>().resultPoint != null)
                {
                    ingameUIManger.UpdateInteractUI(interactobject.GetComponent<Ladder>().resultPoint.gameObject);
                }
            }
            else
                ingameUIManger.UpdateInteractUI(interactobject.gameObject);
        }
        else
        {
            ingameUIManger.InteractTargetUI.SetActive(false);
        }
    }
    [Header("키 두번 입력에 대한 처리")]
    public bool firstUpInput;
    public bool doubleUpInput;
    float inputTimer;
    public float inputTime;
    public bool firstDownInput;
    public bool doubleDownInput;
    [Header("일반 or 스킬 체크")]
    public bool onAttack;

    //[HideInInspector]
    public bool DImensionChangeDisturb;
    event Action Dimensionchangeevent;
    event Action CAmeraChangeevent;
    event Action CorutineRegisterEvent;
    IEnumerator CameraRotateCorutine;
    public void registerCameraChangeAction(Action a)
    {
        CAmeraChangeevent += a;
    }
    public void invokeccamerachangeaction()
    {
        CAmeraChangeevent?.Invoke();
    }
    public void registerCorutineRegisterEvent(Action CorutineRegister)
    {
        this.CorutineRegisterEvent += CorutineRegister;
    }
    public void RegisterCameraRotateCorutine(IEnumerator corutine)//외부에서 받는 중
    {
        CameraRotateCorutine = corutine;

    }
    IEnumerator InvokeDimensionEvent()
    {
        Dimensionchangeevent?.Invoke();
        yield return null;
    }

  public  bool Changing;
    IEnumerator ChangeDimension()
    {
        Changing = true;
        changedimentiosnsfxEvent.Invoke();
        CorutineRegisterEvent?.Invoke();
        //3D로 갈 때는 카메라 먼저 이 후 이벤트
        //2D로 갈 때는 반대로 이벤트 이 후 카메라
        CurrentCamera.GetComponent<CameraManager_Switching2D3D>()
         .trans3D = !CurrentCamera.GetComponent<CameraManager_Switching2D3D>()
         .trans3D;
        CurrentCamera.GetComponent<CameraManager_Switching2D3D>().UpdatePlayerMovestate();

        if (CurrentCamera.GetComponent<CameraManager_Switching2D3D>()
            .trans3D)
        {//3D에서 2D로

            yield return StartCoroutine(InvokeDimensionEvent());

            //이벤트 처리

            if (CameraRotateCorutine != null)
            {
                CAmeraChangeevent?.Invoke();
                yield return StartCoroutine(CameraRotateCorutine);
            }
            //카메라처리
        }
        else
        {



            if (CameraRotateCorutine != null)
            {
                yield return StartCoroutine(CameraRotateCorutine);
                CAmeraChangeevent?.Invoke();

            }
            yield return StartCoroutine(InvokeDimensionEvent());



        }
        PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
        //이벤트 완
        Changing = false;


    }
    void KeysettingCharactermove()
    {
        if (!CurrentPlayer.downAttack)
        {
            CurrentPlayer.Move();
        }
        if (Input.GetKeyDown(KeySettingManager.instance.DimensionChangeKeycode) && !Changing && !DImensionChangeDisturb)
        {

            StartCoroutine(ChangeDimension());
            //Dimensionchangeevent?.Invoke();

        }

        if (InteractTimer > 0)
            InteractTimer -= Time.deltaTime;


        if (interactobject != null)
        {
            if (Input.GetKeyDown(KeySettingManager.instance.InteractKeycode) && InteractTimer <= 0)
            {
                interactevent?.Invoke();
                interactobject.Active(PlayerStat.instance.direction);
                interactobject = null;

                InteractTimer = PlayerStat.instance.InteractDelay;
            }
        }
        if(!jumprestrict)
        if (CurrentPlayer.onInterarctive && (int)PlayerStat.instance.MoveState < 4)
        {

            if (Input.GetKeyDown(KeySettingManager.instance.jumpKeycode) && !Input.GetKey(KeyCode.DownArrow)
                 )
            {


                CurrentPlayer.GetJumpBuffer();


            }
            else
            {
                CurrentPlayer.jumpLimitInput = false;
                /*if(CurrentPlayer.onGround || CurrentPlayer.isJump)
                    CurrentPlayer.jumpLimitInput = false;*/
            }

        }
        else
        {
            if (Input.GetKeyDown(KeySettingManager.instance.jumpKeycode)
                  )
            {


                CurrentPlayer.GetJumpBuffer();


            }
            else
            {
                CurrentPlayer.jumpLimitInput = false;
                /*if(CurrentPlayer.onGround || CurrentPlayer.isJump)
                    CurrentPlayer.jumpLimitInput = false;*/
            }
        }
        if (!Input.GetKey(KeySettingManager.instance.jumpKeycode))
        {
            CurrentPlayer.jumphold();
        }


        if (!ladderInteract)
        {

            if (Input.GetKey(KeySettingManager.instance.DeformKeycode))
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



            if (Input.GetKey(KeySettingManager.instance.DownAttackKeycode) && !CurrentPlayer.onGround/*&&
                PlayerInventory.instance.checkessesntialitem("item01")*/)
            {
                CurrentPlayer.DownAttack();
            }

            if (doubleUpInput && Input.GetKeyDown(KeySettingManager.instance.SkillKeycode) && CurrentType != TransformType.Default)
            {
                CurrentPlayer.Skill1();

                Skill1InputTimer = Skill1InputCheck;
            }
            if (Input.GetKey(KeySettingManager.instance.AttackKeycode) && Skill1InputTimer <= 0/* &&
PlayerInventory.instance.checkessesntialitem("item01")*/)
            {
                if (CurrentPlayer.attackInputValue < 1 && !CurrentPlayer.attackLimitInput)
                    CurrentPlayer.attackBufferTimer = CurrentPlayer.attackBufferTimeMax;
            }
            if (Skill1InputTimer > 0)
                Skill1InputTimer -= Time.fixedDeltaTime;
        }
    }
    void charactermove()
    {
        if (!CurrentPlayer.downAttack)
        {
            CurrentPlayer.Move();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !Changing && !DImensionChangeDisturb)
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
        if (Input.GetKey(KeyCode.C) && !jumprestrict)
        {


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


        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (!firstUpInput && !doubleUpInput && inputTimer <= 0)
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

            if (Input.GetKey(KeyCode.X) && !CurrentPlayer.onGround && doubleDownInput/*&&
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
        if (doubleUpInput && Input.GetKeyDown(KeyCode.S) && CurrentType != TransformType.Default)
        {
            CurrentPlayer.Skill1();

            Skill1InputTimer = Skill1InputCheck;
        }
        if (Input.GetKey(KeyCode.X) && Skill1InputTimer <= 0/* &&
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


public enum TransformType { Default, remoteform, mouseform, transform1, testtransform }


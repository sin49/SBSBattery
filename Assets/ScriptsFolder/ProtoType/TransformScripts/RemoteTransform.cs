
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class RemoteTransform : Player
{
    public override void transformENdAnimation()
    {
     
        base.transformENdAnimation();
       
    }
  


    [Header("차징 스킬 변수")]
    //public float handleMaxTime; // 최대 차징 시간
    //float handletimer; // 차징 타이머 (시간이 증가하는 만큼 범위 증가)

    public SphereCollider handlerange; // 차징 범위 콜라이더



    //public event Action<GameObject> RemoteObjectEvent;

    public List<RemoteObject> remoteObj; // 탐지 범위에 저장될 상호작용 오브젝트 정보



 //public RemoteObject closestObject;
   [ HideInInspector]
    public bool IgnoreRemoteTrigger;
    GameObject activeEffectInstance;
    [Header("조종 오브젝트 감지 최소 범위")]
    public float minimumdistance;

    public bool Charging;

    [Header("빔 관련 변수")]
    public GameObject laserPrefab;
    public float Chargingmovespeed;
    public GameObject LIghtlaserPrefab;
    public GameObject MaxlaserPrefab;
    public GameObject laserEffect; // 빔 이펙트 오브젝트
    public GameObject dustEffect;

    public float lasermaxlifetime=5;
    public float laserminlifetime=0.6f;
    float laserlifetimeupspeed;
    public float lasermaxchargetime =2.5f;
    float laserchargettime;

    public GameObject HitPoint;
    //[Header("체인 라이트닝 변수")]
    //public List<GameObject> enemies; 
    //public GameObject chain; // 체인 오브젝트    
    //public float chainSearchRange; // 체인 오브젝트의 탐지 범위
    //[Header("체인 라이트닝 탐색 큐브 조정")]
    //public Vector3 searchCubeRange; // 플레이어 인지 범위를 Cube 사이즈로 설정
    //public Vector3 searchCubePos; // Cube 위치 조정
    //public bool onChain; // 스킬 사용 시 true변환


    RemoteObject ClosestObjectScript;
    public void GetClosestObjectIgnoreTrigger(RemoteObject obj)
    {
        IgnoreRemoteTrigger = true;
        PlayerHandler.instance.remoteobject = obj;
        ClosestObjectScript = PlayerHandler.instance.remoteobject;
    }
    public void RemoveClosesObject()
    {
        PlayerHandler.instance.remoteobject = null;
        ClosestObjectScript = null;
        //RemoteObjectEvent?.Invoke(null);

    }
    protected override void Awake()
    {
        base.Awake();
        //JumprayDistance = 0.07f;
        //InteractiveUprayDistance = 0.9f;
    }
    private void Update()
    {
        laserlifetimeupspeed = (lasermaxlifetime - laserminlifetime) / lasermaxchargetime;

        BaseBufferTimer();
   
        //for문 사용했으니 최적화 필요함
        if(!IgnoreRemoteTrigger)
            UpdateClosestRemoteObjectEffect();
        //if(ClosestObjectScript!=null&&!PlayerHandler.instance.calculateInteractobjectNRemoteObjectDistance())
        //    RemoteObjectEvent?.Invoke(ClosestObjectScript.HudTarget);
        //else
        //{
        //    RemoteObjectEvent?.Invoke(null);
        //}
        /*if (chargingBufferTimer > 0 && !Charging)
        {
            chargingBufferTimer -= Time.deltaTime;
        }*/
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (laserchargemode && laserchargettime < lasermaxchargetime)
        {
            laserchargettime += Time.deltaTime;
            if (laserchargettime > lasermaxchargetime)
                laserchargettime = lasermaxchargetime;
        }
        if (!Input.GetKey(KeySettingManager.instance.AttackKeycode) && laserchargemode)
        {
            laserchargemode = false;
            AttackKeyUp();
        }
            
    }
    private void OnDisable()
    {
        //closestObject = null;
        ClosestObjectScript = null;
        PlayerHandler.instance.remoteobject = null;
        //RemoteObjectEvent?.Invoke(null);
    }
    void UpdateClosestRemoteObjectEffect()
    {
        float closestdistance = float.MaxValue;
        RemoteObject newclosestobject = null;
        if (PlayerHandler.instance.remoteobject != null && !remoteObj.Contains(PlayerHandler.instance.remoteobject))
            PlayerHandler.instance.remoteobject = null;
        for(int n = 0; n < remoteObj.Count; n++)
        {
            if (remoteObj[n] == null)
            {
                remoteObj.RemoveAt(n);
                n--;
                continue;
            }
            if (!remoteObj[n].CanControl)
                continue;
            //if (!remoteObj[n].GetComponent<RemoteObject>().CanControl)
            //    continue;
            float distance = Vector3.Distance(transform.position, remoteObj[n].transform.position);
            if (distance < closestdistance)
            {
                closestdistance = distance;
                newclosestobject = remoteObj[n];
            }
        }
     
        if (closestdistance > minimumdistance)
        {
            PlayerHandler.instance.remoteobject = null;

            return;
        }
        if (newclosestobject != PlayerHandler.instance.remoteobject)
        {
            PlayerHandler.instance.remoteobject = newclosestobject;
            ClosestObjectScript = PlayerHandler.instance.remoteobject;

        }
       
    }
  
    public override void Skill1()
    {
        if (!PlayerHandler.instance.calculateInteractobjectNRemoteObjectDistance())
        {
            //Charging = true;
            if (PlayerHandler.instance.remoteobject != null)
            {
                base.Skill1();
                Humonoidanimator.Play("Charge");
                SoundPlayer.PlaySkillSound();
                ActiveRemoteObject();
            }
        }
    

        //if (!Input.GetKey(KeyCode.UpArrow) && Charging
        //    || !Input.GetKey(KeyCode.X) && Charging)
        //{
        /*if (handlerange.radius < handlediameterrangemin)
        {
            handlerange.radius = handlediameterrangemin;
        }*/
        //Charging = false;
        //chargingBufferTimer = chargingBufferTimeMax;
        //Humonoidanimator.SetBool("Charge", Charging);
        //if (timeScale < handlediameterrangemin)
        //{
        //    handlerange.transform.localScale = new Vector3(handlediameterrangemin, handlediameterrangemin, 0);
        //}
        //handlerange.gameObject.SetActive(true);
        //handlerange.gameObject.SetActive(Charging);

        //}
    }
    public void AttackKeyUp()
    {
       
                if ( !downAttack)
                {
  
            lasermaterialchangecorutine = null;
            attackBufferTimer = 0;
                    attackInputValue = 1;

                    dontAttack = true;
                    dontMoveTimer = PlayerStat.instance.attackDelay;
                    dontAttackTimer = PlayerStat.instance.initattackCoolTime*2;
                    AttackEvents();
                    Laser();
                }
          
     
    }
  
    public override void Move()
    {
        if(!laserchargemode)
        base.Move();
        else
        {
            PlayerStat.instance.MoveSpeedBonus += -Chargingmovespeed;
            base.Move();
            PlayerStat.instance.MoveSpeedBonus += Chargingmovespeed;
        }
    }
    public override void Jump()
    {
        //if (!laserchargemode)
            base.Jump();
    }
    public override void Attack()
    {

        if (PlayerHandler.instance.onAttack && attackInputValue < 1)
        {
        if (attackBufferTimer > 0 && !dontAttack)
        {
            laserchargemode = true;
                Humonoidanimator.Play("idle");
                lasermaterialchangecorutine = laserchargematerialchange();
                StartCoroutine(lasermaterialchangecorutine);
             
        }
     }
        
        
    }
  
    bool laserchargemode;
    public void Laser()
    {
        //if (PoolingManager.instance != null)
        //    PoolingManager.instance.GetPoolObject("Laser", firePoint);
        //else
     
        RemoteLaser laser_=null;
        float laserlifetime=laserminlifetime+laserlifetimeupspeed*
            laserchargettime;
        float laserdamage=1;
        if (laserchargettime < lasermaxchargetime / 4)
            laser_ = LIghtlaserPrefab.GetComponent<RemoteLaser>();
        else if (laserchargettime >= lasermaxchargetime)
        {
            laser_ = MaxlaserPrefab.GetComponent<RemoteLaser>();
            laserdamage = 3;
        }
        else
            laser_ = laserPrefab.GetComponent<RemoteLaser>();

        laser_.setLaser(laserlifetime, laserdamage);
        laserchargettime = 0;
            Instantiate(laser_.gameObject, firePoint.transform.position, firePoint.transform.rotation);
 
    }
    public Color LaserChargeColor;
    IEnumerator lasermaterialchangecorutine;
    IEnumerator laserchargematerialchange()
    {
        PlayerHandler.instance.onAttack = true;
        bool whitechecker = false;
        float blinkdelay = 0.1f;
        while (laserchargemode)
        {
            if (laserchargettime<lasermaxchargetime)
            {
                if (whitechecker)
                {
                    chrmat.SetColor("_Emissive_Color", new Vector4(0, 0, 0, 1));
                }
                else
                {
                    chrmat.SetColor("_Emissive_Color", LaserChargeColor);//emission 건들기
                }
                whitechecker = !whitechecker;
                blinkdelay = (lasermaxchargetime - laserchargettime) / 8;
                if (blinkdelay > 0.15f)
                    blinkdelay = 0.15f;
                yield return new WaitForSeconds(blinkdelay);
            }
            else
            {
                chrmat.SetColor("_Emissive_Color", LaserChargeColor);
                yield return null;
            }
            
        }

        chrmat.SetColor("_Emissive_Color", new Vector4(0, 0, 0, 1));
    }

    public override void Damaged(float damage)
    {
        
        base.Damaged(damage);
        if(!onInvincible)
        {
            laserchargemode = false;
            laserchargettime = 0;
          
            lasermaterialchangecorutine = null;
        }
        PlayerHandler.instance.onAttack = false;
    }
    IEnumerator LaserAttack()
    {

        if (PoolingManager.instance != null)
            PoolingManager.instance.GetPoolObject("Laser", firePoint);
        else
            Instantiate(laserPrefab, HitPoint.transform.position, HitPoint.transform.rotation);
        yield return new WaitForSeconds(PlayerStat.instance.attackDelay*2);

        canAttack = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(this.transform.position, minimumdistance);
    }
   
    #region 오버랩스피어 시도

    #endregion
    public GameObject ACtiveGameObject;
    public void SearchRemoteObjectList()
    {

    }
    public void ActiveRemoteObject()
    {

        if (PlayerHandler.instance.remoteobject != null)
        {
            
            PlayerHandler.instance.remoteobject.Active();

            //closestObject = null;
            if (!PlayerHandler.instance.remoteobject.CanControl)
            {
                PlayerHandler.instance.remoteobject = null;
                ClosestObjectScript = null;
            }
        }

    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class RemoteTransform : Player
{
    public override void transformENdAnimation()
    {
     
        base.transformENdAnimation();
       
    }
  


    [Header("��¡ ��ų ����")]
    //public float handleMaxTime; // �ִ� ��¡ �ð�
    //float handletimer; // ��¡ Ÿ�̸� (�ð��� �����ϴ� ��ŭ ���� ����)

    public SphereCollider handlerange; // ��¡ ���� �ݶ��̴�



    public event Action<GameObject> RemoteObjectEvent;

    public List<RemoteObject> remoteObj; // Ž�� ������ ����� ��ȣ�ۿ� ������Ʈ ����



 public RemoteObject closestObject;
   [ HideInInspector]
    public bool IgnoreRemoteTrigger;
    GameObject activeEffectInstance;
    [Header("���� ������Ʈ ���� �ּ� ����")]
    public float minimumdistance;

    public bool Charging;

    [Header("�� ���� ����")]
    public GameObject laserPrefab; // �� ��ų ������
    public GameObject laserEffect; // �� ����Ʈ ������Ʈ
    public GameObject HitPoint;
    //[Header("ü�� ����Ʈ�� ����")]
    //public List<GameObject> enemies; 
    //public GameObject chain; // ü�� ������Ʈ    
    //public float chainSearchRange; // ü�� ������Ʈ�� Ž�� ����
    //[Header("ü�� ����Ʈ�� Ž�� ť�� ����")]
    //public Vector3 searchCubeRange; // �÷��̾� ���� ������ Cube ������� ����
    //public Vector3 searchCubePos; // Cube ��ġ ����
    //public bool onChain; // ��ų ��� �� true��ȯ


    RemoteObject ClosestObjectScript;
    public void GetClosestObjectIgnoreTrigger(RemoteObject obj)
    {
        IgnoreRemoteTrigger = true;
        closestObject = obj;
        ClosestObjectScript = closestObject.GetComponent<RemoteObject>();
    }
    public void RemoveClosesObject()
    {
        closestObject = null;
        ClosestObjectScript = null;
        RemoteObjectEvent?.Invoke(null);

    }
    protected override void Awake()
    {
        base.Awake();
        //JumprayDistance = 0.07f;
        //InteractiveUprayDistance = 0.9f;
    }
    private void Update()
    {
        BaseBufferTimer();
   
        //for�� ��������� ����ȭ �ʿ���
        if(!IgnoreRemoteTrigger)
            UpdateClosestRemoteObjectEffect();
        if(ClosestObjectScript!=null)
            RemoteObjectEvent?.Invoke(ClosestObjectScript.HudTarget);
        else
        {
            RemoteObjectEvent?.Invoke(null);
        }
        /*if (chargingBufferTimer > 0 && !Charging)
        {
            chargingBufferTimer -= Time.deltaTime;
        }*/
    }
    private void OnDisable()
    {
        closestObject = null;
        ClosestObjectScript = null;
        RemoteObjectEvent?.Invoke(null);
    }
    void UpdateClosestRemoteObjectEffect()
    {
        float closestdistance = float.MaxValue;
        RemoteObject newclosestobject = null;
        if (closestObject != null && !remoteObj.Contains(closestObject))
            closestObject = null;
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
            closestObject = null;

            return;
        }
        if (newclosestobject != closestObject)
        {
            closestObject = newclosestobject;
            ClosestObjectScript = closestObject.GetComponent<RemoteObject>();

        }
       
    }
    public override void Skill1()
    {
     
        //Charging = true;
        if (closestObject != null)
            {
            base.Skill1();
            Humonoidanimator.Play("Charge");
            SoundPlayer.PlaySkillSound();
                ActiveRemoteObject();
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

    public override void Attack()
    {
        /*if (attackBufferTimer > 0 && canAttack)
        {
   
            AttackEvents();
            StartCoroutine(LaserAttack());
        }*/
        if (PlayerHandler.instance.onAttack && attackInputValue < 1)
        {
            if (attackBufferTimer > 0 && !dontAttack)
            {
                if (PlayerStat.instance.attackType == AttackType.melee && !downAttack)
                {
                    attackBufferTimer = 0;
                    attackInputValue = 1;

                    dontAttack = true;
                    dontMoveTimer = PlayerStat.instance.attackDelay;
                    dontAttackTimer = PlayerStat.instance.initattackCoolTime;
                    AttackEvents();
                    Laser();
                }
            }
        }
    }
    public void Laser()
    {
        if (PoolingManager.instance != null)
            PoolingManager.instance.GetPoolObject("Laser", firePoint);
        else
            Instantiate(laserPrefab, HitPoint.transform.position, HitPoint.transform.rotation);
    }
    IEnumerator LaserAttack()
    {

        if (PoolingManager.instance != null)
            PoolingManager.instance.GetPoolObject("Laser", firePoint);
        else
            Instantiate(laserPrefab, HitPoint.transform.position, HitPoint.transform.rotation);
        yield return new WaitForSeconds(PlayerStat.instance.attackDelay);

        canAttack = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(this.transform.position, minimumdistance);
    }

    #region ���������Ǿ� �õ�

    #endregion
    public GameObject ACtiveGameObject;
    public void SearchRemoteObjectList()
    {

    }
    public void ActiveRemoteObject()
    {

        if (closestObject != null)
        {
            RemoteObject o = closestObject.GetComponent<RemoteObject>();
            o.Active();

            //closestObject = null;
            if (!o.CanControl)
            {
                closestObject = null;
                ClosestObjectScript = null;
            }
        }

    }
}

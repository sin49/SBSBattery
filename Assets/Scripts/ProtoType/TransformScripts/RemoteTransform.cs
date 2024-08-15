
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class RemoteTransform : Player
{
    [Header("��¡ ��ų ����")]
    //public float handleMaxTime; // �ִ� ��¡ �ð�
    //float handletimer; // ��¡ Ÿ�̸� (�ð��� �����ϴ� ��ŭ ���� ����)

    public SphereCollider handlerange; // ��¡ ���� �ݶ��̴�



    public event Action<GameObject> RemoteObjectEvent;

    public List<GameObject> remoteObj; // Ž�� ������ ����� ��ȣ�ۿ� ������Ʈ ����



 public   GameObject closestObject;
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
    protected override void Awake()
    {
        base.Awake();
        JumprayDistance = 0.07f;
        InteractiveUprayDistance = 0.9f;
    }
    private void Update()
    {
        BaseBufferTimer();
   
        //for�� ��������� ����ȭ �ʿ���
        UpdateClosestRemoteObjectEffect();
        if(ClosestObjectScript!=null)
        RemoteObjectEvent?.Invoke(ClosestObjectScript.HudTarget);
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
        GameObject newclosestobject = null;
       
        for(int n = 0; n < remoteObj.Count; n++)
        {
            if (remoteObj[n] == null)
            {
                remoteObj.RemoveAt(n);
                n--;
                continue;
            }
            float distance = Vector3.Distance(transform.position, remoteObj[n].transform.position);
            if (distance < closestdistance)
            {
                closestdistance = distance;
                newclosestobject = remoteObj[n];
            }
        }
        foreach (var obj in remoteObj)
        {
            if(obj==null)
            {
                remoteObj.Remove(obj);
                continue;
            }
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestdistance)
            {
                closestdistance = distance;
                newclosestobject = obj;
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
                Humonoidanimator.Play("Charge");
                ActiveRemoteObject();
        }
        else
        {
            base.Skill1();
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
        if (attackBufferTimer > 0 && canAttack)
        {
   
            AttackEvents();
            StartCoroutine(LaserAttack());
        }
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
            closestObject.GetComponent<RemoteObject>().Active();
       
    }
}

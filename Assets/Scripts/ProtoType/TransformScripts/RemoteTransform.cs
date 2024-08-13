
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
    public float handlediameterrangemax; // ��¡ �ִ� ����
    public float handlediameterrangemin; // ��¡ �ּ� ����
    public SphereCollider handlerange; // ��¡ ���� �ݶ��̴�
    public float chargingBufferTimer;
    public float chargingBufferTimeMax;


    public event Action<GameObject> RemoteObjectEvent;

    public List<GameObject> remoteObj; // Ž�� ������ ����� ��ȣ�ۿ� ������Ʈ ����



 public   GameObject closestObject;
    GameObject activeEffectInstance;
    public float minimumdistance;

    public bool Charging;

    [Header("�� ���� ����")]
    public GameObject laserPrefab; // �� ��ų ������
    public GameObject laserEffect; // �� ����Ʈ ������Ʈ

    //[Header("ü�� ����Ʈ�� ����")]
    //public List<GameObject> enemies; 
    //public GameObject chain; // ü�� ������Ʈ    
    //public float chainSearchRange; // ü�� ������Ʈ�� Ž�� ����
    //[Header("ü�� ����Ʈ�� Ž�� ť�� ����")]
    //public Vector3 searchCubeRange; // �÷��̾� ���� ������ Cube ������� ����
    //public Vector3 searchCubePos; // Cube ��ġ ����
    //public bool onChain; // ��ų ��� �� true��ȯ



    private void Awake()
    {
        //handlerange.
        handlerange = transform.Find("SKillChargeRadius").GetComponent<SphereCollider>();

    }

    private void Update()
    {
        BaseBufferTimer();
   
        //for�� ��������� ����ȭ �ʿ���
        UpdateClosestRemoteObjectEffect();

        RemoteObjectEvent?.Invoke(closestObject);
        /*if (chargingBufferTimer > 0 && !Charging)
        {
            chargingBufferTimer -= Time.deltaTime;
        }*/
    }
    void UpdateClosestRemoteObjectEffect()
    {
        float closestdistance = float.MaxValue;
        GameObject newclosestobject = null;
       
        foreach (var obj in remoteObj)
        {
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


        }
       
    }
    public override void Skill1()
    {
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.X))
        {
            //Charging = true;
            if (closestObject != null)
            {
                Humonoidanimator.Play("Charge");
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

    public override void Attack()
    {
        if (attackBufferTimer > 0 && canAttack)
        {
            canAttack = false;

            StartCoroutine(LaserAttack());
        }
    }

    IEnumerator LaserAttack()
    {
        Humonoidanimator.Play("Attack");
        if (PoolingManager.instance != null)
            PoolingManager.instance.GetPoolObject("Laser", firePoint);
        else
            Instantiate(laserPrefab, this.gameObject.transform.position, this.transform.rotation);
        yield return new WaitForSeconds(PlayerStat.instance.attackDelay);

        canAttack = true;
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

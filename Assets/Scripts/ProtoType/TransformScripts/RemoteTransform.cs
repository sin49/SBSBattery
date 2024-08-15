
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class RemoteTransform : Player
{
    [Header("차징 스킬 변수")]
    //public float handleMaxTime; // 최대 차징 시간
    //float handletimer; // 차징 타이머 (시간이 증가하는 만큼 범위 증가)

    public SphereCollider handlerange; // 차징 범위 콜라이더



    public event Action<GameObject> RemoteObjectEvent;

    public List<GameObject> remoteObj; // 탐지 범위에 저장될 상호작용 오브젝트 정보



 public   GameObject closestObject;
    GameObject activeEffectInstance;
    [Header("조종 오브젝트 감지 최소 범위")]
    public float minimumdistance;

    public bool Charging;

    [Header("빔 관련 변수")]
    public GameObject laserPrefab; // 빔 스킬 프리팹
    public GameObject laserEffect; // 빔 이펙트 오브젝트
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
    protected override void Awake()
    {
        base.Awake();
        JumprayDistance = 0.07f;
        InteractiveUprayDistance = 0.9f;
    }
    private void Update()
    {
        BaseBufferTimer();
   
        //for문 사용했으니 최적화 필요함
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

    #region 오버랩스피어 시도

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

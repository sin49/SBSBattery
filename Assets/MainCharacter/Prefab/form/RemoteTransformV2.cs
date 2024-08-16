using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.SocialPlatforms;

public class RemoteTransformV2 : Player
{
    [Header("차징 스킬 변수")]
    //public float handleMaxTime; // 최대 차징 시간
    //float handletimer; // 차징 타이머 (시간이 증가하는 만큼 범위 증가)

    public SphereCollider handlerange; // 차징 범위 콜라이더
    public float chargingBufferTimer;
    public float chargingBufferTimeMax;


    public List<GameObject> remoteObj; // 탐지 범위에 저장될 상호작용 오브젝트 정보

    public GameObject RemoteObjectEffect;

    GameObject closestObject;
    GameObject activeEffectInstance;
    public float minimumdistance;

    public bool Charging;

    [Header("빔 관련 변수")]
    public GameObject laserPrefab; // 빔 스킬 프리팹
    public GameObject HitPoint;
    public GameObject laserEffect; // 빔 이펙트 오브젝트

    //[Header("체인 라이트닝 변수")]
    //public List<GameObject> enemies; 
    //public GameObject chain; // 체인 오브젝트    
    //public float chainSearchRange; // 체인 오브젝트의 탐지 범위
    //[Header("체인 라이트닝 탐색 큐브 조정")]
    //public Vector3 searchCubeRange; // 플레이어 인지 범위를 Cube 사이즈로 설정
    //public Vector3 searchCubePos; // Cube 위치 조정
    //public bool onChain; // 스킬 사용 시 true변환




   
    private void Update()
    {
        BaseBufferTimer();
        if (closestObject == null)
            RemoteObjectEffect.SetActive(false);
        else
            RemoteObjectEffect.transform.position = closestObject.transform.position;
        //for문 사용했으니 최적화 필요함
        UpdateClosestRemoteObjectEffect();
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
            RemoteObjectEffect.SetActive(false);
            return;
        }
        if (newclosestobject != closestObject)
        {
            closestObject = newclosestobject;
            RemoteObjectEffect.SetActive(true);

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
            Instantiate(laserPrefab, HitPoint.transform.position, HitPoint.transform.rotation);
        yield return new WaitForSeconds(PlayerStat.instance.attackDelay);

        canAttack = true;
    }

    /*public override void Skill2()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Instantiate(chain, transform.position, transform.rotation);
        }
    }*/

    //IEnumerator ChainLightning()
    //{
    //    yield return null;
    /*for (int i = enemies.Count - 1; i >= 0; i--)
    {
        yield return new WaitForSeconds(0.1f);

        Instantiate(chain, enemies[i].transform.position, Quaternion.identity);
    }
    //enemies.Clear();
    onChain = false;*/
    //}

    #region 오버랩스피어 시도
    /*public void SearchRemoteObject()
    {
        Collider[] searchColliders = Physics.OverlapSphere(transform.position, searchRange);

        Debug.Log($"콜라이더 탐색됨 >> {searchColliders.Length}, {searchColliders[0].gameObject}");

        for (int i = 0; i < searchColliders.Length; i++)
        {
            if (searchColliders[i].CompareTag("GameController"))
            {
                Debug.Log("리모컨으로 컨트롤 가능한 오브젝트 탐지함");
                //SaveRemoteObject(searchColliders[i].gameObject);
            }
        }
    }*/

    /*public void SaveRemoteObject(GameObject remote)
    {
        Debug.Log($"가져온 오브젝트의 정보:{remote}");
        for (int i = 0; i < remoteObj.Count; i++)
        {            
            if (remote != remoteObj[i])
            {
                continue;
            }
            else
            {
                remoteObj.Add(remote);
            }
        }
    }*/

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }*/
    #endregion
    public GameObject ACtiveGameObject;
    public void SearchRemoteObjectList()
    {

    }
    public void ActiveRemoteObject()
    {
        if (closestObject != null)
            closestObject.GetComponent<RemoteObject>().Active();
        //for (int i = 0; i < remoteObj.Count; i++)
        //{
        //    remoteObj[i].GetComponent<RemoteObject>().Active();
        //}



        //handletimer = 0;
        //timeScale = 0;

    }

    //IEnumerator ElectricPower()
    //{
    //    yield return new WaitForSeconds(1f);


    //}
}
//[Header("차징 스킬 변수")]
//public float handleMaxTime; // 최대 차징 시간
//float handletimer; // 차징 타이머 (시간이 증가하는 만큼 범위 증가)
//public float handlediameterrangemax; // 차징 최대 범위
//public float handlediameterrangemin; // 차징 최소 범위
//public SphereCollider handlerange; // 차징 범위 콜라이더
//public float chargingBufferTimer;
//public float chargingBufferTimeMax;

//public RectTransform electricCharge;
//public float holdSpeed; // 충전 속도
//public List<GameObject> remoteObj; // 탐지 범위에 저장될 상호작용 오브젝트 정보
//public float timeScale; // 차징 범위 증가 받을 변수
//public float chargeSpeed; // 차징 속도 변수

//public bool Charging;

//[Header("빔 관련 변수")]
//public GameObject laserPrefab; // 빔 스킬 프리팹
//public GameObject laserEffect; // 빔 이펙트 오브젝트

//[Header("체인 라이트닝 변수")]
//public List<GameObject> enemies;
//public GameObject chain; // 체인 오브젝트    
//public float chainSearchRange; // 체인 오브젝트의 탐지 범위
//[Header("체인 라이트닝 탐색 큐브 조정")]
//public Vector3 searchCubeRange; // 플레이어 인지 범위를 Cube 사이즈로 설정
//public Vector3 searchCubePos; // Cube 위치 조정
//public bool onChain; // 스킬 사용 시 true변환



//private void Awake()
//{
//    //handlerange.
//    handlerange = transform.Find("SKillChargeRadius").GetComponent<SphereCollider>();
//}

//private void Update()
//{
//    BaseBufferTimer();

//    /*if (chargingBufferTimer > 0 && !Charging)
//    {
//        chargingBufferTimer -= Time.deltaTime;
//    }*/
//}

//public override void Skill1()
//{
//    Charging = true;
//    /*if (!handlerange.gameObject.activeSelf)
//    {
//        handlerange.gameObject.SetActive(true);
//    }*/
//    Humonoidanimator.SetBool("Charge", Charging);
//    if (!handlerange.gameObject.activeSelf)
//    {
//        //handlerange.gameObject.SetActive(Charging);
//        //handlerange.enabled = Charging;
//        Humonoidanimator.Play("Charge");
//    }

//    handletimer += Time.deltaTime;
//    if (handletimer >= handleMaxTime)
//    {
//        Debug.Log("충전량 최대치입니다");
//        handlerange.gameObject.SetActive(true);
//    }
//    else
//    {
//        /*if (handlerange.radius > handlediameterrangemax)
//        {
//            handlerange.radius = handlediameterrangemax;
//        }
//        else
//        {
//            handlerange.radius += Time.deltaTime;
//        }*/
//        /*if (timeScale > handlediameterrangemax)
//        {
//            handlerange.transform.localScale = new Vector3(handlediameterrangemax, handlediameterrangemax, 0);
//        }
//        else
//        {
//            timeScale += Time.deltaTime;
//            handlerange.transform.localScale = new Vector3(chargeSpeed * timeScale, chargeSpeed * timeScale, 0);
//        }*/
//    }

//    /*if (handlerange.radius < handlediameterrangemin)
//        {
//            handlerange.radius = handlediameterrangemin;
//        }*/
//    if (!PlayerHandler.instance.doubleUpInput || !Input.GetKey(KeyCode.X))
//    {
//        Charging = false;
//        chargingBufferTimer = chargingBufferTimeMax;
//        Humonoidanimator.SetBool("Charge", Charging);
//        if (timeScale < handlediameterrangemin)
//        {
//            handlerange.transform.localScale = new Vector3(handlediameterrangemin, handlediameterrangemin, 0);
//        }
//        //handlerange.gameObject.SetActive(true);
//        handlerange.gameObject.SetActive(Charging);
//        ActiveRemoteObject();
//    }

//    /*if (!Input.GetKey(KeyCode.UpArrow) && Charging
//        || !Input.GetKey(KeyCode.X) && Charging)
//    {            
//    }*/
//}

//public override void Attack()
//{
//    if (attackBufferTimer > 0 && canAttack)
//    {
//        canAttack = false;

//        StartCoroutine(LaserAttack());
//    }
//}

//IEnumerator LaserAttack()
//{
//    Humonoidanimator.Play("Attack");
//    if (PoolingManager.instance != null)
//        PoolingManager.instance.GetPoolObject("Laser", firePoint);
//    else
//        Instantiate(laserPrefab, this.gameObject.transform.position, this.transform.rotation);
//    yield return new WaitForSeconds(PlayerStat.instance.attackDelay);

//    canAttack = true;
//}

///*public override void Skill2()
//{
//    if (Input.GetKeyDown(KeyCode.D))
//    {
//        Instantiate(chain, transform.position, transform.rotation);
//    }
//}*/

//IEnumerator ChainLightning()
//{
//    yield return null;
//    /*for (int i = enemies.Count - 1; i >= 0; i--)
//    {
//        yield return new WaitForSeconds(0.1f);

//        Instantiate(chain, enemies[i].transform.position, Quaternion.identity);
//    }
//    //enemies.Clear();
//    onChain = false;*/
//}

//#region 오버랩스피어 시도
///*public void SearchRemoteObject()
//{
//    Collider[] searchColliders = Physics.OverlapSphere(transform.position, searchRange);

//    Debug.Log($"콜라이더 탐색됨 >> {searchColliders.Length}, {searchColliders[0].gameObject}");

//    for (int i = 0; i < searchColliders.Length; i++)
//    {
//        if (searchColliders[i].CompareTag("GameController"))
//        {
//            Debug.Log("리모컨으로 컨트롤 가능한 오브젝트 탐지함");
//            //SaveRemoteObject(searchColliders[i].gameObject);
//        }
//    }
//}*/

///*public void SaveRemoteObject(GameObject remote)
//{
//    Debug.Log($"가져온 오브젝트의 정보:{remote}");
//    for (int i = 0; i < remoteObj.Count; i++)
//    {            
//        if (remote != remoteObj[i])
//        {
//            continue;
//        }
//        else
//        {
//            remoteObj.Add(remote);
//        }
//    }
//}*/

///*private void OnDrawGizmos()
//{
//    Gizmos.color = Color.blue;
//    Gizmos.DrawWireSphere(transform.position, searchRange);
//}*/
//#endregion

//public void ActiveRemoteObject()
//{
//    for (int i = 0; i < remoteObj.Count; i++)
//    {
//        remoteObj[i].GetComponent<RemoteObject>().Active();
//    }

//    remoteObj.Clear();

//    handletimer = 0;
//    timeScale = 0;

//}

//IEnumerator ElectricPower()
//{
//    yield return new WaitForSeconds(1f);


//}
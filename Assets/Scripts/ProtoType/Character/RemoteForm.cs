using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteForm : Player
{
    //public float searchRange;

    [Header("차징 스킬 변수")]
    public float handleMaxTime; // 최대 차징 시간
    float handletimer; // 차징 타이머 (시간이 증가하는 만큼 범위 증가)
    public float handlediameterrangemax; // 차징 최대 범위
    public float handlediameterrangemin; // 차징 최소 범위
    public SphereCollider handlerange; // 차징 범위 콜라이더

    public RectTransform electricCharge;
    public float holdSpeed; // 충전 속도
    public List<GameObject> remoteObj; // 탐지 범위에 저장될 상호작용 오브젝트 정보
    public float timeScale; // 차징 범위 증가 받을 변수
    public float chargeSpeed; // 차징 속도 변수

    public bool Charging;

    [Header("빔 관련 변수")]
    public GameObject laserPrefab; // 빔 스킬 프리팹
    
    private void Awake()
    {
        //handlerange.
        handlerange = transform.Find("Sphere").GetComponent<SphereCollider>();
    }

    /*private void Update()
    {
        SearchRemoteObject();
    }*/

    public override void Skill1()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (!handlerange.gameObject.activeSelf)
            {
                handlerange.gameObject.SetActive(true);
            }

            Charging = true;
            handletimer += Time.deltaTime;
            if (handletimer >= handleMaxTime)
            {
                Debug.Log("충전량 최대치입니다");
            }
            else
            {
                /*if (handlerange.radius > handlediameterrangemax)
                {
                    handlerange.radius = handlediameterrangemax;
                }
                else
                {
                    handlerange.radius += Time.deltaTime;
                }*/
                if (timeScale > handlediameterrangemax)
                {
                    handlerange.transform.localScale = new Vector3(handlediameterrangemax, handlediameterrangemax, 0);
                }
                else
                {
                    timeScale += Time.deltaTime;
                    handlerange.transform.localScale = new Vector3(chargeSpeed * timeScale, chargeSpeed * timeScale, 0);
                }
            }
        }

        if(!Input.GetKey(KeyCode.S) && Charging)
        {
            /*if (handlerange.radius < handlediameterrangemin)
            {
                handlerange.radius = handlediameterrangemin;
            }*/
            Charging = false;

            if (timeScale < handlediameterrangemin)
            {
                handlerange.transform.localScale = new Vector3(handlediameterrangemin, handlediameterrangemin, 0);
            }
            //handlerange.gameObject.SetActive(true);
            handlerange.enabled = true;
            ActiveRemoteObject();
        }
    }

    public override void Skill2()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Instantiate(laserPrefab, firePoint.position, transform.rotation);
        }
    }

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

    public void ActiveRemoteObject()
    {
        for (int i = 0; i < remoteObj.Count; i++)
        {
            remoteObj[i].GetComponent<RemoteObject>().Active();
        }

        remoteObj.Clear();        
        handlerange.transform.localScale = new Vector3(0, 0, 0);
        handletimer = 0;
        timeScale = 0;
        handlerange.enabled = false;
    }

    IEnumerator ElectricPower()
    {
        yield return new WaitForSeconds(1f);


    }
}

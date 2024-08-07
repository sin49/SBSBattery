using System.Collections;
using UnityEngine;

public class HandleSpotlight : MonoBehaviour
{
    public SpotLightObject lightObj; //빛 오브젝트       
    
    public Transform bossField; // 보스 스테이지 바닥    
    public Vector3 originPos; // 보스의 손 최초 위치
                              // >> 나중에 스포트라이트 손 이동 있을 때를 대비하기위해 미리 선언
    [Header("타겟이 이동할 목표지점")]
    public Transform rightEndSpot; // 오른쪽 목표지점
    public Transform leftEndSpot; // 왼쪽 목표지점
    public Transform targetSpot; // 결정된 목표지점
    [Header("라이트, 손 회전이 따라갈 타겟정보")]
    public GameObject moveTarget; // 목표지점으로 이동하는 오브젝트(빛과 손의 회전이 따라감)
    public float targetSpeed; // moveTarget의 속도
    [Header("몬스터에 대한 카운트, monsterCountMax가 생성될 수 결정")]
    public int monsterCount; // TV몬스터의 목표 도착 카운트
    public int monsterActiveCount; // 생성되어있는 몬스터 기준으로 활성화 카운트
    public int monsterCountMax; // 최대 카운트 수 >> 몬스터의 수, 카운트 최대치 결정
    [Header("몬스터, 몬스터 담아두게될 공간")]
    public GameObject tvMonster; // 몬스터 프리팹
    public Transform tvMonsterGroup; // 생성되는 몬스터 담는 공간
    [Header("몬스터 스폰 쿨타임, 반복될 타이머")]
    public float InvokeStartTime; // 스폰 시작 쿨타임
    public float InvokeRate; // 반복 타이머

    private void Awake()
    {
        originPos = transform.position;
        lightObj.transform.position = 
            new(lightObj.transform.position.x, lightObj.transform.position.y, bossField.position.z);
        moveTarget.transform.position = bossField.position;
    }

    private void Start()
    {
        SpotLightShow();
    }
    
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpotLightShow();
        }
    }*/

    public void SpotLightShow()
    {        
        StartCoroutine(SpotLightMove());        
    }    

    IEnumerator SpotLightMove()
    {
        targetSpot = DecideTargetSpot();
        InvokeRepeating("MonsterSpawn", InvokeStartTime, InvokeRate);
        lightObj.HandleSpotLight(this);

        //lightObj.target = moveTarget.transform;
        //lightObj.tracking = true;
        

        while (Vector3.Distance(moveTarget.transform.position, targetSpot.position) > 1f)
        {
            transform.LookAt(moveTarget.transform);
            moveTarget.transform.LookAt(targetSpot.transform);
            moveTarget.transform.Translate(moveTarget.transform.forward * targetSpeed * Time.deltaTime, Space.World);

            yield return null;
        }
        //목표에 도달했을 때
        //라이트 오브젝트가 떨리는 현상을 멈추기 위해 tracking값을 false로 변환
        //(지금 추적하는 게 SpotLightObject스크립트의 Update함수에서 실행중임)        
        lightObj.tracking = false;

    }
    //목표지점을 결정하여 트랜스폼 반환
    public Transform DecideTargetSpot()
    {
        float rightSpot = Vector3.Distance(transform.position, rightEndSpot.position);
        float leftSpot= Vector3.Distance(transform.position, leftEndSpot.position);

        if (rightSpot > leftSpot)
            return rightEndSpot;
        else
            return leftEndSpot;
    }
    //몬스터 스폰 함수
    public void MonsterSpawn()
    {
        Debug.Log("인보크 리피팅 시작");
        if (tvMonsterGroup.childCount < monsterCountMax)
        {            
            GameObject monster = Instantiate(tvMonster, SpawnPosition().position, Quaternion.identity);
            monster.transform.SetParent(tvMonsterGroup);
            monster.GetComponent<TvMonsterBossField>().SetHandle(this);

            if (tvMonsterGroup.childCount == monsterCountMax)
            {
                CancelInvoke("MonsterSpawn");
            }
        }        
        else
        {
            for(int i=0; i<monsterCountMax; i++)
            {
                if (tvMonsterGroup.GetChild(i).gameObject.activeSelf)
                {
                    continue;
                }
                else
                {
                    tvMonsterGroup.GetChild(i).GetComponent<TvMonsterBossField>().SetHandle(this);
                    tvMonsterGroup.GetChild(i).gameObject.SetActive(true);
                    monsterActiveCount++;
                    if (monsterActiveCount == monsterCountMax)
                        CancelInvoke("MonsterSpawn");
                    return;
                }
            }            
        }

        
    }
    //몬스터의 스폰위치 결정하여 반환
    public Transform SpawnPosition()
    {
        if (targetSpot == rightEndSpot)
            return leftEndSpot;
        else
            return rightEndSpot;
    }
    //몬스터가 목표 지점 근처에 도달했을 때 받는 카운트 함수
    //조건문 만족 시, 라이트 오브젝트의 로테이션값을 기존으로 초기화
    public void CheckMonsterCount()
    {
        if (monsterCount == monsterCountMax)
        {
            transform.position = originPos;
            lightObj.InitRotation();
            monsterCount = 0;
            monsterActiveCount = 0;
        }        
    }    
}

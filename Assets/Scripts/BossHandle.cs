using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandle : MonoBehaviour
{
    public bool Bossawake;
    Vector3 OnePosition;

    public event Action ActionEnd;


    public Transform SweaperStartTransform;
    public Transform SweaperEndTransform;
    public float SweaperStartMoveTime;
    public float sweaperwaitTime;
    public float SweaperEndMoveTime;
    public float sweaperReturnTime;
    float sweapertimer;
    [Header("스포트라이트 오브젝트")]
    public SpotLightObject lightObj; //빛 오브젝트       
    [Header("z축을 고정시키기 위한 보스필드")]
    public Transform bossField; // 보스 스테이지 바닥    
    
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
        moveTarget.transform.position = bossField.position;
    }

    private void Start()
    {
        OnePosition = transform.position;        
    }
    #region 보스 스윕
    public Tuple<Vector3, float> calculateSweapvector(Vector3 position1, Vector3 postion2, float time)
    {
        Vector3 vec = position1 - postion2;
        float distance = vec.magnitude;
        float speed = distance / time;

        return new Tuple<Vector3, float>(vec, speed);
    }
    public IEnumerator Sweaper()
    {

        var tuple = calculateSweapvector(SweaperStartTransform.position, transform.position, SweaperStartMoveTime);
        Vector3 vec = tuple.Item1;
        float speed = tuple.Item2;
        while (sweapertimer <= SweaperStartMoveTime)
        {
            transform.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        yield return new WaitForSeconds(sweaperwaitTime);

        tuple = calculateSweapvector(SweaperEndTransform.position, transform.position, SweaperEndMoveTime);
        vec = tuple.Item1;
        speed = tuple.Item2;
        while (sweapertimer <= SweaperEndMoveTime)
        {
            transform.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        tuple = calculateSweapvector(OnePosition, transform.position, sweaperReturnTime);
        vec = tuple.Item1;
        speed = tuple.Item2;
        while (sweapertimer <= sweaperReturnTime)
        {
            transform.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        transform.position = OnePosition;
        ActionEnd?.Invoke();
    }
    #endregion

    #region 스포트라이트패턴    
    public void SpotLightSHow()
    {
        lightObj.HandleSpotLight(this);
    }

    public IEnumerator SpotLightShow()
    {        
        //InvokeRepeating("MonsterSpawn", InvokeStartTime, InvokeRate);
        lightObj.HandleSpotLight(this);

        yield return null;
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
            for (int i = 0; i < monsterCountMax; i++)
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
    #endregion
}

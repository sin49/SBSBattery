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
    [Header("����Ʈ����Ʈ ������Ʈ")]
    public SpotLightObject lightObj; //�� ������Ʈ       
    [Header("z���� ������Ű�� ���� �����ʵ�")]
    public Transform bossField; // ���� �������� �ٴ�    
    
    [Header("Ÿ���� �̵��� ��ǥ����")]
    public Transform rightEndSpot; // ������ ��ǥ����
    public Transform leftEndSpot; // ���� ��ǥ����
    public Transform targetSpot; // ������ ��ǥ����
    [Header("����Ʈ, �� ȸ���� ���� Ÿ������")]
    public GameObject moveTarget; // ��ǥ�������� �̵��ϴ� ������Ʈ(���� ���� ȸ���� ����)
    public float targetSpeed; // moveTarget�� �ӵ�
    [Header("���Ϳ� ���� ī��Ʈ, monsterCountMax�� ������ �� ����")]
    public int monsterCount; // TV������ ��ǥ ���� ī��Ʈ
    public int monsterActiveCount; // �����Ǿ��ִ� ���� �������� Ȱ��ȭ ī��Ʈ
    public int monsterCountMax; // �ִ� ī��Ʈ �� >> ������ ��, ī��Ʈ �ִ�ġ ����
    [Header("����, ���� ��ƵΰԵ� ����")]
    public GameObject tvMonster; // ���� ������
    public Transform tvMonsterGroup; // �����Ǵ� ���� ��� ����
    [Header("���� ���� ��Ÿ��, �ݺ��� Ÿ�̸�")]
    public float InvokeStartTime; // ���� ���� ��Ÿ��
    public float InvokeRate; // �ݺ� Ÿ�̸�

    private void Awake()
    {
        moveTarget.transform.position = bossField.position;
    }

    private void Start()
    {
        OnePosition = transform.position;        
    }
    #region ���� ����
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

    #region ����Ʈ����Ʈ����    
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
    
    //���� ���� �Լ�
    public void MonsterSpawn()
    {
        Debug.Log("�κ�ũ ������ ����");
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
    //������ ������ġ �����Ͽ� ��ȯ
    public Transform SpawnPosition()
    {
        if (targetSpot == rightEndSpot)
            return leftEndSpot;
        else
            return rightEndSpot;
    }    
    #endregion
}

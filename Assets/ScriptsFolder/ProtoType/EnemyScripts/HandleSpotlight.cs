using System.Collections;
using UnityEngine;

public class HandleSpotlight : MonoBehaviour
{
    public SpotLightObject lightObj; //�� ������Ʈ       
    
    public Transform bossField; // ���� �������� �ٴ�    
    public Vector3 originPos; // ������ �� ���� ��ġ
                              // >> ���߿� ����Ʈ����Ʈ �� �̵� ���� ���� ����ϱ����� �̸� ����
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
        //��ǥ�� �������� ��
        //����Ʈ ������Ʈ�� ������ ������ ���߱� ���� tracking���� false�� ��ȯ
        //(���� �����ϴ� �� SpotLightObject��ũ��Ʈ�� Update�Լ����� ��������)        
        lightObj.tracking = false;

    }
    //��ǥ������ �����Ͽ� Ʈ������ ��ȯ
    public Transform DecideTargetSpot()
    {
        float rightSpot = Vector3.Distance(transform.position, rightEndSpot.position);
        float leftSpot= Vector3.Distance(transform.position, leftEndSpot.position);

        if (rightSpot > leftSpot)
            return rightEndSpot;
        else
            return leftEndSpot;
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
    //������ ������ġ �����Ͽ� ��ȯ
    public Transform SpawnPosition()
    {
        if (targetSpot == rightEndSpot)
            return leftEndSpot;
        else
            return rightEndSpot;
    }
    //���Ͱ� ��ǥ ���� ��ó�� �������� �� �޴� ī��Ʈ �Լ�
    //���ǹ� ���� ��, ����Ʈ ������Ʈ�� �����̼ǰ��� �������� �ʱ�ȭ
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

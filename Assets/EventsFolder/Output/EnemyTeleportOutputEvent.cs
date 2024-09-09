using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeleportOutputEvent : OutputEvent
{
    [Header("지정할 몬스터 오브젝트**필수**")]
    public GameObject selectEnemy;
    [Header("지정할 텔레포트**필수**")]
    public Transform teleportPoint;
    [Header("선택된 좌표")]
    public Transform ePoint;
    [Header("좌표 그룹")]
    public List<Transform> ePointGroup = new List<Transform>();
    [Header("몬스터 그룹")]
    public List<GameObject> eGroup = new List<GameObject>();

    [Header("ePointGroup 이전 번호")] public bool previousPointNum;
    [Header("ePointGroup 다음 번호")] public bool nextPointNum;

    [Header("eGroup 이전 번호")]public bool previousEnemyNum;
    [Header("eGroup 다음 번호")]public bool nextEnemyNum;

    [Header("이전 텔포 트랜스폼")]public bool previousTelpoNum;
    [Header("다음 텔포 트랜스폼")]public bool nextTelpoNum;

    int pointNum, enemyNum, telpoNum;

    public override void output()
    {
        EnemyTeleport();
        base.output();
    }

    private void Update()
    {
        ChangePoint();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            ChangePoint();
    }

    public void EnemyTeleport()
    {
        if (selectEnemy != null && teleportPoint.gameObject != null)
        {
            selectEnemy.transform.position = teleportPoint.position;
            selectEnemy.transform.parent = teleportPoint;
        }
        else
        {
            Debug.Log("지정된 적 혹은 텔레포트 지점이 설정되어있지 않습니다.");
        }
    }

    public void ChangePoint()
    {
        if (previousPointNum)
        {
            previousPointNum = false;
            pointNum--;
            InitPoint();
        }
        
        if (nextPointNum)
        {
            nextPointNum = false;
            pointNum++;
            InitPoint();
        }

        if (previousEnemyNum)
        {
            previousEnemyNum = false;
            enemyNum--;
            InitEnemy();
        }
        if (nextEnemyNum)
        {
            nextEnemyNum = false;
            enemyNum++;
            InitEnemy();
        }


        if (previousTelpoNum)
        {
            previousTelpoNum = false;
            telpoNum--;
            InitTeleport();
        }

        if (nextTelpoNum)
        {
            nextTelpoNum = false;
            telpoNum++;
            InitTeleport();
        }
    }

    //지정 좌표 초기화
    public void InitPoint()
    {
        if (ePointGroup.Count > 0)
        {
            if (pointNum >= ePointGroup.Count)
            {
                pointNum = 0;
            }
            else if (pointNum < 0)
            {
                pointNum = ePointGroup.Count - 1;
            }

            teleportPoint = ePointGroup[pointNum];
            telpoNum = pointNum;
            ePoint = ePointGroup[pointNum];
            if (teleportPoint.childCount > 0)
            {
                InitEnemyWithPoint();
            }
            else
            {
                eGroup.Clear();
                selectEnemy = null;
                Debug.Log("몬스터가 존재하지 않음");
            }
        }
    }
    //지정 좌표와 함께 지정 몬스터도 초기화
    public void InitEnemyWithPoint()
    {
        eGroup.Clear();
        enemyNum = 0;
        if (eGroup.Count < teleportPoint.childCount)
        {
            for (int i = 0; i < teleportPoint.childCount; i++)
            {
                eGroup.Add(teleportPoint.GetChild(i).gameObject);
            }
            selectEnemy = eGroup[enemyNum];
        }
    }
    //지정 몬스터와 몬스터 리스트 초기화
    public void InitEnemy()
    {
        if (eGroup.Count > 0)
        {
            if (enemyNum >= eGroup.Count)
            {
                enemyNum = 0;
            }
            else if (enemyNum < 0)
            {
                enemyNum = eGroup.Count - 1;
            }

            selectEnemy = eGroup[enemyNum];
        }
    }
    //텔레포트 초기화
    public void InitTeleport()
    {
        if (ePointGroup.Count > 0)
        {
            if (telpoNum >= ePointGroup.Count)
            {
                telpoNum = 0;
            }
            else if (telpoNum < 0)
            {
                telpoNum = ePointGroup.Count - 1;
            }            
        }

        teleportPoint = ePointGroup[telpoNum];
    }
}

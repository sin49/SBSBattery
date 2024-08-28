using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyRemoveOutputEvent : OutputEvent
{
    public GameObject selectEnemy;
    public Transform ePoint;
    public List<Transform> ePointGroup = new List<Transform>();
    public List<GameObject> eGroup = new List<GameObject>();

    [Header("좌표 그룹 왼쪽으로 넘기기")]
    public bool previousPointNum;
    [Header("좌표 그룹 오른쪽으로 넘기기")]
    public bool nextPointNum;
    [Header("몬스터 존재 시 변수")]
    public bool previousEnemyNum;
    public bool nextEnemyNum;

    int pointNum, enemyNum;

    public override void output()
    {
        EnemySelectAndDelete();
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
    // 지정된 몬스터를 삭제
    public void EnemySelectAndDelete()
    {
        if (selectEnemy != null)
        {
            if (Application.isPlaying)            
                Destroy(selectEnemy);            
            else
                DestroyImmediate(selectEnemy);

            eGroup.RemoveAt(enemyNum);

            if (eGroup.Count > 0)
            {
                enemyNum = 0;
                selectEnemy = eGroup[enemyNum];
            }
            else
            {
                selectEnemy = null;
            }
        }
        else
        {
            Debug.Log("적 제거 출력이벤트의 적이 지정되어있지 않습니다.");
        }
    }
    //지정 좌표를 변경하는 함수
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

            ePoint = ePointGroup[pointNum];
            if (ePoint.childCount > 0)
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
        if (eGroup.Count < ePoint.childCount)
        {
            for (int i = 0; i < ePoint.childCount; i++)
            {
                eGroup.Add(ePoint.GetChild(i).gameObject);
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
}

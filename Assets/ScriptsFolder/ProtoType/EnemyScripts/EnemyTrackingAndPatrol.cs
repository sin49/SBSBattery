using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrackingAndPatrol : MonoBehaviour
{
    [Header("#공격 활성화 콜라이더 큐브 조정#")]
    [Tooltip("활성화 콜라이더")] public GameObject rangeCollider; // 공격 범위 콜라이더 오브젝트
    [Tooltip("활성화 범위")] public Vector3 rangeSize;
    [Tooltip("활성화 위치")] public Vector3 rangePos;

    [Header("#플레이어 탐색 큐브 조정(콜라이더)#")]
    [Tooltip("탐색 콜라이더")] public GameObject searchCollider; // 탐지 범위 콜라이더
    [Tooltip("탐색 범위")] public Vector3 searchColliderRange;
    [Tooltip("탐색 위치")] public Vector3 searchColliderPos;
    
    [Header("#추격 범위#")]
    [Tooltip("탐색 후 추격 유지 범위")] public float trackingDistance;
    [Tooltip("설정 X")] public float disToPlayer;

    [Header("#정찰 이동관련(정찰 그룹, 정찰목표값, 정찰 대기시간)#")]
    public Transform[] PatrolTransform;
    [Header("스크립트 상에서 값을 임의 결정하게 되어있음")]public Vector3[] patrolGroup; // 0번째: 왼쪽, 1번째: 오른쪽
    [Header("스크립트 상에서 값이 결정됨")]public Vector3 targetPatrol; // 정찰 목표지점-> patrolGroup에서 지정
    [Tooltip("정찰 대기시간")] public float patrolWaitTime; // 정찰 대기시간

    [Header("#정찰 범위 관련#")]
    [Tooltip("왼쪽 정찰 범위")] public float leftPatrolRange; // 좌측 정찰 범위
    [Tooltip("오른쪽 정찰 범위")] public float rightPatrolRange; // 우측 정찰 범위
    [Header("정찰 거리(설정 안해도됨)")] public float patrolDistance; // 정찰 거리

    [HideInInspector]public Vector3 leftPatrol, rightPatrol;
   
    [Header("#그려질 정찰 큐브 사이즈 결정#")]
    [Tooltip("붉은색 높이")] public float yWidth;
    [Tooltip("붉은색 z축 넓이")] public float zWidth;
    [HideInInspector]public Vector3 center;

    private void Update()
    {
        if (rangeCollider != null)
        {
            rangeCollider.GetComponent<BoxCollider>().center = rangePos;
            rangeCollider.GetComponent<BoxCollider>().size = rangeSize;
        }
    }

    public void SetPoint(Transform transform)
    {
        if (PatrolTransform.Length == 0)
        {
            patrolGroup = new Vector3[2];
            patrolGroup[0] = new(transform.position.x - leftPatrolRange, transform.position.y, transform.position.z);
            patrolGroup[1] = new(transform.position.x + rightPatrolRange, transform.position.y, transform.position.z);
            leftPatrol = patrolGroup[0];
            rightPatrol = patrolGroup[1];
        }
        else
        {
            patrolGroup = new Vector3[PatrolTransform.Length];
            for (int n = 0; n < PatrolTransform.Length; n++)
            {
                patrolGroup[n] = PatrolTransform[n].position;
            }
            leftPatrol = patrolGroup[0];
            rightPatrol = patrolGroup[1];
        }
    }

    private void OnDrawGizmos()
    {

        if (searchCollider != null)
        {
            searchCollider.GetComponent<BoxCollider>().size = searchColliderRange;
            searchCollider.GetComponent<BoxCollider>().center = searchColliderPos;

            if (searchCollider.transform.childCount != 0)
            {
                searchCollider.transform.GetChild(0).localScale = searchColliderRange;
                searchCollider.transform.GetChild(0).localPosition = searchColliderPos;
            }
        }


        if (rangeCollider != null)
        {
            rangeCollider.GetComponent<BoxCollider>().size = rangeSize;
            rangeCollider.GetComponent<BoxCollider>().center = rangePos;

            if (rangeCollider.transform.childCount != 0)
            {
                rangeCollider.transform.GetChild(0).localScale = rangeSize;
                rangeCollider.transform.GetChild(0).localPosition = rangePos;
            }

        }
    }
}

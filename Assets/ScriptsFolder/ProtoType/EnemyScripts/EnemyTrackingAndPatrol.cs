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
    public bool searchPlayer;

    [Header("#추격 범위#")]
    [Tooltip("탐색 후 추격 유지 범위")] public float trackingDistance;
    [Tooltip("설정 X")] public float disToPlayer;

    [Header("#정찰 이동관련(정찰 그룹, 정찰목표값, 정찰 대기시간)#")]
    public Transform[] PatrolTransform;
    [Tooltip("설정 X")] public Vector3[] patrolGroup; // 0번째: 왼쪽, 1번째: 오른쪽
    [Tooltip("설정 X")] public Vector3 targetPatrol; // 정찰 목표지점-> patrolGroup에서 지정
    [Tooltip("정찰 대기시간")] public float patrolWaitTime; // 정찰 대기시간

    [Header("#정찰 범위 관련#")]
    [Tooltip("왼쪽 정찰 범위")] public float leftPatrolRange; // 좌측 정찰 범위
    [Tooltip("오른쪽 정찰 범위")] public float rightPatrolRange; // 우측 정찰 범위
    [Tooltip("정찰 거리(설정 안해도됨)")] public float patrolDistance; // 정찰 거리

    protected Vector3 leftPatrol, rightPatrol;

    public bool onPatrol;
    [Header("#그려질 정찰 큐브 사이즈 결정#")]
    [Tooltip("붉은색 높이")] public float yWidth;
    [Tooltip("붉은색 z축 넓이")] public float zWidth;
    Vector3 center;

    [Header("벽 체크 레이캐스트")]
    [Tooltip("벽 체크 Ray의 높이")] public float wallRayHeight;
    [Tooltip("정면 Ray 길이")] public float wallRayLength;
    [Tooltip("위쪽 Ray 길이")] public float wallRayUpLength;

    public Collider forwardWall;
    public Collider upWall;
    public float disToWall;
    public bool wallCheck;
    bool forwardCheck, upCheck;
}

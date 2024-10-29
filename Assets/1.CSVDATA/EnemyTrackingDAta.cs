using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrackingDAta : ScriptableObject
{
    public string tapname;

    [Header("활성화 범위")]
    [Range(0, 10)] public float rangeSizeX;
    [Range(0, 10)] public float rangeSizeY;
    [Range(0, 10)] public float rangeSizeZ;

    [Header("활성화 위치")]
    [Range(0, 30)] public float rangePosX;
    [Range(0, 30)] public float rangePosY;
    [Range(0, 30)] public float rangePosZ;


    [Header("탐색 범위")]
    [Range(0, 10)] public float searchSizeX;
    [Range(0, 10)] public float searchSizeY;
    [Range(0, 10)] public float searchSizeZ;

    [Header("탐색 위치")]
    [Range(0, 30)] public float searchPosX;
    [Range(0, 30)] public float searchPosY;
    [Range(0, 30)] public float searchPosZ;

    [Header("탐색 후 추격 유지 범위")][Range(0, 10)] public float trackingDistance;

    [Tooltip("정찰 대기시간")]
    [Range(0, 1)] public float patrolWaitTime; // 정찰 대기시간

    [Header("#정찰 범위 관련#")]
    [Header("왼쪽 정찰 범위")]
    [Range(0, 5)] public float leftPatrolRange; // 좌측 정찰 범위
    [Header("오른쪽 정찰 범위")]
    [Range(0, 5)] public float rightPatrolRange; // 우측 정찰 범위
    [Header("정찰 거리(최소 0.1)")]
    [Range(0.1f, 5)] public float patrolDistance; // 정찰 거리

    [Header("벽 체크 레이캐스트")]
    [Header("벽 체크 Ray의 높이")][Range(0, 10)] public float wallRayHeight;
    [Header("정면 Ray 길이")][Range(0, 10)] public float wallRayLength;
    [Header("위쪽 Ray 길이")][Range(0, 10)] public float wallRayUpLength;
    [Header("뒤쪽 Ray 길이")][Range(0, 10)] public float wallRayBackLength;
}

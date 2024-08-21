using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharColliderColor : MonoBehaviour
{
    public static CharColliderColor instance;
    [Header("인지 범위(메쉬)")]
    public Color searchRange;
    [Header("공격 활성화 범위(메쉬)")]
    public Color attackActiveRange;
    [Header("추적 범위")]
    public Color trackingRange;
    [Header("정찰 범위")]
    public Color patrolRange;

    private void Awake()
    {
        Debug.Log("에디터에서도 작동 저장한 후에는\n스크립트 비활성화 작업 한번 해줘야됨");

        if (instance == null)
            instance = this;
    }

    private void OnDrawGizmos()
    {
        if (instance == null)
        {
            Debug.Log("에디터 모드로 몬스터 범위 컬러 싱글톤 채워넣는 중");
            instance = this;
        }
    }
}

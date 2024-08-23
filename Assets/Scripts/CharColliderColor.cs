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
       

        if (instance == null)
            instance = this;
    }

    private void OnDrawGizmos()
    {
        if (instance == null)
        {
           
            instance = this;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharColliderColor : MonoBehaviour
{
    public static CharColliderColor instance;

    public Color searchRange;
    public Color attackActiveRange;
    public Color trackingRange;
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
            Debug.Log("싱글톤 비어있음");
            instance = this;
        }
    }
}

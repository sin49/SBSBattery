using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharColliderColor : MonoBehaviour
{
    public static CharColliderColor instance;
    [Header("���� ����")]
    public Color searchRange;
    [Header("���� Ȱ��ȭ ����")]
    public Color attackActiveRange;
    [Header("���� ����")]
    public Color trackingRange;
    [Header("���� ����")]
    public Color patrolRange;

    private void Awake()
    {
        Debug.Log("�����Ϳ����� �۵� ������ �Ŀ���\n��ũ��Ʈ ��Ȱ��ȭ �۾� �ѹ� ����ߵ�");

        if (instance == null)
            instance = this;
    }

    private void OnDrawGizmos()
    {
        if (instance == null)
        {
            Debug.Log("�̱��� �������");
            instance = this;
        }
    }
}

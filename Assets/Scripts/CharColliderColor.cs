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

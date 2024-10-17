using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrackingAndPatrol : MonoBehaviour
{
    [Header("#���� Ȱ��ȭ �ݶ��̴� ť�� ����#")]
    [Tooltip("Ȱ��ȭ �ݶ��̴�")] public GameObject rangeCollider; // ���� ���� �ݶ��̴� ������Ʈ
    [Tooltip("Ȱ��ȭ ����")] public Vector3 rangeSize;
    [Tooltip("Ȱ��ȭ ��ġ")] public Vector3 rangePos;

    [Header("#�÷��̾� Ž�� ť�� ����(�ݶ��̴�)#")]
    [Tooltip("Ž�� �ݶ��̴�")] public GameObject searchCollider; // Ž�� ���� �ݶ��̴�
    [Tooltip("Ž�� ����")] public Vector3 searchColliderRange;
    [Tooltip("Ž�� ��ġ")] public Vector3 searchColliderPos;
    
    [Header("#�߰� ����#")]
    [Tooltip("Ž�� �� �߰� ���� ����")] public float trackingDistance;
    [Tooltip("���� X")] public float disToPlayer;

    [Header("#���� �̵�����(���� �׷�, ������ǥ��, ���� ���ð�)#")]
    public Transform[] PatrolTransform;
    [Header("��ũ��Ʈ �󿡼� ���� ���� �����ϰ� �Ǿ�����")]public Vector3[] patrolGroup; // 0��°: ����, 1��°: ������
    [Header("��ũ��Ʈ �󿡼� ���� ������")]public Vector3 targetPatrol; // ���� ��ǥ����-> patrolGroup���� ����
    [Tooltip("���� ���ð�")] public float patrolWaitTime; // ���� ���ð�

    [Header("#���� ���� ����#")]
    [Tooltip("���� ���� ����")] public float leftPatrolRange; // ���� ���� ����
    [Tooltip("������ ���� ����")] public float rightPatrolRange; // ���� ���� ����
    [Header("���� �Ÿ�(���� ���ص���)")] public float patrolDistance; // ���� �Ÿ�

    [HideInInspector]public Vector3 leftPatrol, rightPatrol;
   
    [Header("#�׷��� ���� ť�� ������ ����#")]
    [Tooltip("������ ����")] public float yWidth;
    [Tooltip("������ z�� ����")] public float zWidth;
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

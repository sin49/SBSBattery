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
    public bool searchPlayer;

    [Header("#�߰� ����#")]
    [Tooltip("Ž�� �� �߰� ���� ����")] public float trackingDistance;
    [Tooltip("���� X")] public float disToPlayer;

    [Header("#���� �̵�����(���� �׷�, ������ǥ��, ���� ���ð�)#")]
    public Transform[] PatrolTransform;
    [Tooltip("���� X")] public Vector3[] patrolGroup; // 0��°: ����, 1��°: ������
    [Tooltip("���� X")] public Vector3 targetPatrol; // ���� ��ǥ����-> patrolGroup���� ����
    [Tooltip("���� ���ð�")] public float patrolWaitTime; // ���� ���ð�

    [Header("#���� ���� ����#")]
    [Tooltip("���� ���� ����")] public float leftPatrolRange; // ���� ���� ����
    [Tooltip("������ ���� ����")] public float rightPatrolRange; // ���� ���� ����
    [Tooltip("���� �Ÿ�(���� ���ص���)")] public float patrolDistance; // ���� �Ÿ�

    protected Vector3 leftPatrol, rightPatrol;

    public bool onPatrol;
    [Header("#�׷��� ���� ť�� ������ ����#")]
    [Tooltip("������ ����")] public float yWidth;
    [Tooltip("������ z�� ����")] public float zWidth;
    Vector3 center;

    [Header("�� üũ ����ĳ��Ʈ")]
    [Tooltip("�� üũ Ray�� ����")] public float wallRayHeight;
    [Tooltip("���� Ray ����")] public float wallRayLength;
    [Tooltip("���� Ray ����")] public float wallRayUpLength;

    public Collider forwardWall;
    public Collider upWall;
    public float disToWall;
    public bool wallCheck;
    bool forwardCheck, upCheck;
}

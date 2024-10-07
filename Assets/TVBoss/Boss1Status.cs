using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Status : MonoBehaviour
{
    [Header("���� ����")]
    [Header("���� ����� ü��(����)")]
    public int lifeCountMax;
    [Header("���� ����(���� �������)")]
    public bool randomPattern;
    [Header("�۾��� ���ϸ� ���")]
    public bool OnlySweapPattern;
    [Header("������ ���ϸ� ���")]
    public bool OnlylaserPattern;
    [Header("���Ϲ� ���ϸ� ���")]
    public bool OnlyfallPattern;
    [Header("�� ü��(�÷��̸���� �ٲ㵵 ���� �ȵ�)")]
    public float HandHP;
    [Header("�� �ı��� �̵� ��ġ")]
    public Transform LhandDefeatTransform;
    public Transform RhandDefeatTransform;
    [Header("���� ���Ϲ�")]
    [Header("���� ���Ϲ�����Ʈ(�÷��̸���� �ٲ㵵 ���� �ȵ�)")]
    public List<Boss1FallObj> fallingObj2;
    [Header("���� ���� ������Ʈ ����(�÷��̸���� �ٲ㵵 ���� �ȵ�)")]
    public List<Boss1BoxFallCreateObj> fallingBoxCreateObj;
    [Header("������")]
    public float damage;
    [Header("�����ð�, �ּ�/�ִ�ӵ�")]
    public float createTime;
    public float minSpeed;
    public float maxSpeed;
    [Header("���Ϲ� ī��Ʈ ����, ���� ����")]
    public int createCountMax;
    public float fallingHeight;
    [Header("���� ���� ����")]
    public float fallingRange;
    [Header("���� ���� ����� ��")]
    public Color GizmoColor;
    [Header("���� �۾���")]
    [Header("�� ũ��(�� y�� ������ ����)")]
    public float handsize = 1;
    [Header("�۾��� �� �� �� ���� �۾��� ������ ����")]
    public float SweaperPatternDealy;
    [Header("�׽�Ʈ �� ���ڰ� 0�̵Ǹ� ���峲")]
    [Header("�۾��� ���� ���� �۾��� ���� �������� ���� �ð�")]
    public float SweaperStartMoveTime;
    [Header("������������ ����ϴ� �ð�")]
    public float sweaperwaitTime;
    [Header("������������ ��ǥ�������� ���� �ð�")]
    public float SweaperEndMoveTime;
    [Header("��ǥ�������� ����ϴ� �ð�")]
    public float SweaperEndWaitTime;
    [Header("��ǥ�������� �̵� �� �ٽ� ����ġ�ϴ� �ð�")]
    public float sweaperReturnTime;
    [Header("�۾��� ����� ����")]
    public Color SweapColor;
    [Header("���� ������")]
    [Header("������ ���� �ð�")]
    public float laserlifetime = 1.5f;
    [Header("������ Y�� ��ġ")]
    public float LaserYpos = -6.4f;

    [Header("������ �ӵ�")]
    public float LaserSpeed;
    [Header("������ Ȱ��ȭ ������ �ð�")]
    public float laserActiveTimer = 1.5f;
    [Header("���� ����")]
    public float TrailColScale = 1;
    [Header("���� ���� �ð�")]
    public float TrailDuration;
    [Header("���� ���� ���� ���� ����")]
    public float ColliderSpawnTime;


    Boss1Laser laser;
    BossTv boss;
    BossFalling BossFalling;
    Boss1Sweap sweap;
    private void Awake()
    {
        laser=transform.parent.GetComponent<Boss1Laser>();
        boss= transform.parent.GetComponent<BossTv>();
        BossFalling=transform.parent.GetComponent<BossFalling>();
        sweap = transform.parent.GetComponent<Boss1Sweap>();
        updateStatus();
    }
    private void Update()
    {
        updateStatus();
    }
    void updateStatus()
    {
        if (boss != null)
        {
            boss.lifeCountMax = lifeCountMax;
            boss.HandHP = HandHP;
            boss.randomPattern = randomPattern;
            if (OnlySweapPattern)
            {
                boss.OnlyTestPattern = true;
                boss.TestAction = sweap;
            }else if (OnlyfallPattern)
            {
                boss.OnlyTestPattern = true;
                boss.TestAction = BossFalling;
            }else if (OnlylaserPattern)
            {
                boss.OnlyTestPattern = true;
                boss.TestAction = laser;
            }
            else
            {
                boss.OnlyTestPattern = false;
            }
            sweap.LhandDefeatTransform = LhandDefeatTransform;
            sweap.RhandDefeatTransform= RhandDefeatTransform;
            sweap.handsize = handsize;
            sweap.SweaperEndMoveTime= SweaperEndMoveTime;
            sweap.SweaperEndWaitTime= SweaperEndWaitTime;
            sweap.sweaperReturnTime= sweaperReturnTime;
            sweap.SweaperStartMoveTime= SweaperStartMoveTime;
            sweap.sweaperwaitTime= sweaperwaitTime;
            sweap.SweaperPatternDealy = SweaperPatternDealy;
            sweap.sweapColor = SweapColor;

            laser.ActionLifeTIme = laserlifetime;
            laser.LaserYpos = LaserYpos;
            laser.laserActiveTimer = laserActiveTimer;
            laser.TrailColScale = TrailColScale;
            laser.TrailDuration = TrailDuration;
            laser.ColliderSpawnTime = ColliderSpawnTime;

            BossFalling.fallingObj2 = fallingObj2;
            BossFalling.fallingBoxCreateObj = fallingBoxCreateObj;

            BossFalling.damage = damage;
            BossFalling.createTime = createTime;
            BossFalling.minSpeed = minSpeed;
            BossFalling.maxSpeed = maxSpeed;

            BossFalling.createCountMax = createCountMax;
            BossFalling.fallingRange = fallingRange;
            BossFalling.fallingHeight = fallingHeight;
            BossFalling.GizmoColor = GizmoColor;
        }
    }
}

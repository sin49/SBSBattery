using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Status : MonoBehaviour
{
    [Header("���� ����")]
    [Header("���� ����� ü��(����)")]
    public int lifeCountMax;
    [Header("���� �� �����̽ð�")]
    public float patterndelay;
    [Header("���� ����(���� �������)")]
    public bool randomPattern;
    [Header("�۾��� ���ϸ� ���")]
    public bool OnlySweapPattern;
    [Header("������ ���ϸ� ���")]
    public bool OnlylaserPattern;
    [Header("������2D ���ϸ� ���")]
    public bool Onlylaser2DPattern;
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
    [Header("������� Ƚ��")]
    public int stombcount;
    [Header("�������� �� y�� ���� ����")]
    public float stombYPlus;
    [Header("������� �غ� �ð�")]
    public float stombinittime;

    [Header("������� �� ��� �ð�")]
    public float stombwaitTIme;
    [Header("������� �ð�")]
    public float stombtime;
    [Header("������� ��� ��� �ð�")]
    public float stombendwaitTIme;
    [Header("������� ���� ���ƿ��� �ð�")]
    public float stombreturntime;
    [Header("������ ���ڸ��� ���ƿ��� �ð�")]
    public float stombreturntime2;
    [Header("������� y��ǥ")]
    public float  stombYpos ;


    [Header("���� �۾���(�̰� ���� 2������ �κп� �� ���� �־�� �ǰ����� �ϴ� ����)")]
  
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


  
    [Header("������2D ��� �ð�")]
    public float laserwarngingTIme;
    [Header("������2D Ȱ��ȭ �ð�")]
    public float laseractiveTIme;


    Boss1Laser laser;
    BossTv boss;
    BossFalling BossFalling;
    Boss1Sweap sweap;
    public Boss1Laser2D laser2D;
    private void Awake()
    {
        laser = transform.parent.GetComponent<Boss1Laser>();
        boss = transform.parent.GetComponent<BossTv>();
        BossFalling = transform.parent.GetComponent<BossFalling>();
        sweap = transform.parent.GetComponent<Boss1Sweap>();

        updateStatus();
    }
    private void Update()
    {
        if(!boss.Phase2)
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
            }
            else if (OnlyfallPattern)
            {
                boss.OnlyTestPattern = true;
                boss.TestAction = BossFalling;
            }
            else if (OnlylaserPattern)
            {
                boss.OnlyTestPattern = true;
                boss.TestAction = laser;
            }
          else if (Onlylaser2DPattern) {
                boss.OnlyTestPattern = true;
                boss.TestAction = laser2D;
            }

            else
            {
                boss.OnlyTestPattern = false;
            }
            boss.patterndelay = patterndelay;
            sweap.LhandDefeatTransform = LhandDefeatTransform;
            sweap.RhandDefeatTransform = RhandDefeatTransform;
            sweap.handsize = handsize;
            sweap.SweaperEndMoveTime = SweaperEndMoveTime;
            sweap.SweaperEndWaitTime = SweaperEndWaitTime;
            sweap.sweaperReturnTime = sweaperReturnTime;
            sweap.SweaperStartMoveTime = SweaperStartMoveTime;
            sweap.sweaperwaitTime = sweaperwaitTime;
            sweap.SweaperPatternDealy = SweaperPatternDealy;
            sweap.sweapColor = SweapColor;
            sweap.stombcount = stombcount;
            sweap.stombYPlus= stombYPlus;
            sweap.stombinittime = stombinittime;
            sweap.stombtime = stombtime;
            sweap.stombwaitTIme= stombwaitTIme;
            sweap.stombendwaitTIme= stombendwaitTIme;
            sweap.stombreturntime= stombreturntime;
            sweap.stombYEnd = stombYpos;
            sweap.stombwaitTIme2 = stombreturntime2;
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

            laser2D.LaserWaringTime = laserwarngingTIme;
            laser2D.laseractiveTime = laseractiveTIme;

        }
    }
}


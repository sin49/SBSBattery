using System;
using UnityEngine;

//��� �κ� �ϼ��ϱ�
[Serializable]
public class Drop
{
    public GameObject parts; // ��ȭ�� Ȱ���� ��ǰ ������Ʈ
    public int partValue; // ��ǰ ���� ��Ÿ�� ���� (��� ����)
}

public class EnemyStat : CharacterStat
{
    [Header("������ �̵� AI")] public EnemyMovePattern movepattern;
    [Header("������ �̵� ���")] public EnemyMoveType movetype;
    [Header(("������ ���� ���"))] public EnemyAttackType attacktype;
    [Header("��� ������ ���Ƿ� �ۼ�(������� ��)")]
    [Header("���� �������� Ȱ��ȭ ���ð�")]public float attackReadyTime;
    public Drop dropGroup; // ��� ��, ����� ��ȭ�� ���� Ŭ���� ����

}

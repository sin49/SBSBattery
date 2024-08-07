using System;
using UnityEngine;

[Serializable]
public class Drop
{
    public GameObject parts; // ��ȭ�� Ȱ���� ��ǰ ������Ʈ
    public int partValue; // ��ǰ ���� ��Ÿ�� ���� (��� ����)
}

public class EnemyStat : CharacterStat
{
    [Header("�� ���º�ȭ")]
    public EnemyState eState;
    [Header("��� ������ ���Ƿ� �ۼ�(������� ��)")]
    public Drop dropGroup; // ��� ��, ����� ��ȭ�� ���� Ŭ���� ����
    [Header("�� �������� ����")]
    public bool onInvincible;
    public float invincibleTimer;
}

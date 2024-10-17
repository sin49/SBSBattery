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
    [Header("��� ������ ���Ƿ� �ۼ�(������� ��)")]
    public Drop dropGroup; // ��� ��, ����� ��ȭ�� ���� Ŭ���� ����
}

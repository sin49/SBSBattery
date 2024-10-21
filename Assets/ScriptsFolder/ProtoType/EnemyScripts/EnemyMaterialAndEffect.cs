using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaterialAndEffect : MonoBehaviour
{
   


    [Header("�⺻ ��Ƽ����")]
    [Header("�⺻ �� ��, ������ / ������ ���̽�, ������ �⺻ ��Ƽ����")] public Material idleMat;
    [Header("�⺻ �� ��κ�, ���� ���� �ʶ��Ʈ")] public Material backMat;
    [Header("�⺻ �� �ٵ� ��Ƽ����, ������ ���� / ������ ����")] public Material headMat;
    [Header("�⺻�� ����, ������ ����, ")] public Material hittedMat;
    [Header("�⺻��, ������")]public Renderer skinRenderer;

    [Header("Emmission ��Ƽ����(�ǰ� �� �ٸ� ���� ==> ���� ��Ƽ����)")] 
    [Header("�⺻�� ��, ������ �ʶ��Ʈ")]public Material emmissionBackMat;
    [Header("�⺻�� �ٵ�, ������ ����")]public Material emmissionHeadMat;
    [Header("�⺻�� ���̽�, ������ �ٵ�")]public Material emmissionHittedMat;
    [Header("�Ϲݸ��� ���")]public Renderer skinHead; //  �Ϲ� ���͸� ��
    // ���� ��Ƽ���� �κ�(�𵨸� ����)
    // 3�� => ��������(����, ��������, �ʶ��Ʈ)
    // 2�� / 1��(==��Ų ������ 2��)  => �Ϲݸ���(��,��), (�Ӹ� �����)
    // 2�� => ���� ����(����, �� �����)
    // 1�� => ���� ����, �� ����

    [Header("�������Ʈ")] public ParticleSystem deadEffect;

    public void StartEmmissionHitMat()
    {

    }

    public void EndEmmissionHitMat()
    {

    }

    public void EndHitMat()
    {

    }
}

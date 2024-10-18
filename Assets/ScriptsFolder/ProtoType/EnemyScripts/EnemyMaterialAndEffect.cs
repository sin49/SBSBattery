using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaterialAndEffect : MonoBehaviour
{
   


    [Header("�⺻ ��Ƽ����")]
    public Material idleMat;
    public Material backMat;
    public Material headMat;
    public Material hittedMat;
    public Renderer skinRenderer;

    public ParticleSystem deadEffect;
    public ParticleSystem moveEffect;

    [Header("Emmission ��Ƽ����")] 
    public Material emmissionBackMat;
    public Material emmissionHeadMat;
    public Material emmissionHittedMat;
    [Header("�Ϲݸ��� ���")]public Renderer skinHead; //  �Ϲ� ���͸� ��
    // ���� ��Ƽ���� �κ�(�𵨸� ����)
    // 3�� => ��������(����, ��������, �ʶ��Ʈ)
    // 2�� / 1��(==��Ų ������ 2��)  => �Ϲݸ���(��,��), (�Ӹ� �����)
    // 2�� => ���� ����(����, �� �����)
    // 1�� => ���� ����, �� ����
}

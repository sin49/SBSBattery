using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [Header("�⺻�� ��, ������ �ٵ�, ������ �ʶ��Ʈ")]public Material emmissionBackMat;
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
        //if(emmissionBackMat !=null)
        Material[] materials = skinRenderer.materials;
        switch (materials.Length)
        {
            case 1:
                materials[0] = emmissionHittedMat;
                skinRenderer.materials = materials;
                break;
            case 2:
                materials[0] = emmissionBackMat;
                materials[1] = emmissionHittedMat;
                skinRenderer.materials = materials;
                break;
            case 3:
                materials[0] = emmissionBackMat;
                materials[1] = emmissionHeadMat;
                materials[2] = emmissionHittedMat;
                skinRenderer.materials = materials;
                break;
            default:
                break;
        }

        if (skinHead != null)
            skinHead.material = emmissionBackMat;
    }

    public void EndEmmissionHitMat()
    {
        Material[] materials = skinRenderer.materials;
        switch (materials.Length)
        {
            case 1:
                materials[0] = hittedMat;
                skinRenderer.materials = materials;
                break;
            case 2:
                materials[0] = backMat;
                materials[1] = hittedMat;
                skinRenderer.materials = materials;
                break;
            case 3:
                materials[0] = backMat;
                materials[1] = headMat;
                materials[2] = hittedMat;
                skinRenderer.materials = materials;
                break;
            default:
                break;
        }

        if (skinHead != null)
            skinHead.material = backMat;
    }

    public void EndHitMat()
    {
        Material[] materials = skinRenderer.materials;
        switch (materials.Length)
        {
            case 1:
                materials[0] = idleMat;
                skinRenderer.materials = materials;
                break;
            case 2:
                materials[0] = idleMat;
                materials[1] = backMat;
                skinRenderer.materials = materials;
                break;
            case 3:
                materials[0] = idleMat;
                materials[1] = headMat;
                materials[2] = backMat;
                skinRenderer.materials = materials;
                break;
            default:
                break;
        }

        if (skinHead != null)
            skinHead.material = backMat;
    }
}

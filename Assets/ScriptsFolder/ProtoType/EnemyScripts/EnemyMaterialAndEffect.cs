using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaterialAndEffect : MonoBehaviour
{
    [Header("기본 머티리얼")]
    public Material idleMat;
    public Material backMat;
    public Material headMat;
    public Material hittedMat;
    public Renderer skinRenderer;

    public ParticleSystem deadEffect;
    public ParticleSystem moveEffect;
}

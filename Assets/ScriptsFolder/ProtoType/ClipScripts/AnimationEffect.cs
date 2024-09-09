using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffect : MonoBehaviour
{
    public ParticleSystem[] particleEffect;

    public void RunEffect()
    {
        particleEffect[0].Play();
    }

    public void AttackEffect()
    {

    }
}

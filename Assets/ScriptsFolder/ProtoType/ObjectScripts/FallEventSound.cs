using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEventSound : SEPlayer
{
    [Header("³«»ç È¿°úÀ½")]
    public AudioClip fallingClip;
    [Header("³«»ç º¼·ý")]
    [Range(0, 1)] public float fallingVolume;

    public void FallingSoundPlay()
    {
        audiosource.Stop();
        audiosource.clip = fallingClip;
        audiosource.volume = fallingVolume;
        audiosource.Play();
    }
}

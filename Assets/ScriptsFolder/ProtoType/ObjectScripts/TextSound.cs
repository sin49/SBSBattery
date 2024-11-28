using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSound : SEPlayer
{
    [Header("텍스트 클립")] public AudioClip textClip;
    [Header("텍스트 사운드 볼륨")]
    [Range(0, 1)] public float textVolume;

    protected override void Start()
    {
        base.Start();
        audiosource.clip = textClip;
        audiosource.volume = textVolume;
        audiosource.pitch = 0.55f;
        audiosource.playOnAwake = false;
    }

    public void PlayTextAudio()
    {
        audiosource.Stop();
        audiosource.Play();
    }
}

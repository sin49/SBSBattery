using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1HandSoundPlayer : SEPlayer
{
    [Header("손 피격 사운드")]
    public AudioClip HandHittedclip;
    [Header("손 피격 볼륨"), Range(0, 1)]
    public float HandHittedVolume;
    public void HandHittedClipPlay()
    {
        if (HandHittedclip != null)
        {
            audiosource.Stop();
            audiosource.clip =HandHittedclip;
            audiosource.volume = HandHittedVolume;
            audiosource.Play();
        }
    }
    [Header("손 파괴 사운드")]
    public AudioClip HandDestoryclip;
    [Header("손 파괴 볼륨"), Range(0, 1)]
    public float HandDestoryVolume;
    public void HandDestoryClipPlay()
    {
        if (HandDestoryclip != null)
        {
            audiosource.Stop();
            audiosource.clip = HandDestoryclip;
            audiosource.volume = HandDestoryVolume;
            audiosource.Play();
        }
    }
}

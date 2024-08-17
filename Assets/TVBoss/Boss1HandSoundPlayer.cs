using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1HandSoundPlayer : SEPlayer
{
    [Header("�� �ǰ� ����")]
    public AudioClip HandHittedclip;
    [Header("�� �ǰ� ����"), Range(0, 1)]
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
    [Header("�� �ı� ����")]
    public AudioClip HandDestoryclip;
    [Header("�� �ı� ����"), Range(0, 1)]
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

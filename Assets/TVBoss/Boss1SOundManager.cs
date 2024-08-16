using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1SOundManager : SEPlayer
{
    [Header("Ƽ���� ����")]
    public AudioClip TMPclip;
    [Header("Ƽ���� ����"), Range(0, 1)]
    public float TMPVolume;
    public void TMPClipPlay()
    {
        if (TMPclip != null)
        {
            audiosource.Stop();
            audiosource.clip = TMPclip;
            audiosource.volume = TMPVolume;
            audiosource.Play();
        }
    }
}

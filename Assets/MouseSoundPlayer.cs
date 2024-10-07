using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSoundPlayer : PlayerSoundPlayer
{
    [Header("�� ���� ��� ����")]
    public AudioClip formchangeClip;
    [Header("�� ���� ��� ����"), Range(0, 1)]
    public float formchangeVolume;
    public void FormChangePlay()
    {
        if (formchangeClip == null)
            return;
        audiosource.Stop();
        audiosource.loop = false;
        audiosource.clip = formchangeClip;
        audiosource.volume = formchangeVolume;
        audiosource.Play();
    }

}

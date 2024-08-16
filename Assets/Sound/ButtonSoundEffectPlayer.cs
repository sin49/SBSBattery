using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEffectPlayer : SEPlayer
{
    [Header("���� ����")]
    public AudioClip SelectClip;
    [Header("���� ���� ����"), Range(0, 1)]
    public float SelectVolume;
    [Header("Ȱ��ȭ ����")]
    public AudioClip ActiveClip;
    [Header("Ȱ��ȭ ���� ����"), Range(0, 1)]
    public float ActiveVolume;
    [Header("��� ����")]
    public AudioClip DeActiveClip;
    [Header("��� ���� ����"), Range(0, 1)]
    public float DeActiveVolume;


   

    public void PlaySelectAudio()
    {
        audiosource.Stop();
        audiosource.clip = SelectClip;
        audiosource.volume = SelectVolume;
        audiosource.Play();
    }
    public void PlayActiveAudio()
    {
        audiosource.Stop();
        audiosource.clip = ActiveClip;
        audiosource.volume = ActiveVolume;
        audiosource.Play();
    }
    public void PlayDeActiveAudio()
    {
        audiosource.Stop();
        audiosource.clip = DeActiveClip;
        audiosource.volume = DeActiveVolume;
        audiosource.Play();
    }
  
}

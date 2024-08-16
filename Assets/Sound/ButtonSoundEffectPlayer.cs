using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEffectPlayer : SEPlayer
{
    [Header("선택 사운드")]
    public AudioClip SelectClip;
    [Header("선택 사운드 볼륨"), Range(0, 1)]
    public float SelectVolume;
    [Header("활성화 사운드")]
    public AudioClip ActiveClip;
    [Header("활성화 사운드 볼륨"), Range(0, 1)]
    public float ActiveVolume;
    [Header("취소 사운드")]
    public AudioClip DeActiveClip;
    [Header("취소 사운드 볼륨"), Range(0, 1)]
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

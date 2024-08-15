using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEffectPlayer : MonoBehaviour,SEPlayer
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
    AudioType audiotype = AudioType.SE;
    [Header("무시해도 됨")]
    public AudioSource audiosource;
    private void Awake()
    {
        if (!audiosource.TryGetComponent<AudioSource>(out audiosource))
        {
            audiosource = gameObject.AddComponent<AudioSource>();
            audiosource.minDistance = audiosource.maxDistance;
            audiosource.dopplerLevel = 0;
            audiosource.loop = false;
        }

    }
    private void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.GetAudioSetting(audiotype, audiosource);
        }
    }

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
    private void OnDestroy()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.RemoveSEMember(this);
        }
    }
}

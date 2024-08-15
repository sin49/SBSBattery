using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEffectPlayer : MonoBehaviour,SEPlayer
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
    AudioType audiotype = AudioType.SE;
    [Header("�����ص� ��")]
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

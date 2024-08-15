using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour, SEPlayer
{
    [Header("�Ÿ� ���� ����")]
    [Header("����7(���е� �ƴ�)�� ������ ����� ����� �� �ְ� ��")]
    [Header("����7�� �׽�Ʈ�� �� ���� �� �ϸ� ���ļ� ����Ǵ� �Ͱ� ����")]
    [Header("����� �����")]
    public AudioClip audioclip;
    [Header("�� ������� ����"),Range(0,1)]
    public float volume;

    AudioType audiotype = AudioType.SE;
    [Header("�����ص� ��")]
    public AudioSource audiosource;

    bool OnViewport;
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
    private void OnBecameVisible()
    {
        OnViewport = true;
    }
    private void OnBecameInvisible()
    {
        OnViewport = false;
    }
    private void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.GetAudioSetting(audiotype, audiosource);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            PlayAudio();
        }
    }
    public void PlayAudio()
    {
        if (audioclip != null)
        {
            audiosource.clip = audioclip;
            audiosource.volume = volume;
            audiosource.Play();
        }
    }
    private void OnDestroy()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.RemoveSEMember(this);
        }
    }
}

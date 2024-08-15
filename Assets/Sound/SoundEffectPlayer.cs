using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour, SEPlayer
{
    [Header("거리 감쇠 없음")]
    [Header("숫자7(넘패드 아님)을 누르면 오디오 재생할 수 있게 함")]
    [Header("숫자7로 테스트할 때 여러 개 하면 겹쳐서 재생되니 귀갱 주의")]
    [Header("재생할 오디오")]
    public AudioClip audioclip;
    [Header("이 오디오의 볼륨"),Range(0,1)]
    public float volume;

    AudioType audiotype = AudioType.SE;
    [Header("무시해도 됨")]
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

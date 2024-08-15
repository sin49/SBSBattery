using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundAudioPlayer : MonoBehaviour
{
    [Header("재생할 오디오")]
    public AudioClip audioclip;

    AudioType audiotype = AudioType.BG;

    [Header("이 오디오의 볼륨"), Range(0, 1)]
    public float volume;


 AudioSource audiosource;
    private void Awake()
    {

           audiosource= gameObject.AddComponent<AudioSource>();
            audiosource.minDistance = audiosource.maxDistance;
            audiosource.dopplerLevel = 0;
            audiosource.loop = true;

    }
    private void Update()
    {
        audiosource.volume = volume;
    }
    private void OnEnable()
    {
        audiosource.clip = audioclip;
        if (AudioManager.instance != null)
        {
            AudioManager.instance.GetAudioSetting(audiotype, audiosource);
        }
        audiosource.Play();
    }
    private void OnDisable()
    {
        audiosource.Stop();
    }
}

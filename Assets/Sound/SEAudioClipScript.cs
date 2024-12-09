using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAudioClipScript : MonoBehaviour
{
    protected AudioSource audiosource;
    bool StartPlating;
    private void Awake()
    {
        audiosource = gameObject.AddComponent<AudioSource>();
        audiosource.minDistance = audiosource.maxDistance;
        audiosource.dopplerLevel = 0;
        audiosource.loop = false;
    }
    private void Start()
    {
        if (AudioManager.instance != null)
        {

            AudioManager.instance.setAudiogroupSettingSE(audiosource);
        }
    }
    private void Update()
    {
        if (StartPlating && !audiosource.isPlaying)//이거 비활성 +풀링써서 최적화해야됨
            SoundPoolingManager.instance.ReturnSoundPooling(this.gameObject, audiosource);
    }
    public void PlayAudioSource(AudioClip clip,float volume)
    {
        audiosource.clip = clip;
        audiosource.volume = volume;
        StartPlating = true;
        audiosource.Play();
    }
}

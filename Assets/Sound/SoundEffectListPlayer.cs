using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SoundEffectObj
{
    public AudioClip clip;
    [Range(0,1)]
    public float volume;
}
public class SoundEffectListPlayer : SEPlayer
{
    [Header("범용 SE 플레이어"),SerializeField]
 public List<SoundEffectObj> list;
    public void PlayAudioNoCancel(int n)
    {
        if (n >= list.Count)
        {

        }
        else if(!audiosource.isPlaying)
        {
            if (list[n] != null)
            {
               
                audiosource.clip = list[n].clip;
                audiosource.volume = list[n].volume;
                audiosource.Play();
            }
        }
    }
    public void StopSound()
    {
        audiosource.Stop();
    }
    public void PlayAudio(int n)
    {
        if(audiosource.loop)
            audiosource.loop = false;
        if (n >= list.Count)
        {
            Debug.Log("index Error");
        }
        else
        {
            if (list[n] != null)
            {
                audiosource.Stop();
                audiosource.clip = list[n].clip;
                audiosource.volume = list[n].volume;
                audiosource.Play();
            }
        }
    }
}

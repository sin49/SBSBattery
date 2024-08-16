using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class SoundEffectObj
{
    public AudioClip clip;
    [Range(0,1)]
    public float volume;
}
public class SoundEffectListPlayer : SEPlayer
{
    [Header("범용 SE 플레이어")]
    [Header("범용이라 쓰기 힘든 점 이해 좀")]
 public List<SoundEffectObj> list;

    public void PlayAudio(int n)
    {
        if (n >= list.Count)
        {
            Debug.Log("index Error");
        }
        else
        {
            audiosource.Stop();
            audiosource.clip = list[n].clip;
            audiosource.volume = list[n].volume;
            audiosource.Play();
        }
    }
}

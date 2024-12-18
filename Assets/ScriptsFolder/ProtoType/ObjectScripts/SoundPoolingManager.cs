using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class SoundPooling
{    
    public AudioClip soundValue;
    public Queue<GameObject> soundPool;
}

public class SoundPoolingManager : MonoBehaviour
{
    public static SoundPoolingManager instance;
    public SoundPooling[] sPool;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        foreach (var s in sPool)
        {
            s.soundPool = new Queue<GameObject>();
        }
    }

    public void GetSoundPooling(AudioClip clip, float volume)
    {
        //Debug.Log("사운드 풀링 요청");
        for (int i = 0; i < sPool.Length; i++)
        {
            if (sPool[i].soundValue == clip)
            {
                if (sPool[i].soundPool.Count == 0)
                {
                    MakeSoundPool(clip, volume);                    
                }
                else
                {
                   GameObject poolObj = sPool[i].soundPool.Dequeue();
                    poolObj.SetActive(true);
                    var script = poolObj.GetComponent<SEAudioClipScript>();
                    script.PlayAudioSource(clip, volume);
                }
            }
        }
    }

    public void MakeSoundPool(AudioClip clip, float volume)
    {
        GameObject poolObj = Instantiate(new GameObject());
        var script = poolObj.AddComponent<SEAudioClipScript>();
        script.PlayAudioSource(clip, volume);
        poolObj.transform.SetParent(transform);
    }

    public void ReturnSoundPooling(GameObject obj, AudioSource se)
    {
        for (int i = 0; i < sPool.Length; i++)
        {
            if (sPool[i].soundValue == se.clip)
            {
                sPool[i].soundPool.Enqueue(obj);
                obj.SetActive(false);
            }
        }
    }
}

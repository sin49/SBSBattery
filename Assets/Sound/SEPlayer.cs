using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
   protected AudioSource audiosource;
    protected AudioType audiotype = AudioType.SE;

    protected void AddAudioSource(GameObject obj,AudioSource source)
    {
        source = obj.AddComponent<AudioSource>();
        source.minDistance = audiosource.maxDistance;
        source.dopplerLevel = 0;
        source.loop = false;
    }
    protected virtual void Awake()
    {
        AddAudioSource(gameObject, audiosource);
        //audiosource = gameObject.AddComponent<AudioSource>();
        //audiosource.minDistance = audiosource.maxDistance;
        //audiosource.dopplerLevel = 0;
        //audiosource.loop = false;


    }
    protected virtual void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.GetAudioSetting(audiotype, audiosource);
        }
    }
    protected virtual void OnDestroy()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.RemoveSEMember(this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
   protected AudioSource audiosource;
    protected AudioType audiotype = AudioType.SE;
    protected void MakeSeAudioClip(AudioClip clip,float volume)
    {
        GameObject clipobject = Instantiate(new GameObject());
   var script=    clipobject.AddComponent<SEAudioClipScript>();
    }
    protected void AddAudioSource(GameObject obj)
    {
        audiosource = obj.AddComponent<AudioSource>();
        audiosource.minDistance = audiosource.maxDistance;
        audiosource.dopplerLevel = 0;
        audiosource.loop = false;
    }
    protected virtual void Awake()
    {
        AddAudioSource(gameObject);

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
            AudioManager.instance.setAudiogroupSettingSE(audiosource);
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

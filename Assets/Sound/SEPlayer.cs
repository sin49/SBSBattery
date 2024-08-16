using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
   protected AudioSource audiosource;
    protected AudioType audiotype = AudioType.SE;

    protected virtual void Awake()
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
            AudioManager.instance.GetAudioSetting(audiotype, audiosource);
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

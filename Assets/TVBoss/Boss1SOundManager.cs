using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1SOundManager : SEPlayer
{
    protected override void Awake()
    {
      
    }
    protected override void Start()
    {
        audiosource=GetComponent<AudioSource>();
        base.Start();
    }
    [Header("휩쓸기시작 사운드")]
    public AudioClip HandSwerapStartclip;
    [Header("휩쓸기시작 볼륨"), Range(0, 1)]
    public float HandSwerapStartVolume;
    public void HandSwerapStartClipPlay()
    {
        if (HandSwerapStartclip != null)
        {
            audiosource.Stop();
            audiosource.clip = HandSwerapStartclip;
            audiosource.volume = HandSwerapStartVolume;
            audiosource.Play();
        }
    }
    [Header("휩쓸기 사운드")]
    public AudioClip HandSwerapEndclip;
    [Header("휩쓸기 볼륨"), Range(0, 1)]
    public float HandSwerapEndVolume;
    public void HandSwerapEndClipPlay()
    {
        if (HandSwerapEndclip != null)
        {
            audiosource.Stop();
            audiosource.clip = HandSwerapEndclip;
            audiosource.volume = HandSwerapEndVolume;
            audiosource.Play();
        }
    }
    [Header("레이저시작 사운드")]
    public AudioClip LazerStartclip;
    [Header("레이저시작 볼륨"), Range(0, 1)]
    public float LazerStartVolume;
    public void LazerStartClipPlay()
    {
        if (LazerStartclip != null)
        {
            audiosource.Stop();
            audiosource.loop = true;
            audiosource.clip = LazerStartclip;
            audiosource.volume = LazerStartVolume;
            audiosource.Play();
        }
    }
    public void LazerStartClipEnd()
    {
        if (LazerStartclip != null)
        {
            audiosource.Stop();
            audiosource.loop = false;
           
        }
    }
    [Header("레이저준비 사운드")]
    public AudioClip Lazerinitclip;
    [Header("레이저준비 볼륨"), Range(0, 1)]
    public float LazerinitVolume;
    public void LazerinitClipPlay()
    {
        if (Lazerinitclip != null)
        {
            audiosource.Stop();
            audiosource.clip = Lazerinitclip;
            audiosource.volume = LazerinitVolume;
            audiosource.Play();
        }
    }
    [Header("물체떨어지기 사운드")]
    public AudioClip ObjectFallingclip;
    [Header("물체떨어지기 볼륨"), Range(0, 1)]
    public float ObjectFallingVolume;
    public void OBjectFallClipPlay()
    {
        if (ObjectFallingclip != null)
        {

            audiosource.Stop();
            audiosource.clip = ObjectFallingclip;
            audiosource.volume = ObjectFallingVolume;
            audiosource.Play();
        }
    }

    [Header("물체땅에닿음 사운드")]
    public AudioClip Objectgroundedclip;
    [Header("물체땅에닿음 볼륨"), Range(0, 1)]
    public float ObjectgroundedVolume;
    public void OBjectlandingClipPlay()
    {
        if (Objectgroundedclip != null)
        {
            audiosource.Stop();
 
            audiosource.clip = Objectgroundedclip;
            audiosource.volume = ObjectgroundedVolume;
            audiosource.Play();
        }
    }
    [Header("모니터 피격 사운드")]
    public AudioClip MonitiorHittedclip;
    [Header("모니터 피격 볼륨"), Range(0, 1)]
    public float MonitiorHittedVolume;
    public void MonitiorHittedClipPlay()
    {
        if (MonitiorHittedclip != null)
        {
            audiosource.Stop();
            audiosource.clip = MonitiorHittedclip;
            audiosource.volume = MonitiorHittedVolume;
            audiosource.Play();
        }
    }
    
  
}

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
    [Header("�۾������ ����")]
    public AudioClip HandSwerapStartclip;
    [Header("�۾������ ����"), Range(0, 1)]
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
    [Header("�۾��� ����")]
    public AudioClip HandSwerapEndclip;
    [Header("�۾��� ����"), Range(0, 1)]
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
    [Header("���������� ����")]
    public AudioClip LazerStartclip;
    [Header("���������� ����"), Range(0, 1)]
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
    [Header("�������غ� ����")]
    public AudioClip Lazerinitclip;
    [Header("�������غ� ����"), Range(0, 1)]
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
    [Header("��ü�������� ����")]
    public AudioClip ObjectFallingclip;
    [Header("��ü�������� ����"), Range(0, 1)]
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

    [Header("��ü�������� ����")]
    public AudioClip Objectgroundedclip;
    [Header("��ü�������� ����"), Range(0, 1)]
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
    [Header("����� �ǰ� ����")]
    public AudioClip MonitiorHittedclip;
    [Header("����� �ǰ� ����"), Range(0, 1)]
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;



public class PlayerSoundPlayer : CharacterSoundPlayer
{
    
  
    [Header("���� ����")]
    public AudioClip JumpClip;
    [Header("���� ���� ����"),Range(0,1)]
    public float JumpVolume;
    [Header("���� ����")]
    public AudioClip LandClip;
    [Header("���� ���� ����"), Range(0, 1)]
    public float LandVolume;
    [Header("��ų ����")]
    public AudioClip SkillClip;
    [Header("��ų ���� ����"), Range(0, 1)]
    public float SkillVolume;
  
    public void PlayLandingSound()
    {
        if (LandClip != null )
        {
            audiosource.Stop();
            audiosource.clip = LandClip;
            audiosource.volume = LandVolume;
            audiosource.Play();
        }
    }
    
    public void PlaySkillSound()
    {
        if (SkillClip != null)
        {
            audiosource.Stop();
            audiosource.clip = SkillClip;
            audiosource.volume = SkillVolume;
            audiosource.Play();
        }
    }


   

  
    public void PlayJumpAudio()
    {
        audiosource.Stop();
        audiosource.clip = JumpClip;
        audiosource.volume = JumpVolume;
        audiosource.Play();
    }
 
}

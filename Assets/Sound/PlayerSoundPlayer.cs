using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;



public class PlayerSoundPlayer : CharacterSoundPlayer
{
    
  
    [Header("점프 사운드")]
    public AudioClip JumpClip;
    [Header("점프 사운드 볼륨"),Range(0,1)]
    public float JumpVolume;
    [Header("착지 사운드")]
    public AudioClip LandClip;
    [Header("착지 사운드 볼륨"), Range(0, 1)]
    public float LandVolume;
    [Header("스킬 사운드")]
    public AudioClip SkillClip;
    [Header("스킬 사운드 볼륨"), Range(0, 1)]
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

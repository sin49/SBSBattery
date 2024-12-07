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
    [Header("내려찍기 빙글 사운드")]
    public AudioClip InitDownAttackClip;
    [Header("내려찍기 빙글 사운드 볼륨"), Range(0, 1)]
    public float InitDownAttackVolume;
    [Header("내려찍기 쾅 사운드")]
    public AudioClip DownAttackEndClip;
    [Header("내려찍기 쾅 사운드 볼륨"), Range(0, 1)]
    public float DownAttackEndVolume;
    [Header("변신 빙글 사운드")]
    public AudioClip InitTransformedClip;
    [Header("변신 빙글 사운드 볼륨"), Range(0, 1)]
    public float InitTransformedVolume;
    [Header("변신 완료 사운드")]
    public AudioClip TransformedEndClip;
    [Header("변신 완료 사운드 볼륨"), Range(0, 1)]
    public float TransformedEndVolume;
   
    [Header("착지 사운드")]
    public AudioClip LandClip;
    [Header("착지 사운드 볼륨"), Range(0, 1)]
    public float LandVolume;
    [Header("스킬 키를 누른 순간 사운드")]
    public AudioClip SkillClip;
    [Header("스킬 사운드 볼륨"), Range(0, 1)]
    public float SkillVolume;
    public void PlayLandingSound()
    {
        if (LandClip != null)
        {
            MakeSeAudioClip(LandClip, LandVolume);
            //Debug.Log("착지 사운드 재생 시도");
            //audiosource.Stop();
            //audiosource.clip = LandClip;
            //audiosource.volume = LandVolume;
            //audiosource.Play();
        }
    }
   
    public void PlayInitDownAttackSound()
    {
        if (InitDownAttackClip != null)
        {
            MakeSeAudioClip(InitDownAttackClip, InitDownAttackVolume);
            //audiosource.Stop();
            //audiosource.clip = InitDownAttackClip;
            //audiosource.volume = InitDownAttackVolume;
            //audiosource.Play();
        }
    }
    public void PlayInitTransformedSound()
    {
        if (InitTransformedClip != null)
        {

            MakeSeAudioClip(InitTransformedClip, InitTransformedVolume);
            //audiosource.Stop();
            //audiosource.clip = InitTransformedClip;
            //audiosource.volume = InitTransformedVolume;
            //audiosource.Play();
        }
    }
    public void PlayTransformedEndSound()
    {
        if (TransformedEndClip != null)
        {
            MakeSeAudioClip(TransformedEndClip, TransformedEndVolume);
            //Debug.Log("변신완료  사운드"+this.gameObject.name);
            //audiosource.Stop();
            //audiosource.clip = TransformedEndClip;
            //audiosource.volume = TransformedEndVolume;
            //audiosource.Play();
        }
    }
    public void PlayDownAttackEndSound()
    {
        
        if (DownAttackEndClip != null)
        {
            MakeSeAudioClip(DownAttackEndClip, DownAttackEndVolume);
            //Debug.Log("내려찍기 완료 사운드 출력시도");
            //audiosource.Stop();
            //audiosource.clip = DownAttackEndClip;
            //audiosource.volume = DownAttackEndVolume;
            //audiosource.Play();
        }
    }
    public void PlaySkillSound()
    {
        if (SkillClip != null)
        {
            MakeSeAudioClip(SkillClip, SkillVolume);
            //audiosource.Stop();
            //audiosource.clip = SkillClip;
            //audiosource.volume = SkillVolume;
            //audiosource.Play();
        }
    }


   

  
    public void PlayJumpAudio()
    {
        MakeSeAudioClip(JumpClip, JumpVolume);
        //audiosource.Stop();
        //audiosource.clip = JumpClip;
        //audiosource.volume = JumpVolume;
        //audiosource.Play();
    }
 
}

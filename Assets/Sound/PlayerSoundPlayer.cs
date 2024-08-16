using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;



public class PlayerSoundPlayer :  SEPlayer
{
    [Header("이동 사운드")]
    public AudioClip MoveClip;
    [Header("이동 사운드 볼륨"), Range(0, 1)]
    public float MoveVolume;
    [Header("캐릭터 사망 사운드")]
    public AudioClip CharacterDieClip;
    [Header("캐릭터 사망 사운드 볼륨"), Range(0, 1)]
    public float CharacterDieVolume;
    [Header("공격 사운드")]
    public AudioClip AttackClip;
    [Header("공격 사운드 볼륨"),Range(0,1)]
    public float AttackVolume;
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
    public void PlayCharacterDieClip()
    {
        if (CharacterDieClip != null)
        {
            audiosource.Stop();
            audiosource.clip = CharacterDieClip;
            audiosource.volume = CharacterDieVolume;
            audiosource.Play();
        }
    }
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
    public void PlayMoveSound()
    {
        if (MoveClip != null&&!audiosource.isPlaying)
        {
            audiosource.Stop();
            audiosource.clip = MoveClip;
            audiosource.volume = MoveVolume;
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


   

    public void PlayAttackAudio()
    {
        if (AttackClip != null)
        {
            audiosource.Stop();
            audiosource.clip = AttackClip;
            audiosource.volume = AttackVolume;
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

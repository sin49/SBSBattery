using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;



public class PlayerSoundPlayer :  SEPlayer
{
    [Header("�̵� ����")]
    public AudioClip MoveClip;
    [Header("�̵� ���� ����"), Range(0, 1)]
    public float MoveVolume;
    [Header("ĳ���� ��� ����")]
    public AudioClip CharacterDieClip;
    [Header("ĳ���� ��� ���� ����"), Range(0, 1)]
    public float CharacterDieVolume;
    [Header("���� ����")]
    public AudioClip AttackClip;
    [Header("���� ���� ����"),Range(0,1)]
    public float AttackVolume;
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

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
    [Header("������� ���� ����")]
    public AudioClip InitDownAttackClip;
    [Header("������� ���� ���� ����"), Range(0, 1)]
    public float InitDownAttackVolume;
    [Header("������� �� ����")]
    public AudioClip DownAttackEndClip;
    [Header("������� �� ���� ����"), Range(0, 1)]
    public float DownAttackEndVolume;
    [Header("���� ���� ����")]
    public AudioClip InitTransformedClip;
    [Header("���� ���� ���� ����"), Range(0, 1)]
    public float InitTransformedVolume;
    [Header("���� �Ϸ� ����")]
    public AudioClip TransformedEndClip;
    [Header("���� �Ϸ� ���� ����"), Range(0, 1)]
    public float TransformedEndVolume;
    [Header("�ǰ� ����")]
    public AudioClip HittedClip;
    [Header("�ǰ� ���� ����"), Range(0, 1)]
    public float HittedVolume;

    [Header("���� ����")]
    public AudioClip LandClip;
    [Header("���� ���� ����"), Range(0, 1)]
    public float LandVolume;
    [Header("��ų Ű�� ���� ���� ����")]
    public AudioClip SkillClip;
    [Header("��ų ���� ����"), Range(0, 1)]
    public float SkillVolume;
    public void PlayLandingSound()
    {
        if (LandClip != null)
        {
            audiosource.Stop();
            audiosource.clip = LandClip;
            audiosource.volume = LandVolume;
            audiosource.Play();
        }
    }
    public void PlayHittedSound()
    {
        if (HittedClip != null )
        {
            audiosource.Stop();
            audiosource.clip = HittedClip;
            audiosource.volume = HittedVolume;
            audiosource.Play();
        }
    }
    public void PlayInitDownAttackSound()
    {
        if (InitDownAttackClip != null)
        {
            audiosource.Stop();
            audiosource.clip = InitDownAttackClip;
            audiosource.volume = InitDownAttackVolume;
            audiosource.Play();
        }
    }
    public void PlayInitTransformedSound()
    {
        if (InitTransformedClip != null)
        {
            audiosource.Stop();
            audiosource.clip = InitTransformedClip;
            audiosource.volume = InitTransformedVolume;
            audiosource.Play();
        }
    }
    public void PlayTransformedEndSound()
    {
        if (TransformedEndClip != null)
        {
            audiosource.Stop();
            audiosource.clip = TransformedEndClip;
            audiosource.volume = TransformedEndVolume;
            audiosource.Play();
        }
    }
    public void PlayDownAttackEndSound()
    {
        
        if (DownAttackEndClip != null)
        {
            Debug.Log("������� �Ϸ� ���� ��½õ�");
            audiosource.Stop();
            audiosource.clip = DownAttackEndClip;
            audiosource.volume = DownAttackEndVolume;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundPlayer : SEPlayer
{
    [Header("�̵� ����")]
    public AudioClip MoveClip;
    [Header("�̵� ���� ����"), Range(0, 1)]
    public float MoveVolume;

    [Header("���� ����")]
    public AudioClip AttackClip;
    [Header("���� ���� ����"), Range(0, 1)]
    public float AttackVolume;
   
    public void PlayMoveSound()
    {
        if (MoveClip != null && !audiosource.isPlaying)
        {
            audiosource.Stop();
            audiosource.clip = MoveClip;
            audiosource.volume = MoveVolume;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundPlayer : SEPlayer
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
    [Header("공격 사운드 볼륨"), Range(0, 1)]
    public float AttackVolume;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeadSoundPlayer : SEPlayer
{
    [Header("캐릭터 사망 사운드")]
    public AudioClip CharacterDieClip;
    [Header("캐릭터 사망 사운드 볼륨"), Range(0, 1)]
    public float CharacterDieVolume;

    protected override void Start()
    {
        base.Start();
        PlayCharacterDieClip();
    }
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeadSoundPlayer : SEPlayer
{
    [Header("ĳ���� ��� ����")]
    public AudioClip CharacterDieClip;
    [Header("ĳ���� ��� ���� ����"), Range(0, 1)]
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

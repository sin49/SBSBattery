using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemSoundPlayer : SEPlayer
{

    [Header("플레이어 사망 사운드")]
    public AudioClip CharacterDieClip;
    [Header("플레이어 사망 사운드 볼륨"), Range(0, 1)]
    public float CharacterDieVolume;
    [Header("차원 전환 사운드")]
    public AudioClip ChangeDimensionClip;
    [Header("차원 전환 볼륨"), Range(0, 1)]
    public float ChangeDimensionVolume;
    [Header("아이템 흭득 사운드")]
    public AudioClip GetItemClip;
    [Header("아이템 흭득 볼륨"), Range(0, 1)]
    public float GetItemVolume; 
    [Header("체력 회복 사운드")]
    public AudioClip RecoverHPClip;
    [Header("체력 회복 볼륨"), Range(0, 1)]
    public float RecoverHPVolume;
    protected override void Start()
    {
        base.Start();
        PlayerHandler.instance.registerchangedimentiosnsfxEvent(PlayChangeDimensionSound);
        PlayerHandler.instance.PlayerDeathEvent += PlayCharacterDieClip;
        PlayerInventory.instance.registerItemGetAction(PlayGetItemSound);
        PlayerStat.instance.registerRecoverAction(PlayRecoverHPSound);
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
    public void PlayChangeDimensionSound()
    {
        if (ChangeDimensionClip != null)
        {
            audiosource.Stop();
            audiosource.clip = ChangeDimensionClip;
            audiosource.volume = ChangeDimensionVolume;
            audiosource.Play();
        }
    }
    public void PlayGetItemSound()
    {
        if (GetItemClip != null)
        {
            audiosource.Stop();
            audiosource.clip = GetItemClip;
            audiosource.volume = GetItemVolume;
            audiosource.Play();
        }
    }
    public void PlayRecoverHPSound()
    {
        if (RecoverHPClip != null)
        {
            audiosource.Stop();
            audiosource.clip = RecoverHPClip;
            audiosource.volume = RecoverHPVolume;
            audiosource.Play();
        }
    }
}

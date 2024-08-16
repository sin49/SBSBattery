using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemSoundPlayer : SEPlayer
{
    
   
    [Header("���� ��ȯ ����")]
    public AudioClip ChangeDimensionClip;
    [Header("���� ��ȯ ����"), Range(0, 1)]
    public float ChangeDimensionVolume;
    [Header("������ ŉ�� ����")]
    public AudioClip GetItemClip;
    [Header("������ ŉ�� ����"), Range(0, 1)]
    public float GetItemVolume; 
    [Header("ü�� ȸ�� ����")]
    public AudioClip RecoverHPClip;
    [Header("ü�� ȸ�� ����"), Range(0, 1)]
    public float RecoverHPVolume;
    private void Start()
    {
        PlayerHandler.instance.registerCameraChangeAction(PlayChangeDimensionSound);
        PlayerInventory.instance.registerItemGetAction(PlayGetItemSound);
        PlayerStat.instance.registerRecoverAction(PlayRecoverHPSound);
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

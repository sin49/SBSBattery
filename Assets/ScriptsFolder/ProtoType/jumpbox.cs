using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpbox :EnvironmentspeedChanger
{
    [Header("리스트 0번: 닿아서 튀는 순간 나는 소리\n(중복해서 소리가 나는 오류가" +
        " 있을 수 있으니 발생하면 알려주샘)")]
    public SoundEffectListPlayer soundEffectListPlayer;
    private void Awake()
    {
        soundEffectListPlayer = GetComponent<SoundEffectListPlayer>();
    }

    protected override void changevector(environmentObject obj)
    {
        if (soundEffectListPlayer != null)
            soundEffectListPlayer.PlayAudio(0);
        base.changevector(obj);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpbox :EnvironmentspeedChanger
{
    [Header("����Ʈ 0��: ��Ƽ� Ƣ�� ���� ���� �Ҹ�\n(�ߺ��ؼ� �Ҹ��� ���� ������" +
        " ���� �� ������ �߻��ϸ� �˷��ֻ�)")]
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

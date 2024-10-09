using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpbox :EnvironmentspeedChanger
{
    [Header("����Ʈ 0��: ��Ƽ� Ƣ�� ���� ���� �Ҹ�\n(�ߺ��ؼ� �Ҹ��� ���� ������" +
        " ���� �� ������ �߻��ϸ� �˷��ֻ�)")]
    public SoundEffectListPlayer soundEffectListPlayer;
    public GameObject jumpBoxEffect;
    private void Awake()
    {
        soundEffectListPlayer = GetComponent<SoundEffectListPlayer>();
        jumpBoxEffect.SetActive(false);
    }

    protected override void changevector(environmentObject obj)
    {
        if (soundEffectListPlayer != null)
            soundEffectListPlayer.PlayAudio(0);
        StartCoroutine(ShowJumpBoxEffect());
        base.changevector(obj);
    }

    IEnumerator ShowJumpBoxEffect()
    {
        jumpBoxEffect.SetActive(true);

        yield return new WaitForSeconds(1f);

        jumpBoxEffect.SetActive(false);
    }
}

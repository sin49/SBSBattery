using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakSoundPlay : MonoBehaviour
{
    [Header("ȿ���� ����Ʈ 0��: �ı� ���� ��")]
    public SoundEffectListPlayer SoundEffectListPlayer;
    private void Awake()
    {
        SoundEffectListPlayer = GetComponent<SoundEffectListPlayer>();
    }
    private void Start()
    {
        if (SoundEffectListPlayer != null)
            SoundEffectListPlayer.PlayAudio(0);
    }
   
}

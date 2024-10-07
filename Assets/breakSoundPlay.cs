using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakSoundPlay : MonoBehaviour
{
    [Header("효과음 리스트 0번: 파괴 됐을 때")]
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

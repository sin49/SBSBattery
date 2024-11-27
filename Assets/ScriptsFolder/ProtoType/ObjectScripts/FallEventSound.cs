using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEventSound : MonoBehaviour
{
    public SoundEffectListPlayer soundEffectListPlayer;

    private void Awake()
    {
        soundEffectListPlayer = GetComponent<SoundEffectListPlayer>();
    }

    
}

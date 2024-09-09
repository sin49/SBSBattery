using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    [Header("한 번만 상호작용")]
    public bool InteractOnce;
    [Header("상호작용 가능 여부")]
    public bool CanInteract = true;
    protected direction direct;
    //public InteractOption InteractOption;
    [Header("0번 활성화 소리")]
    public SoundEffectListPlayer soundEffectListPlayer;
    protected virtual void Awake()
    {
        if(soundEffectListPlayer!=null)
        soundEffectListPlayer.GetComponent<SoundEffectListPlayer>();
    }
    public virtual void Active(direction direct)
    {
        if(soundEffectListPlayer!=null)
        soundEffectListPlayer.PlayAudio(0);
        if (InteractOnce)
            CanInteract = false;
    }

    }
public enum InteractOption {ray,collider }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    [Header("�� ���� ��ȣ�ۿ�")]
    public bool InteractOnce;
    [Header("��ȣ�ۿ� ���� ����")]
    public bool CanInteract = true;
    protected direction direct;
    //public InteractOption InteractOption;
    [Header("0�� Ȱ��ȭ �Ҹ�")]
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
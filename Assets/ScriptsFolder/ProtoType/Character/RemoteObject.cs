using UnityEngine;

public abstract class RemoteObject : MonoBehaviour
{
    [Header("0�� Ȱ��ȭ �Ҹ� 1�� ��Ȱ��ȭ �Ҹ�")]
    public SoundEffectListPlayer soundEffectListPlayer;
    public bool onActive;
    public bool activeonce;
    public bool CanControl = true;
    //UIǥ�� ��ġ
    public GameObject HudTarget;

    protected virtual void Awake()
    {
        soundEffectListPlayer=GetComponent<SoundEffectListPlayer>();
    }
    public virtual void Active()
    {
        if(soundEffectListPlayer!=null)
        soundEffectListPlayer.PlayAudio(0);
        if (activeonce)
            CanControl = false;
    }

    public virtual void Deactive()
    {
        if(soundEffectListPlayer !=null)
        soundEffectListPlayer.PlayAudio(1);
        if (activeonce)
            CanControl = false;
    }


}
using UnityEngine;

public abstract class RemoteObject : MonoBehaviour
{
    [Header("0번 활성화 소리 1번 비활성화 소리")]
    public SoundEffectListPlayer soundEffectListPlayer;
    public bool onActive;
    public bool activeonce;
    public bool CanControl = true;
    //UI표시 위치
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
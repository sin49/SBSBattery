using UnityEngine;

public abstract class RemoteObject : MonoBehaviour
{
    [Header("0번 활성화 소리 1번 비활성화 소리")]
    public SoundEffectListPlayer soundEffectListPlayer;
    public bool onActive;
    public bool CanControl = true;
    //UI표시 위치
    public GameObject HudTarget;

    protected virtual void Awake()
    {
        soundEffectListPlayer=GetComponent<SoundEffectListPlayer>();
    }
    public virtual void Active()
    {
        soundEffectListPlayer.PlayAudio(0);
    }

    public virtual void Deactive()
    {
        if(soundEffectListPlayer !=null)
        soundEffectListPlayer.PlayAudio(1);
    }


}
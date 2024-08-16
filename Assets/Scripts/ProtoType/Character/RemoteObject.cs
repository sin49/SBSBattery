using UnityEngine;

public abstract class RemoteObject : MonoBehaviour
{
    [Header("0�� Ȱ��ȭ �Ҹ� 1�� ��Ȱ��ȭ �Ҹ�")]
    public SoundEffectListPlayer soundEffectListPlayer;
    public bool onActive;
    public bool CanControl = true;
    //UIǥ�� ��ġ
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
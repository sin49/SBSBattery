using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class signalSender : MonoBehaviour
{
    [Header("오디오 0번:활성화 오디오 1번:비활성화")]
    public bool active;
    public SoundEffectListPlayer sound;
  protected  List< signalReceiver> Receiver= new List<signalReceiver>();

  protected  int signalnumber;
    protected virtual void Awake()
    {
        sound = GetComponent<SoundEffectListPlayer>();
    }
    public abstract void register(signalReceiver receiver, int index);
    public virtual void Send(bool signal)
    {
        if (sound != null)
        {
            if (signal)
                sound.PlayAudio(0);
            else
                sound.PlayAudio(1);
        }
        if (Receiver .Count!=0)
        {
            foreach(var a in Receiver)
            {
                a.Receive(signal, signalnumber);
            }
        }
        
    }

}

public abstract class signalReceiver : MonoBehaviour
{
    [Header("0번 활성화 소리 1번 비활성화 소리")]
    public SoundEffectListPlayer soundPlayer;
    public List<signalSender> signalSenders;
    //bool[] signals;
    public bool active;
    protected virtual void Awake()
    {
        soundPlayer = GetComponent<SoundEffectListPlayer>();
    }
    public virtual void CheckSignal()
    {
        bool tmp = active;
        bool chk = true;
        foreach (signalSender a in signalSenders)
        {
            chk &= a.active;
        }

        active = chk;
        if (active != tmp&&soundPlayer!=null)
        {
            if (active)
                soundPlayer.PlayAudio(0);
            else
                soundPlayer.PlayAudio(1);
        }

    }
    public void register()
    {
        for (int i = 0; i < signalSenders.Count; i++)
        {
            if (signalSenders[i] != null)
                signalSenders[i].register(this, i);
        }

    }
    public void Receive(bool signal, int signalnumber)
    {
        CheckSignal();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayEvent : OutputEvent
{
    public SoundEffectListPlayer soundplayer;
    public int index;
    public override void output()
    {
        base.output();
        soundplayer.PlayAudio(index);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBubbleMaker : InteractiveObject
{
    public TextAsset textasset_;
    public override void Active(direction direct)
    {
        base.Active(direct);
        TextBubbleManaager.instance.PlayTextBubble(transform, textasset_);
    }
}

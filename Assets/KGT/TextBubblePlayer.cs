using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TextBubblePlayer : InteractiveObject
{
    [Header("재생할 말풍선 종류")]
    public TextBubble textbubble;
    [Header("재생할 대본 입력")]
    public UnityEngine.TextAsset textasset_;

 
    public override void Active(direction direct)
    {
        base.Active(direct);
        textbubble.gameObject.SetActive(true);
        textbubble.textasset = textasset_;
        textbubble.target = this.transform;
        textbubble.Play();
    }

}

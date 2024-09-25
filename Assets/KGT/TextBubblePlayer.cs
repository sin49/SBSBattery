using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TextBubblePlayer : InteractiveObject
{
    [Header("����� ��ǳ�� ����")]
    public TextBubble textbubble;
    [Header("����� �뺻 �Է�")]
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

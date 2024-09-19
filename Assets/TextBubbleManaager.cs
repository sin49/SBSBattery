using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBubbleManaager : MonoBehaviour
{
    public TextBubble TextBubble;
    public Transform TextBubbleTarget;
    public static TextBubbleManaager instance;
    private void Awake()
    {
        instance = this;
        TextBubble.gameObject.SetActive(false);
    }
    public void PlayTextBubble(Transform target,TextAsset textasset)
    {
     
        TextBubble.gameObject.SetActive(true);
        TextBubble.textasset = textasset;
        TextBubble.target = target;
        TextBubble.Play();
    }

  
    void Update()
    {
        
    }
}

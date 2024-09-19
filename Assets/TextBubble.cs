using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextBubble : MonoBehaviour
{




    [Header("파라미터")]
    [Header("말풍선 속도")]
    public float textplayspeed = 1;
    [Header("말풍선 넓이 증가 폭")]
    public float bubblewidthadd = 35;

    [Header("말풍선 최대 글자 길이(넓이) 수")]
    public float textmaxslashnumber = 30;

    [Header("말풍선 높이 증가 폭")]
    public float bubbleheightadd = 60;

    [Header("여기서 출력 예정인 대사를 확인 가능")]
    public string[] texts;

    [Header("이 밑으로 건들지 말 것")]
    public TextMeshProUGUI textfield;
    public RectTransform textbubbletransform;

    public Transform target;

    public TextAsset textasset;
   protected string playingtext;


    //public string text;

    public virtual void Play()
    {
        readtextasset(textasset);
   
    }

   protected void calculatetextbubblesize()
    {
        string[] lines = textfield.text.Split('\n');
        int maxLineLength = 0;
        foreach (string line in lines)
        {
            if (line.Length > maxLineLength)
            {
                maxLineLength = line.Length;
            }
        }

        float bubblewidth;

        bubblewidth = Mathf.Clamp(maxLineLength * bubblewidthadd, 0, bubblewidthadd * textmaxslashnumber);

        float bubbleheight = bubbleheightadd * lines.Length;

        Vector2 bubblesize = new Vector2(bubblewidth, bubbleheight);


        textbubbletransform.sizeDelta = bubblesize;
    }

    void readtextasset(TextAsset asset)//수정 후
    {
        string s = textasset.text;
        texts = s.Split('|');

    }
  
   

  protected virtual  void Update()
    {


        if (target != null)
            transform.position = PlayerHandler.instance.CurrentCamera.WorldToScreenPoint(
          target.position + Vector3.up);
    }
}

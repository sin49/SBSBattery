using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextBubble : MonoBehaviour
{




    [Header("�Ķ����")]
    [Header("��ǳ�� �ӵ�")]
    public float textplayspeed = 1;
    [Header("��ǳ�� ���� ���� ��")]
    public float bubblewidthadd = 35;

    [Header("��ǳ�� �ִ� ���� ����(����) ��")]
    public float textmaxslashnumber = 30;

    [Header("��ǳ�� ���� ���� ��")]
    public float bubbleheightadd = 60;

    [Header("���⼭ ��� ������ ��縦 Ȯ�� ����")]
    public string[] texts;

    [Header("�� ������ �ǵ��� �� ��")]
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

    void readtextasset(TextAsset asset)//���� ��
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

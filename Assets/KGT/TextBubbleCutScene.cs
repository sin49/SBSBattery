using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBubbleCutScene : TextBubble
{




  
    bool playing;
    bool skipped;

    //public string text;

    public override void Play()
    {
        base.Play();
        StartCoroutine(playtextbubbleCutScene(texts));
    }





 
    IEnumerator playtextbubbleCutScene(string[] textlist)
    {
        PlayerHandler.instance.CantHandle = true;
        //���� ��� �ֱ�
        for (int n = 0; n < textlist.Length; n++)
        {
            textfield.text = "";
            playingtext = textlist[n];
            playing = true;
            for (int i = 0; i < playingtext.Length; i++)
            {

                if (skipped)
                {

                    textfield.text = playingtext;
                    calculatetextbubblesize();
                    skipped = false;
                    break;
                }
                else
                {
                    textfield.text += playingtext[i];
                    calculatetextbubblesize();

                    yield return new WaitForSeconds(1 / textplayspeed);
                }
            }
            playing = false;
            yield return new WaitUntil(() => { return checkkeyinputcheker; });
            checkkeyinputcheker = false;
        }
        this.gameObject.SetActive(false);
        PlayerHandler.instance.CantHandle = false;
        //���� ��� ����

    }
    public float CheckKeyinputdelay = 0.2f;
    float CheckKeyinputtimer;
    bool checkkeyinputcheker;
    protected override void Update()
    {
        if (CheckKeyinputtimer > 0)
            CheckKeyinputtimer -= Time.deltaTime;
        else
        {
            if (Input.GetKeyDown(KeySettingManager.instance.AttackKeycode))//Ű�� ������ ��ǳ���� text�� ��µǴ� ���ϸ��̼� ��ŵ
            {
                CheckKeyinputtimer = CheckKeyinputdelay;
                if (playing)
                {


                    skipped = true;
                }
                else
                {
                    checkkeyinputcheker = true;
                }
            }
        }

        base.Update();
    }
  
   
}

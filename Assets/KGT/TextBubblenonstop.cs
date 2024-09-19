using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TextBubblenonstop : TextBubble
{


    public override void Play()
    {
        base.Play();
        StartCoroutine(playtextbubblenonstop(texts));
    }

  






   
   
    IEnumerator playtextbubblenonstop(string[] textlist)
    {
        PlayerHandler.instance.CantHandle = true;
        for (int n = 0; n < textlist.Length; n++)
        {
            textfield.text = "";
            playingtext = textlist[n];

            for (int i = 0; i < playingtext.Length; i++)
            {


                textfield.text += playingtext[i];
                calculatetextbubblesize();

                yield return new WaitForSeconds(1 / textplayspeed);

            }
            yield return new WaitForSeconds(2.5f);
        }
        this.gameObject.SetActive(false);
        PlayerHandler.instance.CantHandle = false;

    }

   
}

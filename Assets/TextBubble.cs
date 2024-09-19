using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
//컷신용 말풍선(조작 불가와 스킵 기능 제거,자동 넘어가기 추가해서 그냥 말풍선도 반들것)
public class TextBubble : MonoBehaviour
{
    [Header("임시로 숫자 0번키로 확인/스킵 가능")]
    [Header("지금은 한번에 말풍선 하나 밖에 안됨(수정하고 싶으면 말할 것)")]
    [Header("이 말풍선같은 경우 시작하면 조작 불가 상태가 된다")]
    [Header(" 추가 예정)대사 다 되면 확인 표시 뜨기,조작 불가 없이 자동으로 넘어가는 말풍선" +
        ",텍스트 쪽에 표시해서 줄바꾸기")]



    [Header("파라미터")]
    [Header("말풍선 속도")]
    public float textplayspeed = 1;
    [Header("말풍선 넓이 증가 폭")]
    public float bubblewidthadd = 35;
    [Header("말풍선 줄바꾸는 단위")]
    public float textslashnumber = 10;
    [Header("말풍선 높이 증가 폭")]
    public float bubbleheightadd = 60;

    [Header("여기서 출력 예정인 대사를 확인 가능")]
    public List<string> textlist = new List<string>();

    [Header("이 밑으로 건들지 말 것")]
    public TextMeshProUGUI textfield;
    public RectTransform textbubbletransform;

public    Transform target;

    public TextAsset textasset;
    string playingtext;
    bool playing;
    bool skipped;
    
    //public string text;
    
    public void Play()
    {
        readtextasset(textasset);
        //textasset을 분해
        ////
        StartCoroutine(playtextbubble(textlist));
    }
 
    public void readtextasset(TextAsset asset)
    {
        string s = textasset.text;
        List<int> textcount = new List<int>();
        textcount.Add(0);
        int number = 0;
        while (true)
        {
            int slashindex = s.IndexOf("\\", number);
            if (slashindex != -1)
            {
                textcount.Add(slashindex);
                number = slashindex + 1;
            }
            else
            {
                break;
            }
        }
        textcount.Add(s.Length);
        for(int n = 0; n < textcount.Count-1; n++)
        {
            
            string text_ = s.Substring(textcount[n], (textcount[n + 1] - textcount[n]))
                .Replace("\\","")
                    .Replace("\n", "")
                        .Replace("\r", ""); ;
     
         
           textlist.Add(text_);
        }
    }
    IEnumerator playtextbubble(List<string> textlist)
    {
        PlayerHandler.instance.CantHandle = true;
        for (int n = 0; n < textlist.Count; n++)
        {
            textfield.text = "";
            playingtext = textlist[n];
            playing = true;
            for (int i = 0; i < playingtext.Length; i++)
            {
                if (skipped)
                {
                    string fullText = "";
                    for (int i2 = 0; i2 < playingtext.Length; i2++)
                    {
                        fullText += playingtext[i2];
                        if ((i2 + 1) % textslashnumber == 0)
                        {
                            fullText += "\n";
                        }
                    }
                    textfield.text = fullText;
                    skipped = false;
                }
                else
                {
                    textfield.text += playingtext[i];
                    if (i > 0 && i % textslashnumber == 0)
                    {
                        textfield.text += "\n";
                    }

                    yield return new WaitForSeconds(1 / textplayspeed);
                }
            }
            playing = false;
            yield return new WaitUntil(() => { return checkkeyinputcheker; });
            checkkeyinputcheker = false;
        }
        this.gameObject.SetActive(false);
        PlayerHandler.instance.CantHandle = false;

    }
    public float CheckKeyinputdelay = 0.2f;
    float CheckKeyinputtimer;
    bool checkkeyinputcheker;
    void Update()
    {
        float bubblewidth = Mathf.Clamp(textfield.text.Length * bubblewidthadd, 0, bubblewidthadd * (textslashnumber+1));
        float bubbleheight = bubbleheightadd + (int)(textfield.text.Length / (textslashnumber + 1)) * bubbleheightadd;
        Vector2 bubblesize = new Vector2(bubblewidth, bubbleheight);
        textbubbletransform.sizeDelta = bubblesize;
        if (CheckKeyinputtimer > 0)
            CheckKeyinputtimer -= Time.deltaTime;
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))//키를 눌려서 말풍선에 text가 출력되는 에니메이션 스킵
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
            
        
       if(target!=null)
            transform.position = PlayerHandler.instance.CurrentCamera.WorldToScreenPoint(
          target.position + Vector3.up);
    }
}

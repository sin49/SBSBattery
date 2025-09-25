using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public struct textStruct
{
    public string text;
    public float time;

    public textStruct(string text, float time)
    {
        this.text = text;
        this.time = time;
    }
}

public struct DescStruct
{
    public string kor;
    public string eng;

    public DescStruct(string k, string e)
    {
        kor = k;
        eng = e;
    }
}

public class JustTest : MonoBehaviour
{
    public TextAsset descCSV;

    public TextMeshProUGUI t;

    
    public Dictionary<int, textStruct> textList;

    public List<DescStruct> descGroup;
    int descIndex;

    private void Awake()
    {
        descGroup = new List<DescStruct>();
        StringReader stringRead = new StringReader(descCSV.text);

        bool firstLine = true;

        while (true)
        {
            string readLine = stringRead.ReadLine();

            if (readLine == null || string.IsNullOrEmpty(readLine)) break;

            if (firstLine)
            {
                firstLine = false;
                continue;
            }

            string[] value = readLine.Split(",");
            InitLanguageGroup(value);
        }
    }

    private void Start()
    {
        
    }

    #region  Å×½ºÆ®

    // Start is called before the first frame update
    //void Start()
    //{
    //    textList = new Dictionary<int, textStruct>();
    //    textList.Add(0, new textStruct("a", 1));
    //    textList.Add(1, new textStruct("b", 1));
    //    textList.Add(2, new textStruct("c", 1));
    //    textList.Add(3, new textStruct("d", 1));

    //    StartCoroutine(textTimer(textList[index].text, textList[index].time));
    //}
    //int index; 

    //private void Update()
    //{
    //   if (timeOut && index < textList.Count)
    //    {
    //        index++;
    //        StartCoroutine(textTimer(textList[index].text, textList[index].time));
    //    }
    //}
    //bool timeOut;

    //IEnumerator textTimer(string text, float time)
    //{
    //    timeOut = false;

    //    t.text = text;

    //    yield return new WaitForSeconds(time);
    //    timeOut = true;
    //}
    #endregion

    public void InitLanguageGroup(string[] value)
    {
        descGroup.Add(new DescStruct(value[1], value[2]));
    }

    public void NextDescription()
    {
        string str;
        if (descIndex >= descGroup.Count)
        {
            Debug.Log("Nope!");
        }
        else
        {
            if (LanguageManager.instance.isKor)
            {


                str = descGroup[descIndex].kor.Replace("`", ",");
                str = str.Replace("|", "\n");
                t.text = str;
            }
            else
            {
                str = descGroup[descIndex].eng.Replace("`", ",");
                str = str.Replace("|", "\n");
                t.text = str;
            }
            descIndex++;
        }
    }
}

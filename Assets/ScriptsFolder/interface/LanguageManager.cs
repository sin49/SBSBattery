using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    public TextAsset languageCSV;

    [Header("타이틀")]
    public int titleIndex;
    public int titleEndindex;
    List<string> titleKor = new List<string>();
    List<string> titleEng = new List<string>();
    [Header("재확인")]
    public int recheckIndex;
    public int recheckEndindex;
    List<string> recheckKor = new List<string>();
    List<string> recheckEng = new List<string>();
    [Header("로딩")]
    public int loadingIndex;
    public int loadingEndindex;
    List<string> loadingKor = new List<string>();
    List<string> loadingEng = new List<string>();
    [Header("일시정지")]
    public int pauseIndex;
    public int pauseEndindex;
    List<string> pauseKor = new List<string>();
    List<string> pauseEng = new List<string>();
    [Header("소리")]
    public int soundIndex;
    public int soundEndindex;
    List<string> soundKor = new List<string>();
    List<string> soundEng = new List<string>();
    [Header("언어 설정 상태")]
    public bool isKor;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        ReadFileCSV();
    }

    public void ReadFileCSV()
    {
        StringReader stringRead = new StringReader(languageCSV.text);

        bool firstLine = true;

        while (true)
        {
            string readLine =  stringRead.ReadLine();

            if (firstLine)
            {
                firstLine = false;
                continue;
            }

            string[] value = readLine.Split(",");


        }
    }
}

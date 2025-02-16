using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    public TextAsset languageCSV;

    #region 언어팩 변수
    [Header("타이틀")]
    public int titleIndex;
    public int titleEndindex;
    [HideInInspector] public List<string> titleKor = new List<string>();
    [HideInInspector] public List<string> titleEng = new List<string>();
    [HideInInspector] public List<float> titleSpacingKor = new List<float>();
    [HideInInspector] public List<float> titleSpacingEng = new List<float>();

    [Header("재확인")]
    public int recheckIndex;
    public int recheckEndindex;
    [HideInInspector] public List<string> recheckKor = new List<string>();
    [HideInInspector] public List<string> recheckEng = new List<string>();
    [HideInInspector] public List<float> recheckSpacingKor = new List<float>();
    [HideInInspector] public List<float> recheckSpacingEng = new List<float>();

    [Header("로딩")]
    public int loadingIndex;
    public int loadingEndindex;
    [HideInInspector] public List<string> loadingKor = new List<string>();
    [HideInInspector] public List<string> loadingEng = new List<string>();
    [HideInInspector] public List<float> loadingSpacingKor = new List<float>();
    [HideInInspector] public List<float> loadingSpacingEng = new List<float>();

    [Header("일시정지")]
    public int pauseIndex;
    public int pauseEndindex;
    [HideInInspector] public List<string> pauseKor = new List<string>();
    [HideInInspector] public List<string> pauseEng = new List<string>();
    [HideInInspector] public List<float> pauseSpacingKor = new List<float>();
    [HideInInspector] public List<float> pauseSpacingEng = new List<float>();

    [Header("설정")]
    public int settingIndex;
    public int settingEndindex;
    [HideInInspector] public List<string> settingKor = new List<string>();
    [HideInInspector] public List<string> settingEng = new List<string>();
    [HideInInspector] public List<float> settingSpacingKor = new List<float>();
    [HideInInspector] public List<float> settingSpacingEng = new List<float>();

    [Header("소리")]
    public int soundIndex;
    public int soundEndindex;
    [HideInInspector] public List<string> soundKor = new List<string>();
    [HideInInspector] public List<string> soundEng = new List<string>();
    [HideInInspector] public List<float> soundSpacingKor = new List<float>();
    [HideInInspector] public List<float> soundSpacingEng = new List<float>();


    [Header("그래픽")]
    public int graphicIndex;
    public int graphicEndindex;
    [HideInInspector] public List<string> graphicKor = new List<string>();
    [HideInInspector] public List<string> graphicEng = new List<string>();
    [HideInInspector] public List<float> graphicSpacingKor = new List<float>();
    [HideInInspector] public List<float> graphicSpacingEng = new List<float>();

    [Header("게임오버")]
    public int gameoverIndex;
    public int gameoverEndindex;
    [HideInInspector] public List<string> gameoverKor = new List<string>();
    [HideInInspector] public List<string> gameoverEng = new List<string>();
    [HideInInspector] public List<float> gameoverSpacingKor = new List<float>();
    [HideInInspector] public List<float> gameoverSpacingEng = new List<float>();

    [Header("언어")]
    public int languageIndex;
    public int languageEndindex;
    [HideInInspector] public List<string> languageKor = new List<string>();
    [HideInInspector] public List<string> languageEng = new List<string>();
    [HideInInspector] public List<int> languageSpacingKor = new List<int>();
    [HideInInspector] public List<int> languageSpacingEng = new List<int>();

    [Header("키 설정")]
    public int keySetIndex;
    public int keySetEndindex;
    [HideInInspector] public List<string> keySetKor = new List<string>();
    [HideInInspector] public List<string> keySetEng = new List<string>();
    [HideInInspector] public List<float> keysetSpacingKor = new List<float>();
    [HideInInspector] public List<float> keysetSpacingEng = new List<float>();

    [Header("패드 설정")]
    public int padSetIndex;
    public int padSetEndIndex;
    [HideInInspector] public List<string> padSetKor = new List<string>();
    [HideInInspector] public List<string> padSetEng = new List<string>();
    [HideInInspector] public List<float> padsetSpacingKor = new List<float>();
    [HideInInspector] public List<float> padsetSpacingEng = new List<float>();

    [Header("체크포인트")]
    public int checkPointIndex;
    public int checkPointEndindex;
    [HideInInspector] public List<string> cpKor = new List<string>();
    [HideInInspector] public List<string> cpEng = new List<string>();
    [HideInInspector] public List<float> cpSpacingKor = new List<float>();
    [HideInInspector] public List<float> cpSpacingEng = new List<float>();

    [Header("모바일 터치")]
    public int touchIndex;
    public int touchEndIndex;
    [HideInInspector]public List<string> touchKor = new List<string>();
    [HideInInspector] public List<string> touchEng = new List<string>();
    [HideInInspector] public List<float> touchSpacingKor = new List<float>();
    [HideInInspector] public List<float> touchSpacingEng = new List<float>();

    [Header("설정 저장")]
    public int setSaveIndex;
    public int setSaveEndIndex;
    [HideInInspector] public List<string> setSaveKor = new List<string>();
    [HideInInspector] public List<string> setSaveEng = new List<string>();
    [HideInInspector] public List<float> setSaveSpacingKor = new List<float>();
    [HideInInspector] public List<float> setSaveSpacingEng = new List<float>();

    [Header("언어 설정 상태")]
    public bool isKor;

    int saveIndex;
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;

        ReadFileCSV();
    }

    private void Start()
    {
        LangEventCall();
    }

    public void ReadFileCSV()
    {
        StringReader stringRead = new StringReader(languageCSV.text);

        bool firstLine = true;

        while (true)
        {
            string readLine =  stringRead.ReadLine();

            if (readLine == null || string.IsNullOrEmpty(readLine)) return;
            
            if (firstLine)
            {
                firstLine = false;
                continue;
            }

            string[] value = readLine.Split(",");
            InitLanguageGroup(value);
        }
    }

    public void InitLanguageGroup(string[] values)
    {
        int index = int.Parse(values[0]);
        string kor = values[1];
        string eng = values[2];
        int korSpacing = int.Parse(values[3]);
        int engSpacing = int.Parse(values[4]);

        if (index >= titleIndex && index <= titleEndindex)
        {
            titleKor.Add(kor);
            titleEng.Add(eng);
            titleSpacingKor.Add(korSpacing);
            titleSpacingEng.Add(engSpacing);
        }
        else if (index >= recheckIndex && index <= recheckEndindex)
        {
            recheckKor.Add(kor);
            recheckEng.Add(eng);
            recheckSpacingKor.Add(korSpacing);
            recheckSpacingEng.Add(engSpacing);
        }
        else if (index >= loadingIndex && index <= loadingEndindex)
        {
            loadingKor.Add(kor);
            loadingEng.Add(eng);
            loadingSpacingKor.Add(korSpacing);
            loadingSpacingEng.Add(engSpacing);
        }
        else if (index >= settingIndex && index <= settingEndindex)
        {
            settingKor.Add(kor);
            settingEng.Add(eng);
            settingSpacingKor.Add(korSpacing);
            settingSpacingEng.Add(engSpacing);
        }
        else if (index >= soundIndex && index <= soundEndindex)
        {
            soundKor.Add(kor);
            soundEng.Add(eng);
            soundSpacingKor.Add(korSpacing);
            soundSpacingEng.Add(engSpacing);
        }
        else if (index >= graphicIndex && index <= graphicEndindex)
        {
            graphicKor.Add(kor);
            graphicEng.Add(eng);
            graphicSpacingKor.Add(korSpacing);
            graphicSpacingEng.Add(engSpacing);
        }
        else if (index >= gameoverIndex && index <= gameoverEndindex)
        {
            gameoverKor.Add(kor);
            gameoverEng.Add(eng);
            gameoverSpacingKor.Add(korSpacing);
            gameoverSpacingEng.Add(engSpacing);
        }
        else if (index >= languageIndex && index <= languageEndindex)
        {
            languageKor.Add(kor);
            languageEng.Add(eng);
            languageSpacingKor.Add(korSpacing);
            languageSpacingEng.Add(engSpacing);
        }            
        else if (index >= keySetIndex && index <= keySetEndindex)
        {
            keySetKor.Add(kor);
            keySetEng.Add(eng);
            keysetSpacingKor.Add(korSpacing);
            keysetSpacingEng.Add(korSpacing);
        }
        else if (index >= padSetIndex && index <= padSetEndIndex)
        {
            padSetKor.Add(kor);
            padSetEng.Add(eng);
            padsetSpacingKor.Add(korSpacing);
            padsetSpacingEng.Add(engSpacing);
        }
        else if (index >= pauseIndex && index <= pauseEndindex)
        {
            pauseKor.Add(kor);
            pauseEng.Add(eng);
            pauseSpacingKor.Add(korSpacing);
            pauseSpacingEng.Add(engSpacing);
        }
        else if (index >= checkPointIndex && index <= checkPointEndindex)
        {
            cpKor.Add(kor);
            cpEng.Add(eng);
            cpSpacingKor.Add(korSpacing);
            cpSpacingEng.Add(engSpacing);
        }
        else if (index >= touchIndex && index <= touchEndIndex)
        {
            touchKor.Add(kor);
            touchEng.Add(eng);
            touchSpacingKor.Add(korSpacing);
            touchSpacingEng.Add(engSpacing);
        }
        else if (index >= setSaveIndex && index <= setSaveEndIndex)
        {
            setSaveKor.Add(kor);
            setSaveEng.Add(eng);
            setSaveSpacingKor.Add(korSpacing);
            setSaveSpacingEng.Add(engSpacing);
        }
    }

    Action languageEvent;

    public void LanguageEventResister(Action a)
    {
        languageEvent += a;
    }

    public void LangEventCall()
    {
        languageEvent?.Invoke();
    }

    public void ResetLangEvent()
    {
        languageEvent = null;
        Debug.Log("언어 액션 널 실행");
    }
}

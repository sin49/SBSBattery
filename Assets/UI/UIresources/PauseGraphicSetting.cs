using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;

[Serializable]
public class ResolutionGroup
{
    public string resolutionName;
    public int width, height;
}

public class PauseGraphicSetting : UIInteract
{
    public TestSettingUI choiceSetUI;

    public List<GameObject> graphicList = new List<GameObject>();
    public GameObject screw;

    [Header("수직동기화 관련")]
    public TextMeshProUGUI vsyncTMP; // 수직동기화 선택TMP
    public List<string> vsyncString = new List<string>(); //수직동기화 선택
    public float[] vsyncSpacing = new float[2];


    [Header("화면모드 관련")]
    public TextMeshProUGUI screenTMP; // 화면모드 선택TMP
    public List<string> screenString = new List<string>(); // 화면모드 선택
    public float[] screenSpacing = new float[2];

    [Header("해상도 설정")]
    public TextMeshProUGUI resolutionTMP;
    public List<string> resolutionTitle = new List<string>();
    public List<string> resolutionString = new List<string>();
    public List<ResolutionGroup> resolutionValue = new List<ResolutionGroup>();

    int index, beforeIndex;
    int vsyncIndex, screenIndex, resolutionIndex;

    bool onButton, graphicActive;
    public Sprite activeArrow, deactiveArrow;
    public Sprite activeButton, deactiveButton;

    public List<GameObject> buttonList;

    public CustomRecheckUI cRecheck;

    [Header("해상도 CSV파일")]
    public TextAsset resolutionCSV;
    
    private void Awake()
    {
        ResisterLang();
        ChangeLanguage();
        ReadCSV();
        korPack = LanguageManager.instance.soundKor;
        engPack = LanguageManager.instance.soundEng;
    }

    public void ReadCSV()
    {
        bool firstLine = true;
        StringReader readCSV = new StringReader(resolutionCSV.text); // CSV파일의 텍스트를 StreamReader클래스로 

        while (true)
        {
            string data = readCSV.ReadLine();

            if (firstLine)
            {
                firstLine = false;
                continue;
            }

            if (string.IsNullOrEmpty(data))
            {
                break;
            }

            //해상도 CSV파일의 요소: 인덱스 0번, 해상도 이름 1번, 가로 2번, 세로 3번

            ResolutionGroup rg = new ResolutionGroup();

            string[] values = data.Split(",");
            resolutionString.Add(values[0]);
            rg.resolutionName = values[0];
            rg.width = int.Parse(values[1]);
            rg.height = int.Parse(values[2]);

            resolutionValue.Add(rg);
        }
    }

    private void OnEnable()
    {
        InitGraphicSetting();
    }

    private void Start()
    {
        korPack = LanguageManager.instance.graphicKor;
        engPack = LanguageManager.instance.graphicEng;
    }

    public void InitGraphicSetting()
    {
        foreach (GameObject obj in buttonList)
        {            
            obj.SetActive(true);            
        }

        if (beforeIndex > graphicList.Count - 3)
        {
            DeactiveButton();
        }
        else
        {
            graphicList[beforeIndex].GetComponent<Image>().sprite = deactiveArrow;
        }

        if (index > graphicList.Count - 3)
        {
            onButton = true;
            ActiveButton();
        }
        else
        {
            onButton = false;
            graphicList[index].GetComponent<Image>().sprite = activeArrow;
        }

        UpdateGraphicData();

        //switch (beforeIndex)
        //{
        //    case 0:
        //    case 1:
        //        graphicList[beforeIndex].GetComponent<Image>().sprite = deactiveArrow;
        //        break;
        //    case 2:
        //    case 3:
        //        DeactiveButton();
        //        break;
        //    default:
        //        Debug.Log("out of range");
        //        break;
        //}

        //switch (index)
        //{
        //    case 0:
        //    case 1:
        //        graphicList[index].GetComponent<Image>().sprite = activeArrow;
        //        break;
        //    case 2:
        //    case 3:
        //        ActiveButton();
        //        break;
        //    default:
        //        Debug.Log("out of range");
        //        break;
        //}
        graphicActive = true;
    }

    public void UpdateGraphicData()
    {
        // 수직동기화 데이터 확인
        if (PlayerPrefs.HasKey("VsyncData"))
            vsyncIndex = PlayerPrefs.GetInt("VsyncData");
        vsyncTMP.text = vsyncString[vsyncIndex];
        vsyncTMP.characterSpacing = vsyncSpacing[vsyncIndex];

        // 화면모드 데이터 확인
        if (PlayerPrefs.HasKey("ScreenModeData"))
            screenIndex = PlayerPrefs.GetInt("ScreenModeData");
        screenTMP.text = screenString[screenIndex];
        screenTMP.characterSpacing = screenSpacing[screenIndex];

        // 해상도 데이터 확인
        string dataName = "ResolutionData.json";
        string filePath = Path.Combine(Application.persistentDataPath, dataName);

        if (File.Exists(filePath))
        {
            var a = File.ReadAllText(filePath);
            ResolutionGroup rg = JsonUtility.FromJson<ResolutionGroup>(a);

            for (int i = 0; i < resolutionString.Count; i++)
            {
                if (resolutionString[i] == rg.resolutionName)
                {
                    resolutionIndex = i;
                    break;
                }
            }
        }

        resolutionTMP.text = resolutionString[resolutionIndex];

    }

    bool moved, horiMoved;
    float moveValue;
    float horiValue;
    // Update is called once per frame
    void Update()
    {
        if (graphicActive)
        {
            moveValue = Input.GetAxisRaw("Vertical");
            horiValue = Input.GetAxisRaw("Horizontal");

            HorizontalInput(); // 수직입력
            VerticalInput(); // 수평입력
            EnterInput(); // 확인입력

        }        
    }

    #region 입력
    public void HorizontalInput()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || moveValue > 0) && !moved)
        {
            moved = true;

            if (index > 0)
            {
                beforeIndex = index;
                index--;
                UpdateUI();
            }
            Debug.Log("두 번 나오나");
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || moveValue == 0)
        {
            moved = false;
        }


        if ((Input.GetKeyDown(KeyCode.DownArrow) || moveValue < 0) && !moved)
        {
            moved = true;

            if (onButton)
                return;
            if (index < graphicList.Count - 1)
            {
                beforeIndex = index;
                index++;
                UpdateUI();
            }
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) || moveValue == 0)
        {
            moved = false;
        }
    }
    public void VerticalInput()
    {
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || horiValue < 0) && !horiMoved)
        {
            horiMoved = true;
            if (!onButton)
            {
                switch (index)
                {
                    case 0:
                        if (vsyncIndex > 0)
                            vsyncIndex--;
                        UpdateResolutionUI();
                        break;
                    case 1:
                        if (screenIndex > 0)
                            screenIndex--;
                        UpdateScreenUI();
                        break;
                    case 2:
                        if (resolutionIndex > 0)
                            resolutionIndex--;
                        UpdateResValueUI();
                        break;
                    default:
                        Debug.Log("out of range");
                        break;
                }
            }
            else
            {
                if (index > graphicList.Count - 2)
                {
                    beforeIndex = index;
                    index--;
                    UpdateUI();
                }
            }

        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || horiValue == 0)
            horiMoved = false;

        if ((Input.GetKeyDown(KeyCode.RightArrow) || horiValue > 0) && !horiMoved)
        {
            horiMoved = true;
            if (!onButton)
            {
                switch (index)
                {
                    case 0:
                        if (vsyncIndex < vsyncString.Count - 1)
                            vsyncIndex++;
                        UpdateResolutionUI();
                        break;
                    case 1:
                        if (screenIndex < screenString.Count - 1)
                            screenIndex++;
                        UpdateScreenUI();
                        break;
                    case 2:
                        if (resolutionIndex < resolutionString.Count - 1)
                            resolutionIndex++;
                        UpdateResValueUI();
                        break;
                    default:
                        Debug.Log("out of range");
                        break;
                }
            }
            else
            {
                if (index < graphicList.Count - 1)
                {
                    beforeIndex = index;
                    index++;
                    UpdateUI();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || horiValue == 0)
            horiMoved = false;
    }
    public void EnterInput()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Space)
                || Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return))
        {
            SelectSetting();
        }
    }
    #endregion

    #region 버튼 선택 작용
    public void SelectSetting()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            switch (index)
            {
                case 0:
                    //Debug.Log("수직동기화 기능 구현해야함");
                    break;
                case 1:
                    //Debug.Log("화면모드 적용 기능 구현해야함");
                    break;
                case 2:
                    //SaveGraphicSetting();
                    graphicActive = false;
                    cRecheck.ActionActive(SaveGraphicSetting, CurrentSettingExit);
                    break;
                case 3:
                    CurrentSettingExit();
                    break;
            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    //Debug.Log("수직동기화 기능 구현해야함");
                    break;
                case 1:
                    //Debug.Log("화면모드 적용 기능 구현해야함");
                    break;
                case 2:
                    break;
                case 3:
                    //SaveGraphicSetting();
                    graphicActive = false;
                    cRecheck.ActionActive(SaveGraphicSetting, CurrentSettingExit);
                    break;
                case 4:
                    CurrentSettingExit();
                    break;
            }
        }
    }

    public void SaveGraphicSetting()
    {
        ChoiceVSyncMode();
        ChoiceScreenMode();
        ChoiceResolutionMode();
        graphicActive = true;
    }

    public void ChoiceVSyncMode()
    {
        switch (vsyncIndex)
        {
            case 0:
                QualitySettings.vSyncCount = 0;
                break;
            case 1:
                QualitySettings.vSyncCount = 1;
                break;
        }

        PlayerPrefs.SetInt("VsyncData", vsyncIndex);
    }

    public void ChoiceScreenMode()
    {
        switch (screenIndex)
        {
            case 0:
                GameManager.instance.ChangeWindowed();
                break;
            case 1:
                GameManager.instance.ChangeFullscreen();
                break;
        }
        PlayerPrefs.SetInt("ScreenModeData", screenIndex);
    }

    public void ChoiceResolutionMode()
    {
        GameManager.instance.SetResolution(resolutionValue[resolutionIndex].resolutionName, resolutionValue[resolutionIndex].width, resolutionValue[resolutionIndex].height);

        string dataName = "ResolutionData.json";
        string filePath = Path.Combine(Application.persistentDataPath, dataName);

        ResolutionGroup rg = new ResolutionGroup();
        rg.resolutionName = resolutionValue[resolutionIndex].resolutionName;
        rg.width = resolutionValue[resolutionIndex].width;
        rg.height = resolutionValue[resolutionIndex].height;

        string data = JsonUtility.ToJson(rg);
        File.WriteAllText(filePath, data);
        //Debug.Log("해상도 적용");
    }
    #endregion

    #region 수직동기화 업데이트
    public void ResolutionDecrease()
    {
        if (vsyncIndex > 0)
        {
            vsyncIndex--;
            UpdateResolutionUI();
        }
    }

    public void ResolutionIncrease()
    {
        if (vsyncIndex < vsyncString.Count - 1)
        {
            vsyncIndex++;
            UpdateResolutionUI();
        }
    }

    //해상도 갱신 준비
    public void UpdateResolutionUI()
    {
        vsyncTMP.text = vsyncString[vsyncIndex];
    }
    #endregion

    #region 화면모드 업데이트
    public void ScreenDecrease()
    {
        if (screenIndex > 0)
        {
            screenIndex--;
            UpdateScreenUI();
        }
    }

    public void ScreenIncrease()
    {
        if (screenIndex < screenString.Count - 1)
        {
            screenIndex++;
            UpdateScreenUI();
        }
    }

    //화면 모드 갱신 준비
    public void UpdateScreenUI()
    {
        screenTMP.text = screenString[screenIndex];
    }
    #endregion

    #region 해상도 업데이트

    public void ResValueDecrease()
    {
        if (resolutionIndex > 0)
        {
            resolutionIndex--;
            UpdateResValueUI();
        }
    }

    public void ResValueIncrease()
    {
        if (resolutionIndex < resolutionString.Count - 1)
        {
            resolutionIndex++;
            UpdateResValueUI();
        }
    }

    public void UpdateResValueUI()
    {
        resolutionTMP.text = resolutionString[resolutionIndex];
    }
    #endregion

    #region 이벤트 (버튼 + 이벤트 트리거)
    public void CheckArrow(int n)
    {
        switch (n)
        {
            case 0:
            case 1:
                SetIndex(0);
                break;
            case 2:
            case 3:
                SetIndex(1);
                break;
            case 4:
            case 5:
                SetIndex(2);
                break;
        }
    }

    public void SetIndex(int n)
    {
        if (index == n) return;
        beforeIndex = index;
        index = n;
        UpdateUI();
    }

    [Header("수직동기화 화살표")] 
    public List<GameObject> vsyncArrow = new List<GameObject>();
    [Header("화면모드 화살표")]
    public List<GameObject> screenArrow = new List<GameObject>();
    [Header("해상도 화살표")]
    public List<GameObject> resolutionArrow = new List<GameObject>();
    public void UpdateArrow()
    {
        if (vsyncIndex <= 0)
        {
            vsyncArrow[0].SetActive(false);
        }
        else if (vsyncIndex >= vsyncString.Count-1)
        {
            vsyncArrow[1].SetActive(false);
        }
        else
        {
            foreach (GameObject arrow in vsyncArrow)
            {
                arrow.SetActive(true);
            }
        }

        if (screenIndex <= 0)
        {
            screenArrow[0].SetActive(false);
        }
        else if (screenIndex >= screenString.Count - 1)
        {
            screenArrow[1].SetActive(false);
        }
        else
        {
            foreach (GameObject arrow in screenArrow)
            {
                arrow.SetActive(true);
            }
        }

        if (resolutionIndex <= 0)
        {
            resolutionArrow[0].SetActive(false);
        }
        else if (resolutionIndex >= resolutionString.Count - 1)
        {
            resolutionArrow[1].SetActive(false);
        }
        else
        {
            foreach (GameObject arrow in resolutionArrow)
            {
                arrow.SetActive(true);
            }
        }
    }
    #endregion

    //현재 화면에서 나감
    public void CurrentSettingExit()
    {
        graphicActive = false;
        onButton = false;
        foreach (GameObject obj in buttonList)
        {
            obj.SetActive(false);
        }
        if (index > graphicList.Count - 3)
        {
            graphicList[index].GetComponent<Image>().sprite = deactiveButton;
            fontList[index].color = deactiveFontColor;
        }
        screw.SetActive(false);
        gameObject.SetActive(false);
        choiceSetUI.ShowChoiceScreen();
    }
    // 설정할 놈으로 이동
    public void UpdateUI()
    {
        if (index > graphicList.Count - 3)
        {
            Debug.Log("버튼 만짐");
            if (!onButton)
            {
                onButton = true;
                screw.SetActive(true);
            }
            screw.transform.SetParent(graphicList[index].transform.GetChild(1));
            screw.transform.position = graphicList[index].transform.GetChild(1).position;
            ActiveButton();
        }
        else
        {
            Debug.Log("이건 화살표 그건데?");
            if (onButton)
            {
                onButton = false;
                screw.SetActive(false);
            }
            graphicList[index].GetComponent<Image>().sprite = activeArrow;
        }

        if (beforeIndex > graphicList.Count - 3)
        {
            DeactiveButton();
        }
        else
        {
            graphicList[beforeIndex].GetComponent<Image>().sprite = deactiveArrow;
        }
    }

    public void ActiveButton()
    {
        graphicList[index].GetComponent<Image>().sprite = activeButton;
        fontList[index].color = activeFontColor;
    }

    public void DeactiveButton()
    {
        graphicList[beforeIndex].GetComponent<Image>().sprite = deactiveButton;
        fontList[beforeIndex].color = deactiveFontColor;
    }

    #region 언어변경
    [Header("그래픽 설정 타이틀")]
    public TextMeshProUGUI titleFont;
    [Header("언어팩에 작용할 폰트 리스트")]
    public List<TextMeshProUGUI> languageList = new List<TextMeshProUGUI>();

    public void ResisterLang()
    {
        LanguageManager.instance.LanguageEventResister(ChangeLanguage);
    }

    public void ChangeLanguage()
    {
        int index = 0;

        if (LanguageManager.instance.isKor)
        {
            titleFont.text = LanguageManager.instance.graphicKor[0];
            titleFont.characterSpacing = LanguageManager.instance.graphicSpacingKor[0];
            for (int i = 0; i < fontList.Count; i++)
            {
                fontList[i].text = LanguageManager.instance.graphicKor[i + 1];
                fontList[i].characterSpacing = LanguageManager.instance.graphicSpacingKor[i + 1];
                index++;
            }
            for (int i = 0; i < vsyncString.Count; i++)
            {
                vsyncString[i] = LanguageManager.instance.graphicKor[index + 1];
                vsyncSpacing[i] = LanguageManager.instance.graphicSpacingKor[index + 1];
                index++;
            }
            for (int i = 0; i < screenString.Count; i++)
            {
                screenString[i] = LanguageManager.instance.graphicKor[index + 1];
                screenSpacing[i] = LanguageManager.instance.graphicSpacingKor[index + 1];
                index++;
            }
        }
        else
        {
            titleFont.text = LanguageManager.instance.graphicEng[0];
            titleFont.characterSpacing = LanguageManager.instance.graphicSpacingEng[0];
            for (int i = 0; i < fontList.Count; i++)
            {
                fontList[i].text = LanguageManager.instance.graphicEng[i + 1];
                fontList[i].characterSpacing = LanguageManager.instance.graphicSpacingEng[i + 1];
                index++;
            }
            for (int i = 0; i < vsyncString.Count; i++)
            {
                vsyncString[i] = LanguageManager.instance.graphicEng[index + 1];
                vsyncSpacing[i] = LanguageManager.instance.graphicSpacingEng[index + 1];
                index++;
            }
            for (int i = 0; i < screenString.Count; i++)
            {
                screenString[i] = LanguageManager.instance.graphicEng[index + 1];
                screenSpacing[i] = LanguageManager.instance.graphicSpacingEng[index + 1];
                index++;
            }

            vsyncTMP.text = vsyncString[vsyncIndex];
            vsyncTMP.characterSpacing = vsyncSpacing[vsyncIndex];
            screenTMP.text = screenString[screenIndex];
            screenTMP.characterSpacing = screenSpacing[screenIndex];
        }
    }
    //public void ChangeLanguage()
    //{
    //    if (LanguageManager.instance.isKor)
    //    {
    //        titleFont.text = LanguageManager.instance.graphicKor[0];
    //        titleFont.characterSpacing = LanguageManager.instance.graphicSpacingKor[0];

    //        for (int i = 0; i < fontList.Count; i++)
    //        {
    //            languageList[i].text = LanguageManager.instance.graphicKor[i+1];
    //            languageList[i].characterSpacing = LanguageManager.instance.graphicSpacingKor[i + 1];
    //        }
    //    }
    //    else
    //    {
    //        titleFont.text = LanguageManager.instance.graphicEng[0];
    //        titleFont.characterSpacing = LanguageManager.instance.graphicSpacingEng[0];

    //        for (int i = 0; i < fontList.Count; i++)
    //        {
    //            languageList[i].text = LanguageManager.instance.graphicEng[i+1];
    //            languageList[i].characterSpacing = LanguageManager.instance.graphicSpacingEng[i + 1];
    //        }
    //    }
    //}
    #endregion
}

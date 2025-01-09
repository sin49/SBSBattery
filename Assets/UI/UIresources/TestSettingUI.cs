using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestSettingUI : UIInteract
{
    //public SelectUI uiGroup;
    //[HideInInspector]public TestPauseUI uiGroup;

    public SelectUI uiSelect;
    public TitleScreen title;

    public bool settingActive;

    public List<Image> buttonList = new List<Image>();
    int index, beforeIndex;
    int rangeIndex, beforeRangeIndex;

    public Sprite activeButton;
    public Sprite deactiveButton;

    public Animator settingAnimator;
    Vector3 settingScale = new(0.8f, 0.8f, 0.8f);


    bool choiceSetting;

    public GameObject choice, sound, graphic, keyCustom, padCustom, language;

    [HideInInspector] public GameObject canvas;

    public List<string> textList = new List<string>();
    public List<float> spacingList = new List<float>();

    public List<GameObject> arrowGroup = new List<GameObject>();
    public Sprite activeArrow, deactiveArrow;

    private void Start()
    {
        ResisterLang();
        ChangeLanguage();
        AddArrowClick();
        gameObject.SetActive(false);
    }

    #region 화살표 이벤트(버튼 + eventTrigger)
    public void AddArrowClick()
    {
        arrowGroup[0].GetComponent<Button>().onClick.AddListener(OnClickUpArrow); // 0 => uparrow
        arrowGroup[1].GetComponent<Button>().onClick.AddListener(OnClickDownArrow); // 1 => downarrow
    }

    public void OnClickUpArrow() //버튼이벤트
    {
        Debug.Log("위 화살표");
        if (index > 0)
        {
            beforeIndex = index;
            index--;
            beforeRangeIndex = rangeIndex;
            rangeIndex--;
            UpdateUI();
            ActiveSound();
        }
    }

    public void OnClickDownArrow() // 버튼이벤트
    {
        Debug.Log("아래 화살표");
        if (index < textList.Count - 1)
        {
            beforeIndex = index;
            index++;
            beforeRangeIndex = rangeIndex;
            rangeIndex++;
            UpdateUI();
            ActiveSound();
        }
    }

    public void PointerEnterArrow(int n) // 이벤트 트리거
    {
        arrowGroup[n].GetComponent<Image>().sprite = activeArrow;
        SelectSound();
    }

    public void PointerExitArrow(int n) // 이벤트 트리거
    {
        arrowGroup[n].GetComponent<Image>().sprite = deactiveArrow;
    }

    public void setIndex(int n) //이벤트 트리거
    {
        beforeRangeIndex = rangeIndex;
        rangeIndex = n;
        UpdateSetIndex();
        SelectSound();
    }

    public void UpdateSetIndex()
    {
        DeactiveButton();
        ActiveButton();
        CheckButtonIndex();
        //CheckArrowActive();
    }
    #endregion


    private void OnEnable()
    {        
        for (int i = 0; i < buttonList.Count; i++)
        {
            fontList[i].text = textList[i];
            fontList[i].characterSpacing = spacingList[i];
        }
        InitButtonUI();
    }

    private void OnDisable()
    {
        ResetIndex();
    }

    public void ResetIndex()
    {
        index = 0;
        rangeIndex = 0;

        beforeIndex = 0;
        beforeRangeIndex = 0;
    }

    bool moved;
    float movevalue;
    // Update is called once per frame
    void Update()
    {

        if (settingActive)
        {            
            movevalue = Input.GetAxisRaw("Vertical");

            if (!choiceSetting)
            {
                if ((Input.GetKeyDown(KeyCode.UpArrow) || movevalue > 0) && !moved)
                {
                    moved = true;
                    if (index > 0)
                    {
                        beforeIndex = index;
                        index--;

                        beforeRangeIndex = rangeIndex;
                        rangeIndex--;
                        UpdateUI();
                    }                    
                }

                if (Input.GetKeyUp(KeyCode.UpArrow) && movevalue ==0)
                    moved = false;

                if (Input.GetKeyUp(KeyCode.DownArrow) && movevalue == 0)
                    moved = false;

                if ((Input.GetKeyDown(KeyCode.DownArrow) || movevalue < 0) && !moved)
                {
                    moved = true;
                    if (index < textList.Count - 1)
                    {
                        beforeIndex = index;
                        index++;

                        beforeRangeIndex = rangeIndex;
                        rangeIndex++;
                        UpdateUI();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Space) || 
                Input.GetKeyDown(KeyCode.JoystickButton0)|| Input.GetKeyDown(KeyCode.Return))
            {
                ChoiceInteractUI();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                SettingExit();
            }
        }
    }
    public bool CheckTitle()
    {
        bool check = false;
        if (SceneManager.GetActiveScene().name == "CheckTitleTest" || SceneManager.GetActiveScene().name == "TitleTest")
        {
            check = true;
        }

        return check;
    }

    public void ChoiceInteractUI()
    {
        
        switch (index)
        {
            case 0:
                Debug.Log("소리 설정");
                NextSelectSetting(sound);
                break;
            case 1:
                Debug.Log("그래픽 설정");
                NextSelectSetting(graphic);
                break;
            case 2:
                NextSelectSetting(keyCustom);
                break;
            case 3:
                NextSelectSetting(padCustom);
                break;
            case 4:
                NextSelectSetting(language);
                break;
            case 5:
                SettingExit();
                break;
            default:
                Debug.Log("범위 초과함");
                break;
        }

        switch (index)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                ActiveSound();
                break;
            case 5:
                DeactiveSound();
                break;
        }
    }

    #region 시스템 사운드
    public void ActiveSound()
    {
        if (SceneManager.GetActiveScene().name != "CheckTitleTest")
            uiSelect.pauseui.ButtonSoundEffectPlayer_.PlayActiveAudio();
        else
            title.ButtionSoundEffectPlayer_.PlayActiveAudio();
    }

    public void DeactiveSound()
    {
        if (SceneManager.GetActiveScene().name != "CheckTitleTest")
            uiSelect.pauseui.ButtonSoundEffectPlayer_.PlayDeActiveAudio();
        else
            title.ButtionSoundEffectPlayer_.PlayDeActiveAudio();
    }

    public void SelectSound()
    {
        if (SceneManager.GetActiveScene().name != "CheckTitleTest")
            uiSelect.pauseui.ButtonSoundEffectPlayer_.PlaySelectAudio();
        else
            title.ButtionSoundEffectPlayer_.PlaySelectAudio();
    }
    #endregion
    public void NextSelectSetting(GameObject selectSetting)
    {
        settingActive = false;
        choiceSetting = true;
        choice.SetActive(false);
        selectSetting.SetActive(true);
        GetComponent<Image>().enabled = false;
    }

    public void SettingExit()
    {
        fontList[rangeIndex].color = deactiveFontColor;
        buttonList[rangeIndex].sprite = deactiveButton;
        settingActive = false;
        if (SceneManager.GetActiveScene().name != "CheckTitleTest" && SceneManager.GetActiveScene().name != "TitleTest")
        {
            settingAnimator.Play("SettingChangePause");
            StartCoroutine(SCP());
        }
        else
        {
            gameObject.SetActive(false);
            title.SettingBackScreen();
        }

    }

    IEnumerator SCP()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        if (settingAnimator.GetCurrentAnimatorStateInfo(0).IsName("SettingChangePause"))
        {
            while (settingAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }

            gameObject.SetActive(false);

            uiSelect.uiGroup.SetActive(true);
            uiSelect.PauseBackSetting();

            uiSelect.coinPanel.SetActive(true);
            uiSelect.pauseIconPanel.SetActive(true);
        }
    }

    public void UpdateUI()
    {
        Debug.Log("UI 업데이트");
        int count = 0;
        if (rangeIndex <= 0)
        {
            rangeIndex = 0;
            while (count < buttonList.Count)
            {
                fontList[rangeIndex + count].text = textList[index + count];
                fontList[rangeIndex + count].characterSpacing = spacingList[index + count];
                count++;
            }
        }
        else if (rangeIndex >= buttonList.Count - 1)
        {
            rangeIndex = buttonList.Count - 1;
            while (count < buttonList.Count)
            {
                fontList[rangeIndex - count].text = textList[index - count];
                fontList[rangeIndex - count].characterSpacing = spacingList[index - count];
                count++;
            }
        }
        CheckArrowActive();
        //else if (rangeIndex > 0 && rangeIndex < buttonList.Count-1)
        //{
        //    int i = index - rangeIndex; // 4-1 -> 3 +0(3) + 1(4) +2(5)
        //    rangeIndex = 0;
        //    while (count < buttonList.Count)
        //    {
        //        fontList[rangeIndex + count].text = textList[i + count];
        //        fontList[rangeIndex + count].characterSpacing = spacingList[i + count];
        //        count++;
        //    }
        //}

        DeactiveButton();
        ActiveButton();
    }

    public void CheckArrowActive()
    {
        if (index <= 0)
        {
            arrowGroup[0].SetActive(false);
            arrowGroup[0].GetComponent<Image>().sprite = deactiveArrow;
        }
        else if (index >= textList.Count - 1)
        {
            arrowGroup[1].SetActive(false);
            arrowGroup[1].GetComponent<Image>().sprite = deactiveArrow;
        }
        else
        {
            arrowGroup[0].SetActive(true);
            arrowGroup[1].SetActive(true);
        }
    }

    public void CheckButtonIndex() //화살표 클릭에 따라 인덱스 체크(문자열 비교)
    {
        foreach (string str in textList)
        {            
            if (fontList[rangeIndex].text == str)
            {
                int changeIndex = textList.IndexOf(str);
                beforeIndex = index;
                index = changeIndex;
                break;
            }
        }
    }

    public void InitButtonUI()
    {
        beforeIndex = index;
        index = 0;
        DeactiveButton();
        ActiveButton();

        if (SceneManager.GetActiveScene().name != "CheckTitleTest" && SceneManager.GetActiveScene().name != "TitleTest")
        {
            StartCoroutine(EndSettingAnimation());
        }
        else
            settingActive = true;
    }

    IEnumerator EndSettingAnimation()
    {
        while (settingAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        settingActive = true;
    }

    public void ShowChoiceScreen()
    {
        GetComponent<Image>().enabled = true;
        choice.SetActive(true);
        buttonList[beforeRangeIndex].sprite = deactiveButton;
        buttonList[rangeIndex].sprite = activeButton;
        choiceSetting = false;
        settingActive = true;

    }

    public void ActiveButton()
    {
        buttonList[rangeIndex].sprite = activeButton;
        fontList[rangeIndex].color = activeFontColor;
    }

    public void DeactiveButton()
    {
        buttonList[beforeRangeIndex].sprite = deactiveButton;
        fontList[beforeRangeIndex].color = deactiveFontColor;
    }
    #region 언어변경
    public void ResisterLang()
    {
        LanguageManager.instance.LanguageEventResister(ChangeLanguage);
    }
    [Header("설정 타이틀")]
    public TextMeshProUGUI titleFont;

    public void ChangeLanguage()
    {
        if (LanguageManager.instance.isKor)
        {
            titleFont.text = LanguageManager.instance.settingKor[0];
            titleFont.characterSpacing = LanguageManager.instance.settingSpacingKor[0];
            for (int i = 0; i< textList.Count; i++)
            {
                textList[i] = LanguageManager.instance.settingKor[i+1];
                spacingList[i] = LanguageManager.instance.settingSpacingKor[i + 1];
            }
        }
        else
        {
            titleFont.text = LanguageManager.instance.settingEng[0];
            titleFont.characterSpacing = LanguageManager.instance.settingSpacingEng[0];
            for (int i = 0; i < textList.Count; i++)
            {
                textList[i] = LanguageManager.instance.settingEng[i+1];
                spacingList[i] = LanguageManager.instance.settingSpacingEng[i + 1];
            }
        }
        UpdateLanguage();
    }
    
    public void UpdateLanguage()
    {
        UpdateUI();
        if (rangeIndex > 0 && rangeIndex < buttonList.Count - 1)
        {
            int count = 0;
            int i = index - rangeIndex;
            while (count < buttonList.Count)
            {
                fontList[count].text = textList[i + count];
                fontList[count].characterSpacing = spacingList[i + count];
                count++;
            }
        }
    }
    #endregion
}

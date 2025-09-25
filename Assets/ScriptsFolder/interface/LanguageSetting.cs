using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class LanguageSetting : UIInteract
{
    public List<string> LanguageGroup = new List<string>();

    int index, beforeIndex;
    bool onHandle;

    public List<GameObject> arrowGroup = new List<GameObject>();
    public TestSettingUI settingUI;
    public Sprite activeArrow, deactiveArrow, activeButton, deactiveButton;

    public Button checkButton;

    [Header("언어설정 타이틀")] public TextMeshProUGUI titleFont;

    private void OnEnable()
    {
        InitLanguage();
        UpdateArrow();
        InitIndex();
    }

    private void Start()
    {
        ResisterLang();
        AddButtonEvent();
    }

    public void InitIndex()
    {
        switch (vertIndex)
        {
            case 0:
                foreach (GameObject arrow in arrowGroup)
                {
                    arrow.GetComponent<Image>().sprite = activeArrow;
                }
                fontList[1].color = deactiveFontColor;
                checkButton.GetComponent<Image>().sprite = deactiveButton;
                break;
            case 1:
                foreach (GameObject arrow in arrowGroup)
                {
                    arrow.GetComponent<Image>().sprite = deactiveArrow;
                }
                fontList[1].color = activeFontColor;
                checkButton.GetComponent<Image>().sprite = activeButton;
                break;
        }

    }

    #region (버튼 + 이벤트 트리거)
    public void AddButtonEvent()
    {
        arrowGroup[0].GetComponent<Button>().onClick.AddListener(PreviousButton);
        arrowGroup[1].GetComponent<Button>().onClick.AddListener(NextButton);
        checkButton.onClick.AddListener(OnClickCheckButton);
    }

    public void NextButton()
    {
        Debug.Log("오른쪽 화살표");
        if (index < arrowGroup.Count - 1)
        {
            index++;
            UpdateLanguageUI();
        }
    }

    public void PreviousButton()
    {
        Debug.Log("왼쪽 화살표");
        if (index > 0)
        {
            index--;
            UpdateLanguageUI();
        }
    }

    public void PointerEnter(int n)
    {
        arrowGroup[n].GetComponent<Image>().sprite = activeArrow;
    }

    public void PointerExit(int n)
    {
        arrowGroup[n].GetComponent<Image>().sprite = deactiveArrow;
    }

    public void OnClickCheckButton()
    {
        SettingExit();
    }

    public void PointerEnterButton(int n)
    {
        fontList[n].color = activeFontColor;
        checkButton.GetComponent<Image>().sprite = activeButton;
    }

    public void PointerExitButton(int n)
    {
        fontList[n].color = deactiveFontColor;
        checkButton.GetComponent<Image>().sprite = deactiveButton;
    }
    #endregion

    public void InitLanguage()
    {
        if (LanguageManager.instance.isKor)
        {
            titleFont.text = LanguageManager.instance.languageKor[0];
            titleFont.characterSpacing = LanguageManager.instance.languageSpacingKor[0];
            for (int i = 0; i < fontList.Count; i++)
            {
                fontList[i].text = LanguageManager.instance.languageKor[i+1];
                fontList[i].characterSpacing = LanguageManager.instance.languageSpacingKor[i+1];
            }            
        }
        else
        {
            titleFont.text = LanguageManager.instance.languageEng[0];
            titleFont.characterSpacing = LanguageManager.instance.languageSpacingEng[0];
            for (int i = 0; i < fontList.Count; i++)
            {
                fontList[i].text = LanguageManager.instance.languageEng[i + 1];
                fontList[i].characterSpacing = LanguageManager.instance.languageSpacingEng[i + 1];
            }
        }

        onHandle = true;
    }
    float horiValue, vertValue;
    bool moved, onButton;
    // Update is called once per frame
    void Update()
    {
        if (!onHandle) return;

        UpdateHoriInput();
        UpdateVertInput();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            SelectUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            SettingExit();
        }

        if (settingUI != null)
        {
            Debug.Log($"설정 UI 있음{settingUI.gameObject}");
        }
        else
        {
            Debug.Log("LanguageSetting쪽에서 SettingUI오브젝트가 살아있지 않음");
        }
    }

    public void SelectUI()
    {
        switch (vertIndex)
        {
            case 0:
                Debug.Log("언어 설정중");
                break;
            case 1:
                SettingExit();
                break;
        }
    }

    #region 키 입력관련
    //좌우 입력
    public void UpdateHoriInput()
    {
        if (onButton) return;

        horiValue = Input.GetAxisRaw("Horizontal");

        if (((Input.GetKeyDown(KeySettingManager.instance.rightKeycode) || Input.GetKeyDown(KeyCode.RightArrow)) || horiValue > 0) && !moved)
        {
            moved = true;

            if (index < LanguageGroup.Count - 1)
            {
                //beforeIndex = index;
                index++;
                arrowGroup[0].GetComponent<Image>().sprite = deactiveArrow;
                arrowGroup[1].GetComponent<Image>().sprite = activeArrow;
                UpdateLanguageUI();
            }
        }

        if ((Input.GetKeyUp(KeySettingManager.instance.rightKeycode) || Input.GetKeyUp(KeyCode.RightArrow))
            && horiValue == 0)
        {
            moved = false;
        }

        if (((Input.GetKeyDown(KeySettingManager.instance.leftKeycode) || Input.GetKeyDown(KeyCode.LeftArrow)) || horiValue < 0) && !moved)
        {
            moved = true;
            if (index > 0)
            {
                //beforeIndex = index;
                index--;
                //arrowGroup[0].GetComponent<Image>().sprite = activeArrow;
                //arrowGroup[1].GetComponent<Image>().sprite = deactiveArrow;
                UpdateLanguageUI();
            }
        }

        if ((Input.GetKeyUp(KeySettingManager.instance.leftKeycode) || Input.GetKeyUp(KeyCode.LeftArrow))
            && horiValue == 0)
        {
            moved = false;
        }
    }
    //상하 입력
    int vertIndex, beforeVertIndex;

    //public bool UpKeyInput()
    //{
    //    if (Input.GetKeyUp(KeySettingManager.instance.upKeycode) || Input.GetKeyUp(KeyCode.UpArrow))
    //    {
    //        return true;
    //    }
    //    else
    //        return false;
    //}
    
    //public bool DownKeyInput()
    //{
    //    if (Input.GetKeyUp(KeySettingManager.instance.downKeycode) || Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        return true;
    //    }
    //    else
    //        return false;
    //}

    public void UpdateVertInput()
    {
        vertValue = Input.GetAxisRaw("Vertical");

        if ((Input.GetKeyDown(KeySettingManager.instance.upKeycode) || Input.GetKeyDown(KeyCode.UpArrow)) || vertValue > 0 && !moved)
        {
            moved = true;
            if (vertIndex > 0)
            {
                beforeVertIndex = vertIndex;
                vertIndex--;
                UpdateVertUI();
            }
        }

        if ((Input.GetKeyUp(KeySettingManager.instance.upKeycode) || Input.GetKeyUp(KeyCode.UpArrow)) && vertValue == 0)
        {
            moved = false;
        }

        if (((Input.GetKeyDown(KeySettingManager.instance.downKeycode) || Input.GetKeyDown(KeyCode.DownArrow)) || vertValue < 0) && !moved)
        {
            moved = true;
            if (vertIndex < LanguageGroup.Count - 1)
            {
                beforeVertIndex = vertIndex;
                vertIndex++;
                UpdateVertUI();
            }
        }

        if ((Input.GetKeyUp(KeySettingManager.instance.downKeycode) || Input.GetKeyUp(KeyCode.DownArrow))
            && vertValue == 0)
        {
            moved = false;
        }
    }

    public void SettingExit()
    {
        onHandle = false;
        gameObject.SetActive(false);
        settingUI.ShowChoiceScreen();
    }
    #endregion

    #region 언어 변경
    public void ResisterLang()
    {
        LanguageManager.instance.LanguageEventResister(ChangeLanguage);
    }

    public void ChangeLanguage()
    {
        if (LanguageManager.instance.isKor)
        {
            titleFont.text = LanguageManager.instance.languageKor[0];
            titleFont.characterSpacing = LanguageManager.instance.languageSpacingKor[0];
            for (int i = 0; i < fontList.Count; i++)
            {
                fontList[i].text = LanguageManager.instance.languageKor[i + 1];
                fontList[i].characterSpacing = LanguageManager.instance.languageSpacingKor[i + 1];
            }
        }
        else
        {
            titleFont.text = LanguageManager.instance.languageEng[0];
            titleFont.characterSpacing = LanguageManager.instance.languageSpacingEng[0];
            for (int i = 0; i < fontList.Count; i++)
            {
                fontList[i].text = LanguageManager.instance.languageEng[i + 1];
                fontList[i].characterSpacing = LanguageManager.instance.languageSpacingEng[i + 1];
            }
        }
    }
    #endregion

    Dictionary<string, bool> languageDic
        = new Dictionary<string, bool>()
        {
            {"English", false },
            {"한국어", true }
        };
    #region UI갱신?
    public void UpdateLanguageUI()
    {
        string str = LanguageGroup[index];
        fontList[0].text = str;
        Debug.Log($"before eventCall {str}");
        if (languageDic.TryGetValue(str, out bool isKor))
        {
            LanguageManager.instance.isKor = isKor;
        }

        if (LanguageManager.instance.isKor)
        {
            PlayerPrefs.SetInt("Language", 2);
        }
        else
        {
            PlayerPrefs.SetInt("Language", 1);
        }
        
        LanguageManager.instance.LangEventCall();
        Debug.Log($"after eventCall {str}");
        UpdateArrow();
    }

    public void UpdateVertUI()
    {
        if (vertIndex < fontList.Count - 1)
        {
            onButton = false;
            foreach (GameObject obj in arrowGroup)
            {
                obj.GetComponent<Image>().sprite = activeArrow;
            }
            fontList[beforeVertIndex].color = deactiveFontColor;
            checkButton.GetComponent<Image>().sprite = deactiveButton;
        }
        else
        {
            onButton = true;
            foreach (GameObject obj in arrowGroup)
            {
                obj.GetComponent<Image>().sprite = deactiveArrow;
            }
            fontList[vertIndex].color = activeFontColor;
            checkButton.GetComponent<Image>().sprite = activeButton;
        }
    }

    public void UpdateArrow()
    {
        if (index >= LanguageGroup.Count - 1)
        {
            arrowGroup[0].SetActive(true);
            arrowGroup[1].SetActive(false);
        }
        else if (index <= 0)
        {
            arrowGroup[0].SetActive(false);
            arrowGroup[1].SetActive(true);
        }
        else
        {
            arrowGroup[0].SetActive(true);
            arrowGroup[1].SetActive(true);
        }
    }
    #endregion
}

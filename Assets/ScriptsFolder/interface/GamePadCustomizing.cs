
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

[Serializable]
public class PadDefaultData
{
    public List<string> pName = new List<string>();
    public List<int> pNumber = new List<int>();
    public List<bool> RT;
    public List<bool> LT;
}

[Serializable]
public class PadSaveData
{
    public List<string> pName = new List<string>();
    public List<int> pNumber = new List<int>();
    public List<bool> RT;
    public List<bool> LT;
}

public class GamePadCustomizing : UIInteract
{
    public List<TextMeshProUGUI> padName = new List<TextMeshProUGUI>();
    [HideInInspector] public List<Image> imageList = new List<Image>();

    public Color deactiveColor;
    public Color activeColor;

    int index, beforeindex;
    bool onHandle, ableChange;

    public TestSettingUI settingUI;

    public Image keySelect;

    float moveValue;
    bool moved;

    KeyCode keyInput = KeyCode.None;
    string getKeyValue = "";

    public Dictionary<KeyCode, string> padKeyName =
        new Dictionary<KeyCode, string>()
        {
            {KeyCode.JoystickButton0, "A" },
            {KeyCode.JoystickButton1, "B"},
            {KeyCode.JoystickButton2, "X"},
            {KeyCode.JoystickButton3, "Y"},
            {KeyCode.JoystickButton4, "LB"},
            {KeyCode.JoystickButton5, "RB"}
        };

    public CustomRecheckUI customRecheck;

    private void Start()
    {
        ResisterLang();
        ChangeLanguage();
    }

    private void OnEnable()
    {
        if (KeySettingManager.instance != null)
        {
            Debug.Log("패드 설정 호출");
            InitPadSetting();
        }
        else
        {
            Debug.Log("패드 설정 호출이 안됨");
        }
    }

    public void InitPadSetting()
    {
        keyChanged = false;

        fontList[index].color = deactiveFontColor;
        keySelect.color = deactiveColor;
        keySelect.transform.position = fontList[index].transform.position;

        PadCode();
    }

    public void PadCode()
    {
        if (KeySettingManager.instance.pSaveData.pName.Count != 0)
        {
            for (int i = 0; i < fontList.Count; i++)
            {
                fontList[i].text = PadCodeText(KeySettingManager.instance.changePadGroup[i]);
                if (KeySettingManager.instance.changeRT[i])
                {
                    fontList[i].text = "RT";
                    SetPadTrigger(i, true, false);
                }
                else if (KeySettingManager.instance.changeLT[i])
                {
                    fontList[i].text = "LT";
                    SetPadTrigger(i, false, true);
                }
            }
        }
        else if (KeySettingManager.instance.pDefaultData.pName.Count != 0)
        {
            for (int i = 0; i < fontList.Count; i++)
            {
                fontList[i].text = PadCodeText(KeySettingManager.instance.defaultPadGroup[i]);
                if (KeySettingManager.instance.defaultRT[i])
                {
                    fontList[i].text = "RT";
                    SetPadTrigger(i, true, false);
                }
                else if (KeySettingManager.instance.defaultLT[i])
                {
                    fontList[i].text = "LT";
                    SetPadTrigger(i, false, true);
                }
            }
        }

        onHandle = true;
    }

    public string PadCodeText(KeyCode keycode)
    {
        if (padKeyName.TryGetValue(keycode, out string pName))
        {
            //Debug.Log($"{keycode.ToString()}패드 입력,{pName}의 입력이 저장되었습니다");
            return pName;
        }
        //else  Debug.Log($"{keycode.ToString()}의 입력이 제대로 이루어지지 않았습니다");

        return "NONE";
    }

    public void SetPadTrigger(int index, bool rt, bool lt)
    {
        switch (index)
        {
            case 0:
                KeySettingManager.instance.atkRT = rt;
                KeySettingManager.instance.atkLT = lt;
                break;
            case 1:
                KeySettingManager.instance.jumpRT = rt;
                KeySettingManager.instance.jumpLT = lt;
                break;
            case 2:
                KeySettingManager.instance.downAtkRT = rt;
                KeySettingManager.instance.downAtkLT = lt;
                break;
            case 3:
                KeySettingManager.instance.interactRT = rt;
                KeySettingManager.instance.interactLT = lt;
                break;
            case 4:
                KeySettingManager.instance.dimensionRT = rt;
                KeySettingManager.instance.dimensionLT = lt;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!onHandle) return;

        moveValue = Input.GetAxisRaw("Vertical");

        if (ableChange)
        {
            Debug.Log("changepadsetting 호출");
            ChangePadSetting();
        }
        else if (!ableChange)
        {
            if (moveValue > 0 && !moved)
            {
                moved = true;
                if (index > 0)
                {
                    beforeindex = index;
                    index--;
                    UpdateUI();
                }
            }

            if (moveValue < 0 && !moved)
            {
                moved = true;
                if (index < fontList.Count - 1)
                {
                    beforeindex = index;
                    index++;
                    UpdateUI();
                }
            }

            if (moveValue == 0 && moved)
            {
                moved = false;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                Debug.Log("체크 호출");
                CheckPadSetting();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                if (!CheckNullCode())
                {
                    if(keyChanged)
                        CallRecheckUI();
                    else
                    {
                        gameObject.SetActive(false);
                        onHandle = false;
                        settingUI.ShowChoiceScreen();
                    }

                }
                else
                {
                    Debug.Log("빈칸으로 설정을 완료할 수 없습니다");
                }

            }
        }
    }

    bool keyChanged;

    public void CallRecheckUI()
    {
        Debug.Log("Call Check");
        onHandle = false;
        customRecheck.ActionActive(StartSaveData, CancelSaveData);
    }

    public void StartSaveData()
    {
        gameObject.SetActive(false);
        KeySettingManager.instance.SavePadData();
        settingUI.ShowChoiceScreen();
    }

    public void CancelSaveData()
    {
        gameObject.SetActive(false);
        KeySettingManager.instance.ReturnPadData();
        settingUI.ShowChoiceScreen();
    }

    public bool CheckNullCode()
    {
        for (int i = 0; i < fontList.Count; i++)
        {
            if (fontList[i].text == "")
                return true;
        }

        return false;
    }

    public void UpdateUI()
    {
        keySelect.transform.position = fontList[index].transform.position;
    }

    public void CheckPadSetting()
    {
        ableChange = true;
        keySelect.color = activeColor;
        fontList[beforeindex].color = deactiveFontColor;
        fontList[index].color = activeFontColor;
    }

    public void ChangePadSetting()
    {
        foreach (KeyCode currentPad in Enum.GetValues(typeof(KeyCode)))
        {
            if ((int)currentPad < 330) continue;

            if (Input.GetKeyDown(currentPad))
            {
                if ((int)currentPad > 349)
                {
                    //Debug.Log($"{currentPad}는 불필요합니다");
                    ableChange = false;
                    break;
                }

                Debug.Log("패드입력 호출");
                keyChanged = true;
                ableChange = false;
                keyInput = currentPad;
                SetPadByKeycode(keyInput);
                fontList[index].text = KeySettingManager.instance.SetPadName(keyInput);

                if (!KeySettingManager.instance.changeTrigger && KeySettingManager.instance.sameValue)
                {
                    if (KeySettingManager.instance.changePadGroup[KeySettingManager.instance.changeIndex] == KeyCode.None)
                    fontList[KeySettingManager.instance.changeIndex].text = "";
                }
                
                fontList[index].color = deactiveFontColor;
                keySelect.color = deactiveColor;
            }
        }

        float xboxRT = Input.GetAxisRaw("XboxRT");
        float xboxLT = Input.GetAxisRaw("XboxLT");

        if (xboxRT > 0)
        {
            if (padAxis.TryGetValue("XboxRT", out string tName))
            {
                SetPadByTrigger(tName);
            }
        }

        if (xboxLT > 0)
        {
            if (padAxis.TryGetValue("XboxLT", out string tName))
            {
                SetPadByTrigger(tName);
            }
        }
    }

    Dictionary<string, string> padAxis =
        new Dictionary<string, string>()
        {
            {"XboxRT", "RT"},
            {"XboxLT", "LT"}
        };

    public void SetPadByKeycode(KeyCode padcode)
    {
        switch (index)
        {
            case 0:
                AttackPad(padcode);
                break;
            case 1:
                JumpPad(padcode);                
                break;
            case 2:
                DownAttackPad(padcode);
                break;
            case 3:
                InteractPad(padcode);
                break;
            case 4:
                DimensionPad(padcode);
                break;
            default:
                break;
        }
        //Debug.Log($"beforepadcode {KeySettingManager.instance.beforePadCode}");
        //Debug.Log($"currentIndex {index}, inpputpadcode {padcode}");
        KeySettingManager.instance.ChangePadData(padcode, index, false);
    }

    bool rTrigger, lTrigger;
    string changeAxis = "";
    public void SetPadByTrigger(string axis)
    {
        ableChange = false;
        if (axis == "RT")
        {
            changeAxis = "LT";
            rTrigger = true;
            lTrigger = false;
        }
        else if (axis == "LT")
        {
            changeAxis = "RT";
            lTrigger = true;
            rTrigger = false;
        }

        switch (index)
        {
            case 0:
                KeySettingManager.instance.AttackPadCode = KeyCode.None;
                if (KeySettingManager.instance.atkRT)
                {
                    KeySettingManager.instance.saveRT = true;
                }
                else if(KeySettingManager.instance.atkLT)
                {
                    KeySettingManager.instance.saveLT = true;
                }
                KeySettingManager.instance.atkRT = rTrigger;
                KeySettingManager.instance.atkLT = lTrigger;
                break;
            case 1:
                KeySettingManager.instance.JumpPadCode = KeyCode.None;
                if (KeySettingManager.instance.jumpRT)
                {
                    KeySettingManager.instance.saveRT = true;
                }
                else if (KeySettingManager.instance.jumpLT)
                {
                    KeySettingManager.instance.saveLT = true;
                }
                KeySettingManager.instance.jumpRT = rTrigger;
                KeySettingManager.instance.jumpLT = lTrigger;
                break;
            case 2:
                KeySettingManager.instance.DownAttackPadCode = KeyCode.None;
                if (KeySettingManager.instance.downAtkRT)
                {
                    KeySettingManager.instance.saveRT = true;
                }
                else if (KeySettingManager.instance.downAtkLT)
                {
                    KeySettingManager.instance.saveLT = true;
                }
                KeySettingManager.instance.downAtkRT = rTrigger;
                KeySettingManager.instance.downAtkLT = lTrigger;
                break;
            case 3:
                KeySettingManager.instance.InteractPadCode = KeyCode.None;
                if (KeySettingManager.instance.interactRT)
                {
                    KeySettingManager.instance.saveRT = true;
                }
                else if (KeySettingManager.instance.interactLT)
                {
                    KeySettingManager.instance.saveLT = true;
                }
                KeySettingManager.instance.interactRT = rTrigger;
                KeySettingManager.instance.interactLT = lTrigger;
                break;
            case 4:
                KeySettingManager.instance.dimensionPadCode = KeyCode.None;
                if (KeySettingManager.instance.dimensionRT)
                {
                    KeySettingManager.instance.saveRT = true;
                }
                else if (KeySettingManager.instance.dimensionLT)
                {
                    KeySettingManager.instance.saveLT = true;
                }
                KeySettingManager.instance.dimensionRT = rTrigger;
                KeySettingManager.instance.dimensionLT = lTrigger;
                break;
            default:
                break;
        }

       

        KeySettingManager.instance.ChangePadData(KeySettingManager.instance.beforePadCode, index, true, axis);
        keySelect.color = deactiveColor;
        fontList[index].color = deactiveFontColor;

        fontList[index].text = axis;
        if (KeySettingManager.instance.changeTrigger && KeySettingManager.instance.sameTrigger)
        {
            if (KeySettingManager.instance.changePadGroup[KeySettingManager.instance.changeIndex] == KeyCode.None)
            fontList[KeySettingManager.instance.changeIndex].text = "";
        }
        keyChanged = true;
    }

    #region 패드 키입력 설정
    public void AttackPad(KeyCode keycode)
    {
        KeySettingManager.instance.beforePadCode = KeySettingManager.instance.AttackPadCode;
        KeySettingManager.instance.AttackPadCode = keycode;       
        KeySettingManager.instance.atkRT = false;
        KeySettingManager.instance.atkLT = false;
    }

    public void JumpPad(KeyCode keycode)
    {
        KeySettingManager.instance.beforePadCode = KeySettingManager.instance.JumpPadCode;
        KeySettingManager.instance.JumpPadCode = keycode;
        KeySettingManager.instance.jumpRT = false;
        KeySettingManager.instance.jumpLT = false;
    }

    public void DownAttackPad(KeyCode keycode)
    {
        KeySettingManager.instance.beforePadCode = KeySettingManager.instance.DownAttackPadCode;
        KeySettingManager.instance.DownAttackPadCode = keycode;
        KeySettingManager.instance.downAtkRT= false;
        KeySettingManager.instance.downAtkLT = false;
    }

    public void DimensionPad(KeyCode keycode)
    {
        KeySettingManager.instance.beforePadCode = KeySettingManager.instance.dimensionPadCode;
        KeySettingManager.instance.dimensionPadCode = keycode;
        KeySettingManager.instance.dimensionRT = false;
        KeySettingManager.instance.dimensionLT = false;
    }

    //public void SkillPad(KeyCode keycode)
    //{
    //    KeySettingManager.instance.SkillPadCode = keycode;
    //    KeySettingManager.instance.skillRT = false;
    //    KeySettingManager.instance.skillLT = false;
    //}

    public void InteractPad(KeyCode keycode)
    {
        KeySettingManager.instance.beforePadCode = KeySettingManager.instance.InteractPadCode;
        KeySettingManager.instance.InteractPadCode = keycode;
        KeySettingManager.instance.interactRT = false;
        KeySettingManager.instance.interactLT = false;
    }
    #endregion

    public bool ChangeXboxPadSetting(KeyCode keycode)
    {
        bool check = false;

        if (padKeyName.TryGetValue(keycode, out string padValue))
        {
            getKeyValue = padValue;
        }

        return check;
    }
    #region 언어변경
    [Header("게임패드 타이틀")]
    public TextMeshProUGUI titleFont;

    public void ResisterLang()
    {
        LanguageManager.instance.LanguageEventResister(ChangeLanguage);
    }

    public void ChangeLanguage()
    {
        if (LanguageManager.instance.isKor)
        {
            titleFont.text = LanguageManager.instance.padSetKor[0];
            titleFont.characterSpacing = LanguageManager.instance.padsetSpacingKor[0];

            for (int i = 0; i < fontList.Count; i++)
            {
                padName[i].text = LanguageManager.instance.padSetKor[i+1];
                padName[i].characterSpacing = LanguageManager.instance.padsetSpacingKor[i + 1];
            }
        }
        else
        {
            titleFont.text = LanguageManager.instance.padSetEng[0];
            titleFont.characterSpacing = LanguageManager.instance.padsetSpacingEng[0];

            for (int i = 0; i < fontList.Count; i++)
            {
                padName[i].text = LanguageManager.instance.padSetEng[i + 1];
                padName[i].characterSpacing = LanguageManager.instance.padsetSpacingEng[i + 1];
            }
        }
    }
    #endregion
}

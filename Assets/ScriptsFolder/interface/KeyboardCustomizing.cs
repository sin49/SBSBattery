using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class KeyboardCustomizing : UIInteract
{
    //public List<Image> imageList;
    public List<TextMeshProUGUI> keyName = new List<TextMeshProUGUI>();

    public Color deactiveColor;
    public Color activeColor;

    int index, beforeIndex;
    bool onHandle, ableSetting;

    public TestSettingUI settingUI;

    public Image keySelect;
    public CustomRecheckUI customRecheck;


    private void OnEnable()
    {
        fontList[index].color = deactiveFontColor;//
        keySelect.color = deactiveColor;
        keySelect.transform.position = fontList[index].transform.position;
        //keySelect.transform.SetParent(fontList[index].transform);
        if (KeySettingManager.instance != null)
            InitKeySettingText();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (KeySettingManager.instance != null)
            InitKeySettingText();
        ResisterLang();
        ChangeLanguage();
    }
    string s = "Arrow";
    public void InitKeySettingText()
    {
        fontList[0].text = KeySettingManager.instance.upKeycode.ToString();
        fontList[1].text = KeySettingManager.instance.downKeycode.ToString();
        fontList[2].text = KeySettingManager.instance.rightKeycode.ToString();
        fontList[3].text = KeySettingManager.instance.leftKeycode.ToString();
        fontList[4].text = KeySettingManager.instance.AttackKeycode.ToString();
        fontList[5].text = KeySettingManager.instance.jumpKeycode.ToString();
        fontList[6].text = KeySettingManager.instance.DownAttackKeycode.ToString();
        fontList[7].text = KeySettingManager.instance.InteractKeycode.ToString();
        fontList[8].text = KeySettingManager.instance.DimensionChangeKeycode.ToString();

        for (int i = 0; i < fontList.Count; i++)
        {
            if (fontList[i].text == "none")
            {
                fontList[i].text = "";
            }
            else if (fontList[i].text.Contains(s))
            {
                fontList[i].text = fontList[i].text.Substring(0, fontList[i].text.Length-s.Length);
            }
        }

        onHandle = true;
        keyChecked = false;
    }
    bool moved;
    float moveValue;
    // Update is called once per frame
    void Update()
    {
        if (!onHandle) return;

        moveValue = Input.GetAxisRaw("Vertical");

        if (ableSetting)
        {
            CurrentKeyInput();
        }


        if (!ableSetting)
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
            }

            if (Input.GetKeyUp(KeyCode.UpArrow) || moveValue == 0)
                moved = false;

            if ((Input.GetKeyDown(KeyCode.DownArrow) || moveValue < 0) && !moved)
            {
                moved = true;
                if (index < fontList.Count - 1)
                {
                    beforeIndex = index;
                    index++;
                    UpdateUI();
                }
            }

            if (Input.GetKeyUp(KeyCode.DownArrow) || moveValue == 0)
                moved = false;

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                CheckSetting();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                if (!CheckNullCode())
                {
                    if(keyChecked)
                        CallRecheckUI();
                    else
                    {
                        onHandle = false;
                        gameObject.SetActive(false);
                        settingUI.ShowChoiceScreen();
                    }

                    //onHandle = false;
                    //gameObject.SetActive(false);
                    //settingUI.ShowChoiceScreen();
                }
                else
                {
                    Debug.Log("설정 불가능 UI 만들어도 될 듯");
                }
            }

        }
    }

    public void CallRecheckUI()
    {
        onHandle = false;
        customRecheck.ActionActive(StartSaveKey, CancelSaveKey);
    }

    public void StartSaveKey()
    {
        KeySettingManager.instance.SaveKeyData();
        gameObject.SetActive(false);
        settingUI.ShowChoiceScreen();
    }

    public void CancelSaveKey()
    {
        KeySettingManager.instance.ReturnKeyData();
        gameObject.SetActive(false);
        settingUI.ShowChoiceScreen();
    }

    bool keyChecked;

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
        //imageList[beforeIndex].color = deactiveColor;
        //imageList[index].color = activeColor;
        keySelect.transform.position = fontList[index].transform.position;
        keySelect.transform.SetParent(fontList[index].transform);
    }

    public void CheckSetting()
    {
        ableSetting = true;
        //fontList[index].color = activeFontColor;
        //imageList[index].color = activeColor;
        keySelect.color = activeColor;
        fontList[index].color = activeFontColor;
    }

    KeyCode currentKey = KeyCode.None;

    public void CurrentKeyInput()
    {
        foreach (KeyCode keyInput in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (keyInput == KeyCode.Return || keyInput == KeyCode.Escape || ((int)keyInput >= 330 && (int)keyInput <= 509))
            {
                Debug.Log("esc키 혹은 패드입력을 받지 않습니다");
                continue;
            }

            Debug.Log("BBBBB");

            if (Input.GetKeyDown(keyInput))
            {
                Debug.Log($"keyinput값 : {keyInput}");
                keyChecked = true;
                ableSetting = false;
                currentKey = keyInput;
                ChangeKeyCode();
                KeySettingManager.instance.ChangeKeyData(currentKey, index);
                InitKeyText();
            }
        }
    }

    public void ChangeKeyCode()
    {
        fontList[index].text = currentKey.ToString();
        keySelect.color = deactiveColor;
        fontList[index].color = deactiveFontColor;
        //keySelect.color = originColor;
        switch (index)
        {
            case 0:
                KeySettingManager.instance.upKeycode = currentKey;
                break;
            case 1:
                KeySettingManager.instance.downKeycode = currentKey;
                break;
            case 2:
                KeySettingManager.instance.rightKeycode = currentKey;
                break;
            case 3:
                KeySettingManager.instance.leftKeycode = currentKey;
                break;
            case 4:
                KeySettingManager.instance.AttackKeycode = currentKey;
                break;
            case 5:
                KeySettingManager.instance.jumpKeycode = currentKey;
                break;
            case 6:
                KeySettingManager.instance.DownAttackKeycode = currentKey;
                break;           
            case 7:
                KeySettingManager.instance.InteractKeycode = currentKey;
                break;
            case 8:
                KeySettingManager.instance.DimensionChangeKeycode = currentKey;
                break;            
            default:
                Debug.Log("인덱스 범위 초과");
                break;
        }
    }

    public void InitKeyText()
    {
        Debug.Log(KeySettingManager.instance.sameValue);
        string a = currentKey.ToString();
        if (a.Contains(s))
            fontList[index].text = a.Substring(0, a.Length - s.Length);
        else
            fontList[index].text = a;
        keySelect.color = deactiveColor;
        if (KeySettingManager.instance.sameValue)
        {
            if (KeySettingManager.instance.changeKeyGroup[KeySettingManager.instance.changeIndex] == KeyCode.None)
                fontList[KeySettingManager.instance.changeIndex].text = "";
        }
    }

    public void ResisterLang()
    {
        LanguageManager.instance.LanguageEventResister(ChangeLanguage);
    }

    [Header("키설정 타이틀")]
    public TextMeshProUGUI titleFont;

    public void ChangeLanguage()
    {
        if (LanguageManager.instance.isKor)
        {
            titleFont.text = LanguageManager.instance.keySetKor[0];
            titleFont.characterSpacing = LanguageManager.instance.keysetSpacingKor[0];

            for (int i = 0; i < fontList.Count; i++)
            {
                keyName[i].text = LanguageManager.instance.keySetKor[i+1];
                keyName[i].characterSpacing = LanguageManager.instance.keysetSpacingEng[i + 1];
            }
        }
        else
        {
            titleFont.text = LanguageManager.instance.keySetEng[0];
            titleFont.characterSpacing = LanguageManager.instance.keysetSpacingEng[0];

            for (int i = 0; i < fontList.Count; i++)
            {
                keyName[i].text = LanguageManager.instance.keySetEng[i+1];
                keyName[i].characterSpacing = LanguageManager.instance.keysetSpacingEng[i + 1];
            }
        }

    }
}

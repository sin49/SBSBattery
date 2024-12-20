
using System;
using System.Collections.Generic;
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
    public List<Image> imageList = new List<Image>();

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
            {KeyCode.Joystick1Button0, "A" },
            {KeyCode.Joystick1Button1, "B"},
            {KeyCode.Joystick1Button2, "X"},
            {KeyCode.Joystick1Button3, "Y"},
            {KeyCode.Joystick1Button5, "RB"},
            {KeyCode.Joystick1Button7, "LB"},
        };

    private void OnEnable()
    {
        if (KeySettingManager.instance != null)
        {
            InitPadSetting();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (KeySettingManager.instance != null)
        {
            InitPadSetting();
        }
    }

    public void InitPadSetting()
    {
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
                fontList[index].text = PadCodeText(KeySettingManager.instance.changePadGroup[i]);
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
                fontList[index].text = PadCodeText(KeySettingManager.instance.defaultPadGroup[i]);
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
            return pName;
        }
        else  Debug.Log($"{keycode.ToString()}의 입력이 제대로 이루어지지 않았습니다");

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
            if(Input.GetKeyDown(KeyCode.Joystick1Button0))
                ChangePadSetting();
        }
        else
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

            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                CheckPadSetting();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                onHandle = false;
                gameObject.SetActive(false);
                settingUI.ShowChoiceScreen();
            }
        }
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
        foreach (KeyCode currentPad in System.Enum.GetValues(typeof(KeyCode)))
        {
            if ((int)currentPad < 330) continue;

            if (Input.GetKeyDown(currentPad))
            {
                keyInput = currentPad;
                if (ChangeXboxPadSetting(keyInput))
                {
                    ableChange = false;
                    SetPadByKeycode(keyInput);
                    fontList[index].text = KeySettingManager.instance.SetPadName(keyInput);
                    keySelect.color = deactiveColor;
                    keyInput = KeyCode.None;
                }
                else
                {
                    Debug.Log("존재하는 패드 입력이 아닙니다");
                    continue;
                }
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
                DimensionPad(padcode);
                break;
            case 3:
                SkillPad(padcode);
                break;
            case 4:
                DownAttackPad(padcode);
                break;
            case 5:
                InteractPad(padcode);
                break;
            default:
                break;
        }
    }

    bool rTrigger, lTrigger;

    public void SetPadByTrigger(string axis)
    {
        ableChange = false;
        fontList[index].text = axis;
        keySelect.color = deactiveColor;
        if (axis == "RT")
        {
            rTrigger = true;
            lTrigger = false;
        }
        else if (axis == "LT")
        {
            lTrigger = true;
            rTrigger = false;
        }

        CheckTrigger(axis);

        switch (index)
        {
            case 0:
                KeySettingManager.instance.atkRT = rTrigger;
                KeySettingManager.instance.atkLT = lTrigger;
                break;
            case 1:
                KeySettingManager.instance.jumpRT = rTrigger;
                KeySettingManager.instance.jumpLT = lTrigger;
                break;
            case 2:
                KeySettingManager.instance.dimensionRT = rTrigger;
                KeySettingManager.instance.dimensionLT = lTrigger;
                break;
            case 3:
                KeySettingManager.instance.skillRT = rTrigger;
                KeySettingManager.instance.skillLT = lTrigger;
                break;
            case 4:
                KeySettingManager.instance.downAtkRT = rTrigger;
                KeySettingManager.instance.downAtkLT = lTrigger;
                break;
            case 5:
                KeySettingManager.instance.interactRT = rTrigger;
                KeySettingManager.instance.interactLT = lTrigger;
                break;
            default:
                break;
        }

        KeySettingManager.instance.SavePadData();
        keySelect.color = deactiveColor;
        fontList[index].color = deactiveFontColor;
    }

    #region 패드 키입력 설정
    public void AttackPad(KeyCode keycode)
    {
        KeySettingManager.instance.AttackPadCode = keycode;
        KeySettingManager.instance.atkRT = false;
        KeySettingManager.instance.atkLT = false;
    }

    public void JumpPad(KeyCode keycode)
    {
        KeySettingManager.instance.JumpPadCode = keycode;
        KeySettingManager.instance.jumpRT = false;
        KeySettingManager.instance.jumpLT = false;
    }

    public void DownAttackPad(KeyCode keycode)
    {
        KeySettingManager.instance.DownAttackPadCode = keycode;
        KeySettingManager.instance.downAtkRT= false;
        KeySettingManager.instance.downAtkLT = false;
    }

    public void DimensionPad(KeyCode keycode)
    {
        KeySettingManager.instance.dimensionPadCode = keycode;
        KeySettingManager.instance.dimensionRT = false;
        KeySettingManager.instance.dimensionLT = false;
    }

    public void SkillPad(KeyCode keycode)
    {
        KeySettingManager.instance.SkillPadCode = keycode;
        KeySettingManager.instance.skillRT = false;
        KeySettingManager.instance.skillLT = false;
    }

    public void InteractPad(KeyCode keycode)
    {
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

    public void CheckTrigger(string t)
    {
        switch (t)
        {
            case "RT":
                ChangeTrigger(KeySettingManager.instance.changeRT);
                break;
            case "LT":
                ChangeTrigger(KeySettingManager.instance.changeLT);
                break;
            default:
                Debug.Log("패드 트리거 검사가 정상 실행되지 않았습니다");
                break;
        }
    }

    public void ChangeTrigger(List<bool> tGroup)
    {
        for (int i = 0; i < tGroup.Count; i++)
        {
            tGroup[i] = false;
        }
    }
}

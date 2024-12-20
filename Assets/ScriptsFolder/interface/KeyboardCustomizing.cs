using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardCustomizing : UIInteract
{
    //public List<Image> imageList;

    public Color deactiveColor;
    public Color activeColor;

    int index, beforeIndex;
    bool onHandle, ableSetting;

    public TestSettingUI settingUI;

    public Image keySelect;

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
    }

    public void InitKeySettingText()
    {
        fontList[0].text = KeySettingManager.instance.AttackKeycode.ToString();
        fontList[1].text = KeySettingManager.instance.jumpKeycode.ToString();
        fontList[2].text = KeySettingManager.instance.DimensionChangeKeycode.ToString();
        //fontList[3].text = KeySettingManager.instance.SkillKeycode.ToString();
        fontList[4].text = KeySettingManager.instance.DownAttackKeycode.ToString();
        fontList[5].text = KeySettingManager.instance.InteractKeycode.ToString();

        onHandle = true;
    }
    bool moved;
    float moveValue;
    // Update is called once per frame
    void Update()
    {
        if (!onHandle) return;

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

            if (Input.GetKeyDown(KeyCode.Return))
            {
                CheckSetting();
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
            }

            if (Input.GetKeyDown(keyInput))
            {
                currentKey = keyInput;
                ChangeKeyCode();
                KeySettingManager.instance.SaveKeyData();
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
                KeySettingManager.instance.AttackKeycode = currentKey;
                break;
            case 1:
                KeySettingManager.instance.jumpKeycode = currentKey;
                break;
            case 2:
                KeySettingManager.instance.DimensionChangeKeycode = currentKey;
                break;
            //case 3:
            //    KeySettingManager.instance.SkillKeycode = currentKey;
            //    break;
            case 4:
                KeySettingManager.instance.DownAttackKeycode = currentKey;
                break;
            case 5:
                KeySettingManager.instance.InteractKeycode = currentKey;
                break;
            case 6:
                KeySettingManager.instance.DeformKeycode = currentKey;
                break;
            default:
                Debug.Log("인덱스 범위 초과");
                break;
        }
    }

    public void InitKeyText()
    {
        fontList[index].text = currentKey.ToString();
        keySelect.color = deactiveColor;
    }
}

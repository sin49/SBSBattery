using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardCustomizing : UIInteract
{
    public List<Image> imageList;

    public Color deactiveColor;
    public Color activeColor;

    int index, beforeIndex;
    bool onHandle, ableSetting;

    public TestSettingUI settingUI;
    
    private void OnEnable()
    {
        fontList[index].color = deactiveFontColor;

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
        fontList[3].text = KeySettingManager.instance.SkillKeycode.ToString();
        fontList[4].text = KeySettingManager.instance.DownAttackKeycode.ToString();
        fontList[5].text = KeySettingManager.instance.InteractKeycode.ToString();
        fontList[6].text = KeySettingManager.instance.DeformKeycode.ToString();

        onHandle = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (onHandle) return;

        if (ableSetting)
        {
            CurrentKeyInput();
        }


        if (!ableSetting)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (index > 0)
                {
                    beforeIndex = index;
                    index--;
                    UpdateUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (index < imageList.Count - 1)
                {
                    beforeIndex = index;
                    index++;
                    UpdateUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                CheckSetting();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                onHandle = false;
                gameObject.SetActive(false);
            }

        }        
    }

    public void UpdateUI()
    {
        imageList[beforeIndex].color = deactiveColor;
        imageList[index].color = activeColor;
    }

    public void CheckSetting()
    {
        ableSetting = true;
        fontList[index].color = activeFontColor;
        imageList[index].color = activeColor;
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
                ableSetting = false;
                currentKey = keyInput;
                ChangeKeyCode();
            }
        }
    }

    public void ChangeKeyCode()
    {
        fontList[index].text = currentKey.ToString();
        
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
            case 3:
                KeySettingManager.instance.SkillKeycode = currentKey;
                break;
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
}

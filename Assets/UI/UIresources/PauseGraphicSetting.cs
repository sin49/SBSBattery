using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class PauseGraphicSetting : UIInteract
{
    public TestSettingUI choiceSetUI;

    public List<GameObject> graphicList = new List<GameObject>();
    public GameObject screw;

    [Header("해상도 관련")]
    public TextMeshProUGUI resolutionTMP;
    public List<string> resolutionString = new List<string>();
    public List<Resolution> resolutionList = new List<Resolution>();

    [Header("화면모드 관련")]
    public TextMeshProUGUI screenTMP;
    public List<string> screenString = new List<string>();

    int index, beforeIndex;
    int resolutionIndex, screenIndex;

    bool onButton, graphicActive;
    public Sprite activeArrow, deactiveArrow;
    public Sprite activeButton, deactiveButton;

    public List<GameObject> buttonList;
    
    private void OnEnable()
    {
        InitGraphicSetting();
    }


    public void InitGraphicSetting()
    {
        foreach (GameObject obj in buttonList)
        {            
            obj.SetActive(true);            
        }

        if (index > graphicList.Count - 3)
            onButton = true;

        switch (beforeIndex)
        {
            case 0:
            case 1:
                graphicList[beforeIndex].GetComponent<Image>().sprite = deactiveArrow;
                break;
            case 2:
            case 3:
                DeactiveButton();
                break;
            default:
                Debug.Log("out of range");
                break;
        }

        switch (index)
        {
            case 0:
            case 1:
                graphicList[index].GetComponent<Image>().sprite = activeArrow;
                break;
            case 2:
            case 3:
                ActiveButton();
                break;
            default:
                Debug.Log("out of range");
                break;
        }
        graphicActive = true;
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

            if ((Input.GetKeyDown(KeyCode.LeftArrow) || horiValue < 0) && !horiMoved)
            {
                horiMoved = true;
                if (!onButton)
                {
                    switch (index)
                    {
                        case 0:
                            if (resolutionIndex > 0)
                                resolutionIndex--;
                            UpdateResolutionUI();
                            break;
                        case 1:
                            if (screenIndex > 0)
                                screenIndex--;
                            UpdateScreenUI();
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
                            if (resolutionIndex < resolutionString.Count - 1)
                                resolutionIndex++;
                            UpdateResolutionUI();
                            break;
                        case 1:
                            if (screenIndex < screenString.Count - 1)
                                screenIndex++;
                            UpdateScreenUI();
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

            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Space) 
                || Input.GetKeyDown(KeyCode.JoystickButton0)|| Input.GetKeyDown(KeyCode.Return))
            {
                SelectSetting();
            }

        }        
    }

    public void SelectSetting()
    {
        switch (index)
        {
            case 0:
                Debug.Log("수직동기화 기능 구현해야함");
                break;
            case 1:
                Debug.Log("화면모드 적용 기능 구현해야함");
                break;
            case 2:
                SaveGraphicSetting();
                break;
            case 3:
                CurrentSettingExit();
                break;
        }
    }

    public void SaveGraphicSetting()
    {
        ChoiceVSyncMode();
        ChoiceScreenMode();
    }

    public void ChoiceVSyncMode()
    {
        switch (resolutionIndex)
        {
            case 0:
                QualitySettings.vSyncCount = 0;
                break;
            case 1:
                QualitySettings.vSyncCount = 1;
                break;
        }

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

    }

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
        }
    }

    public void SetIndex(int n)
    {
        if (index == n) return;
        beforeIndex = index;
        index = n;
        UpdateUI();
    }

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
    //해상도 갱신 준비
    public void UpdateResolutionUI()
    {
        resolutionTMP.text = resolutionString[resolutionIndex];
    }
    //화면 모드 갱신 준비
    public void UpdateScreenUI()
    {
        screenTMP.text = screenString[screenIndex];
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

    public void ResolutionDecrease()
    {
        if (resolutionIndex > 0)
        {
            resolutionIndex--;
            UpdateResolutionUI();
        }
    }

    public void ResolutionIncrease()
    {
        if (resolutionIndex < resolutionString.Count - 1)
        {
            resolutionIndex++;
            UpdateResolutionUI();
        }
    }

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
}

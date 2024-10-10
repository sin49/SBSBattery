using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseGraphicSetting : MonoBehaviour
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
                graphicList[beforeIndex].GetComponent<Image>().sprite = deactiveButton;
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
                graphicList[index].GetComponent<Image>().sprite = activeButton;
                break;
            default:
                Debug.Log("out of range");
                break;
        }
        graphicActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (graphicActive)
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
                if (onButton)
                    return;
                if (index < graphicList.Count - 1)
                {
                    beforeIndex = index;
                    index++;
                    UpdateUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
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

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
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

            if (Input.GetKeyDown(KeyCode.C))
            {
                switch (index)
                {
                    case 0:
                        Debug.Log("해상도 적용 기능 구현해야함");
                        break;
                    case 1:
                        Debug.Log("화면모드 적용 기능 구현해야함");
                        break;
                    case 2:
                        Debug.Log("해상도 및 화면모드 저장하는 기능 구현해야함");
                        break;
                    case 3:
                        CurrentSettingExit();
                        break;
                }
            }

        }        
    }
    //현재 화면에서 나감
    public void CurrentSettingExit()
    {
        graphicActive = false;
        onButton = false;
        foreach (GameObject obj in buttonList)
        {
            obj.SetActive(false);
            obj.GetComponent<Image>().sprite = deactiveButton;
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
            if (!onButton)
            {
                onButton = true;
                screw.SetActive(true);
            }
            screw.transform.SetParent(graphicList[index].transform.GetChild(1));
            screw.transform.position = graphicList[index].transform.GetChild(1).position;
            graphicList[index].GetComponent<Image>().sprite = activeButton;
        }
        else
        {
            if (onButton)
            {
                onButton = false;
                screw.SetActive(false);
            }
            graphicList[index].GetComponent<Image>().sprite = activeArrow;
        }

        if (beforeIndex > graphicList.Count - 3)
        {
            graphicList[beforeIndex].GetComponent<Image>().sprite = deactiveButton;
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
}

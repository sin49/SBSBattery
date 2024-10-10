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

    [Header("�ػ� ����")]
    public TextMeshProUGUI resolutionTMP;
    public List<string> resolutionString = new List<string>();
    public List<Resolution> resolutionList = new List<Resolution>();

    [Header("ȭ���� ����")]
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
                        Debug.Log("�ػ� ���� ��� �����ؾ���");
                        break;
                    case 1:
                        Debug.Log("ȭ���� ���� ��� �����ؾ���");
                        break;
                    case 2:
                        Debug.Log("�ػ� �� ȭ���� �����ϴ� ��� �����ؾ���");
                        break;
                    case 3:
                        CurrentSettingExit();
                        break;
                }
            }

        }        
    }
    //���� ȭ�鿡�� ����
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
    // ������ ������ �̵�
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
    //�ػ� ���� �غ�
    public void UpdateResolutionUI()
    {
        resolutionTMP.text = resolutionString[resolutionIndex];
    }
    //ȭ�� ��� ���� �غ�
    public void UpdateScreenUI()
    {
        screenTMP.text = screenString[screenIndex];
    }
}

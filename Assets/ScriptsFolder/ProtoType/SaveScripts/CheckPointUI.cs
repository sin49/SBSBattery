using Autodesk.Fbx;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointUI : UIInteract
{
    public List<Button> buttonList = new List<Button>();
    public List<Image> checkList = new List<Image>();
    public List<GameObject> choiceList = new List<GameObject>();

    public GameObject buttonPanel;

    int index, beforeIndex;

    public bool onHandle;

    public Sprite activeButton, deactiveButton;

    private void OnEnable()
    {
        InitSaveUI();
    }

    private void OnDisable()
    {
        onHandle = false;
        buttonPanel.SetActive(false);
    }

    public void InitSaveUI()
    {
        onHandle = true;
        buttonPanel.SetActive(true);

        checkList[beforeIndex].sprite = deactiveButton;
        fontList[beforeIndex].color = deactiveFontColor;

        checkList[index].sprite = activeButton;
        fontList[beforeIndex].color = deactiveFontColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!onHandle) return;

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (index > 0)
            {
                beforeIndex = index;
                index--;
                UpButtonCheck();
                UpdateUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (index < checkList.Count - 1)
            {
                beforeIndex = index;
                index++;
                DownButtonCheck();
                UpdateUI();
            }
            beforeIndex = index;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C))
        {
            SelectButton();
        }

    }

    public void UpButtonCheck()
    {
        while (true)
        {
            if (buttonList[index].interactable) return;
            index--;
        }

    }

    public void DownButtonCheck()
    {
        while (true)
        {
            if (buttonList[index].interactable) return;
            index++;
        }
    }

    // 마우스 클릭 구현 시 사용
    public void OnClickButton()
    {

    }

    public void UpdateUI()
    {
        checkList[beforeIndex].sprite = deactiveButton;
        fontList[beforeIndex].color = deactiveFontColor;
        checkList[index].sprite = activeButton;
        fontList[index].color = activeFontColor;
    }
    
    public void SelectButton()
    {
        onHandle = false;
        switch (index)
        {
            case 0:
                ActiveChoiceListUI();
                break;
            case 1:
                ActiveChoiceListUI();
                break;
            case 2:
                ActiveChoiceListUI();
                break;
            case 3:
                ActiveChoiceListUI();
                break;
            case 4:
                ActiveChoiceListUI();
                break;
            case 5:                
                CheckListExit();
                break;
            default:
                break;
        }
    }

    public void ActiveChoiceListUI()
    {
        choiceList[index].SetActive(true);
        buttonPanel.SetActive(false);
    }

    public void CheckListExit()
    {
        gameObject.SetActive(false);
    }

    public void ReturnFromChoiceUI()
    {
        onHandle = true;
        buttonPanel.SetActive(true);
        UpdateUI();
    }
}

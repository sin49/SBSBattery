using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.UI;

public class ChoiceCheckPointUI : UIInteract
{
    public CheckPointUI checkPointUI;

    public Image stageIcon;

    public bool onHandle;

    public int currentIndex;

    public int stageCount;

    public GameObject currentStageButton;
    public List<CheckList> checkLists = new List<CheckList>();
    public Sprite activeButton, deactiveButton;
    int index, beforeIndex;

    List<Image> buttonList = new List<Image>();

    private void OnEnable()
    {
        InitCheckPointButton();
    }

    private void OnDisable()
    {
        onHandle = false;
        checkLists.Clear();
        buttonList.Clear();
        fontList.Clear();
    }

    public void InitCheckPointButton()
    {
        //currentStageButton.SetActive(true);
        checkLists = currentStageButton.GetComponentsInChildren<CheckList>().ToList();
        fontList = currentStageButton.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        
        for (int i = 0; i < checkLists.Count; i++)
        {
            buttonList.Add(checkLists[i].GetComponent<Image>());
        }

        buttonList[index].sprite = activeButton;
        currentIndex = checkLists[index].checkStageIndex;

        onHandle = true;
    }
    bool moved;
    float moveValue;
    // Update is called once per frame
    void Update()
    {
        if (!onHandle) return;

        moveValue = Input.GetAxisRaw("Vertical");
        if ((Input.GetKeyUp(KeyCode.UpArrow) || moveValue > 0) && !moved)
        {
            moved = true;
            if (index > 0)
            {
                beforeIndex = index;
                index--;
                UpdateUI();
                checkPointUI.SelectSound();
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || moveValue == 0)
            moved = false;

        if ((Input.GetKeyDown(KeyCode.DownArrow) || moveValue < 0) && !moved)
        {
            moved = true;
            if (index < checkLists.Count - 1)
            {
                beforeIndex = index;
                index++;
                ButtonInteractCheck();
                UpdateUI();
                checkPointUI.SelectSound();
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) || moveValue == 0)
            moved = false;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.C) 
            || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Space))
        {
            SelectCheckPoint();
            checkPointUI.ActiveSound();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            CheckListExit();
            checkPointUI.DeactiveSound();
        }

    }

    public void ButtonInteractCheck()
    {
        if (!buttonList[index].GetComponent<Button>().interactable)
            index--;
    }

    public void UpdateUI()
    {
        checkLists[beforeIndex].GetComponent<Image>().sprite = deactiveButton;
        fontList[beforeIndex].color = deactiveFontColor;
        checkLists[index].GetComponent<Image>().sprite = activeButton;
        fontList[index].color = activeFontColor;
        currentIndex = checkLists[index].checkStageIndex;
    }

    public void SelectCheckPoint()
    {
        onHandle = false;
        Time.timeScale = 1;
        GameManager.instance.LoadChoiceCheckPoint(currentIndex);
    }

    public void CheckListExit()
    {
        onHandle = false;

        checkLists[index].GetComponent<Image>().sprite = deactiveButton;
        
        beforeIndex = index = 0;

        checkPointUI.ReturnFromChoiceUI();
        currentStageButton.SetActive(false);
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceCheckPointUI : UIInteract
{
    public CheckPointUI checkPointUI;

    public Image stageIcon;

    public bool onHandle;

    public int currentIndex;

    public int stageCount;

    public GameObject currentStageButton;
    public List<GameObject> stageGroup = new List<GameObject>();
    public List<CheckList> checkLists = new List<CheckList>();
    public Sprite activeButton, deactiveButton;
    int index, beforeIndex;

    List<Image> buttonList = new List<Image>();

    private void OnEnable()
    {
        checkLists = currentStageButton.GetComponentsInChildren<CheckList>().ToList();
        fontList = currentStageButton.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        for (int i = 0; i < checkLists.Count; i++)
        {
            buttonList.Add(checkLists[i].GetComponent<Image>());
            checkLists[i].GetComponent<Button>().onClick.AddListener(SelectCheckPoint);
            checkLists[i].GetComponent<Button>().onClick.AddListener(checkPointUI.ActiveSound);
        }

        buttonList[index].sprite = activeButton;
        fontList[index].color = activeFontColor;
        currentIndex = checkLists[index].checkStageIndex;

        onHandle = true;
    }

    private void OnDisable()
    {
        onHandle = false;
        //checkLists.Clear();
        //buttonList.Clear();
        //fontList.Clear();
    }

    //public void InitCheckPointButton()
    //{
    //    checkLists = currentStageButton.GetComponentsInChildren<CheckList>().ToList();
    //    fontList = currentStageButton.GetComponentsInChildren<TextMeshProUGUI>().ToList();


    //    for (int i = 0; i < checkLists.Count; i++)
    //    {
    //        buttonList.Add(checkLists[i].GetComponent<Image>());
    //        checkLists[i].GetComponent<Button>().onClick.AddListener(SelectCheckPoint);
    //        checkLists[i].GetComponent<Button>().onClick.AddListener(checkPointUI.ActiveSound);
    //        //checkLists[i].gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
    //        //UnityEngine.EventSystems.EventTrigger.Entry entry
    //        //    = new UnityEngine.EventSystems.EventTrigger.Entry();
    //        //entry.eventID = EventTriggerType.PointerEnter;
    //        //entry.callback.AddListener((data) => OnPointerEnter(i));
    //        //Debug.Log($"ÀÎµ¦½º °ª : {i}");
    //        //checkLists[i].GetComponent<UnityEngine.EventSystems.EventTrigger>().triggers.Add(entry);
    //    }
    //}

    private void OnPointerEnter(int n)
    {
        Debug.Log($"trigger index {n}");
        Debug.Log("´­·¶½À´Ï´Ù");
        checkPointUI.SelectSound();
        SetIndex(n);
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
            || Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
        {
            SelectCheckPoint();
            checkPointUI.ActiveSound();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            CheckListExit();
            checkPointUI.DeactiveSound();
        }

    }

    public void SetIndex(int n)
    {
        beforeIndex = index;
        index = n;
        UpdateUI();
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
        GameManager.instance.mouseTimeMove = true;
        GameManager.instance.LoadChoiceCheckPoint(currentIndex);
        gameObject.SetActive(false);
    }

    public void CheckListExit()
    {
        onHandle = false;
        checkLists[index].GetComponent<Image>().sprite = deactiveButton;
        beforeIndex = index = 0;

        checkPointUI.ReturnFromChoiceUI();
        currentStageButton.SetActive(false);
        gameObject.SetActive(false);

        checkLists.Clear();
        fontList.Clear();
        buttonList.Clear();
    }
}

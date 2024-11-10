using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.UI;

public class ChoiceCheckPointUI : MonoBehaviour
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
        onHandle = true;
        InitCheckPointButton();
    }

    private void OnDisable()
    {
        onHandle = false;
        checkLists.Clear();
    }

    public void InitCheckPointButton()
    {
        //currentStageButton.SetActive(true);
        for (int i = 0; i < currentStageButton.transform.childCount; i++)
        {
            checkLists.Add(currentStageButton.transform.GetChild(i).GetComponent<CheckList>());
        }
        
        for (int i = 0; i < checkLists.Count; i++)
        {
            buttonList.Add(checkLists[i].GetComponent<Image>());
        }

        buttonList[beforeIndex].sprite = deactiveButton;
        buttonList[index].sprite = activeButton;
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
                UpdateUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (index < checkLists.Count - 1)
            {
                beforeIndex = index;
                index++;
                UpdateUI();
            }
            beforeIndex = index;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Space))
        {
            SelectCheckPoint();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CheckListExit();
        }

    }

    public void UpdateUI()
    {
        checkLists[beforeIndex].GetComponent<Image>().sprite = deactiveButton;
        checkLists[index].GetComponent<Image>().sprite = activeButton;
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
        checkPointUI.ReturnFromChoiceUI();
        currentStageButton.SetActive(false);
        gameObject.SetActive(false);
    }
}

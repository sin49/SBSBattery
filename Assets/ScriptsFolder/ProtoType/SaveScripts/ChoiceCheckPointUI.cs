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

    public List<CheckList> checkLists = new List<CheckList>();
    public Sprite activeButton, deactiveButton;
    int index, beforeIndex;
    private void OnEnable()
    {
        onHandle = true;
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
            if (index < stageCount - 1)
            {
                beforeIndex = index;
                index++;
                UpdateUI();
            }
            beforeIndex = index;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Space))
        {

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }

    }

    public void UpdateUI()
    {
        checkLists[beforeIndex].GetComponent<Image>().sprite = deactiveButton;
        checkLists[index].GetComponent<Image>().sprite = activeButton;
    }

    public void SelectCheckPoint()
    {
        GameManager.instance.LoadChoiceCheckPoint(currentIndex);
    }

    public void CheckListExit()
    {
        checkPointUI.ReturnFromChoiceUI();
        gameObject.SetActive(false);
    }
}

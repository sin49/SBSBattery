using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomRecheckUI : UIInteract
{

    public List<GameObject> buttonList = new List<GameObject>();

    public Sprite activeButton, deactiveButton;

    int index, beforeIndex;

    bool onHandle;

    Action yesEvent;
    Action noEvent;


    private void Awake()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponent<Button>().onClick.AddListener(ChoiceButton);
        }
        ResisterChange();
    }

    private void Start()
    {
        ChangeLanguage();
    }

    private void OnEnable()
    {
        InitCustomRecheck();
    }

    public void InitCustomRecheck()
    {
        buttonList[index].GetComponent<Image>().sprite= deactiveButton;
        fontList[index].color = deactiveFontColor;

        index = 0;
        buttonList[index].GetComponent<Image>().sprite = activeButton;
        fontList[index].color = activeFontColor;
    }

    float moveValue;
    bool moved;

    private void Update()
    {
        if (!onHandle) return;

        if (Input.GetKey(KeySettingManager.instance.rightKeycode))
            moveValue = 1;
        else if (Input.GetKey(KeySettingManager.instance.leftKeycode))
            moveValue = -1;
        else
        moveValue = Input.GetAxisRaw("Horizontal");

        if (moveValue > 0 && !moved)
        {
            moved = true;
            if (index < buttonList.Count-1)
            {
                beforeIndex = index;
                index++;
                UpdateUI();
            }

        }

        if (moveValue < 0 && !moved)
        {
            if (index > 0)
            {
                beforeIndex = index;
                index--;
                UpdateUI();
            }
        }

        if (moveValue == 0 && moved)
        {
            moved = false;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Debug.Log("리첵크 호출 확인");
            ChoiceButton();
        }

    }

    public void UpdateUI()
    {
        buttonList[beforeIndex].GetComponent<Image>().sprite = deactiveButton;
        fontList[beforeIndex].color = deactiveFontColor;

        buttonList[index].GetComponent<Image>().sprite = activeButton;
        fontList[index].color = activeFontColor;
    }

    public void ChoiceButton()
    {
        switch (index)
        {
            case 0:
                yesEvent?.Invoke();
                yesEvent = null;
                EndActionEvent();
                break;
            case 1:
                noEvent?.Invoke();
                noEvent = null;
                EndActionEvent();
                break;
        }
    }

    public void ActionActive(Action yesButton, Action noButton)
    {
        yesEvent = noEvent = null;
        gameObject.SetActive(true);
        onHandle = true;
        yesEvent += yesButton;        
        noEvent += noButton;
        Debug.Log($"yes count {yesEvent.GetInvocationList().Length}, no count {noEvent.GetInvocationList().Length}");
    }
    
    public void EndActionEvent()
    {
        gameObject.SetActive(false);
        onHandle = false;
    }

    public void SetIndex(int n)
    {
        if (index == n) return;
        beforeIndex = index;
        index = n;
        UpdateUI();
    }

    public TextMeshProUGUI mainTMP;

    public void ResisterChange()
    {
        LanguageManager.instance.LanguageEventResister(ChangeLanguage);
    }

    public void ChangeLanguage()
    {
        if (LanguageManager.instance.isKor)
        {
            mainTMP.text = LanguageManager.instance.recheckKor[0];
            fontList[0].text = LanguageManager.instance.recheckKor[1];
            fontList[1].text = LanguageManager.instance.recheckKor[2];
        }
        else
        {
            mainTMP.text = LanguageManager.instance.recheckEng[0];
            fontList[0].text = LanguageManager.instance.recheckEng[1];
            fontList[1].text = LanguageManager.instance.recheckEng[2];
        }
    }
}

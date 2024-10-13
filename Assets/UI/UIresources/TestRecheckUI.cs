using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestRecheckUI : UIInteract
{
    public TextMeshProUGUI Description;

    public Transform SelectedUI;
    public Transform YesButton;
    public Transform NoButton;

    public bool ok;
    public bool reCheckActive;

    public event Action OKEvent;
    public event Action CancelEvent;

    public Sprite activeButton, deactiveButton;
    public List<GameObject> buttonList = new List<GameObject>();
    int index, beforeIndex;

    public Animator recheckAnimator;

    public TitleScreen title;
    public SelectUI selectui;
    private void Awake()
    {
        //if (this.gameObject.activeSelf)
        //    this.gameObject.SetActive(false);
    }
    public void ActiveUI(string Desc, Action OKEvent, Action CancelEvent)
    {
        Description.text = Desc;
        this.OKEvent += OKEvent;
        this.CancelEvent += CancelEvent;
        gameObject.SetActive(true);
        Debug.Log("UI 활성화");
    }
    void initializeUI()
    {
        ok = true;
        StartCoroutine(SettingChangeReCheck());
        beforeIndex = index;
        index = 1;
        UpdateUI();
    }
    private void OnEnable()
    {
        initializeUI();
    }


    IEnumerator SettingChangeReCheck()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        if (recheckAnimator.GetCurrentAnimatorStateInfo(0).IsName("SettingUI"))
        {
            while (recheckAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            reCheckActive = true;
        }
    }

    void UpdateUI()
    {
        SelectedUI.gameObject.SetActive(true);

        /*if (ok)
            SelectedUI.transform.position = YesButton.transform.position;
        else
            SelectedUI.transform.position = NoButton.transform.position;*/
        switch(index)
        {
            case 0:
                SelectedUI.SetParent(YesButton);
                SelectedUI.position = YesButton.position;
                break;
            case 1:
                SelectedUI.SetParent(NoButton);
                SelectedUI.position = NoButton.position;
                break;
        }
        DeactiveButton();
        ActiveButton();
    }
    void OkButtonInput()
    {
        if (SceneManager.GetActiveScene().name != "CheckTitleTest" && SceneManager.GetActiveScene().name != "TitleTest")
        {
            OKEvent?.Invoke();
            DeActiveUI();
        }
        else
            SaveDeleteInTitle();
    }
    void CancelButtonInput()
    {
        if (SceneManager.GetActiveScene().name != "CheckTitleTest" && SceneManager.GetActiveScene().name != "TitleTest")
        {
            CancelEvent?.Invoke();
            DeActiveUI();
        }
        else
            RecheckExitTitle();
    }
    void handleUI()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!ok)
                ok = true;
            if (index > 0)
            {
                beforeIndex = index;
                index--;
                UpdateUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (ok)
                ok = false;
            if (index < buttonList.Count - 1)
            {
                beforeIndex = index;
                index++;
                UpdateUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.C) && reCheckActive)
        {
            if (ok)
                OkButtonInput();
            else
                CancelButtonInput();
        }

    }
    private void OnDisable()
    {
        reCheckActive = false;
        CancelButtonInput();
    }
    void DeActiveUI()
    {

        CancelEvent = null;
        OKEvent = null;
        this.gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name != "CheckTitleTest" && SceneManager.GetActiveScene().name != "TitleTest")
            selectui.pauseui.pauseInteract = true;
        Debug.Log("UI 비활성화");
    }
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf && reCheckActive)
            handleUI();
    }

    public void ActiveButton()
    {
        buttonList[index].GetComponent<Image>().sprite = activeButton;
        fontList[index].color = activeFontColor;
    }
    
    public void DeactiveButton()
    {
        buttonList[beforeIndex].GetComponent<Image>().sprite = deactiveButton;
        fontList[beforeIndex].color = deactiveFontColor;
    }

    public void RecheckExitTitle()
    {
        if (SceneManager.GetActiveScene().name == "TitleTest" || SceneManager.GetActiveScene().name == "CheckTitleTest")
        {
            reCheckActive = false;
            gameObject.SetActive(false);
            title.SettingBackScreen();
        }
        else
        {
            Debug.Log("타이틀이 아닙니다");
        }
    }

    public void SaveDeleteInTitle()
    {
        reCheckActive = false;
        title.DeleteData();
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    public bool OnHandle;
    public GameObject SelectedUI;

    public PauseUI pauseui;
    public ItemListUI itemlistui;
    public TestRecheckUI testRecheckUI;
    public TextMeshProUGUI tokenText;

    int index, beforeIndex;


    bool buttonselected;

    public List<GameObject> ButtonList = new List<GameObject>();
    public List<TextMeshProUGUI> fontList = new List<TextMeshProUGUI>();

    [Header("코인관련 UI")] public GameObject coinPanel;
    [Header("일시정지 UI")] public GameObject pauseIconPanel;
    //void initlizeUI()
    //{
    //    index = 0;
    //    OnHandle = true;
    //    UpdateUI();
    //}

    public GameObject uiGroup;
    public GameObject settingUI;
    public GameObject checkPointUI;

    public Animator uiAnimator;
    public Animator screwAnimator;

    public Color originColor;
    public Color choiceColor;
    Vector3 choiceScale = new(1.1f, 1.1f, 1.1f);
    Vector3 originScale = new(1, 1, 1);

    Vector2 screwScale = new(.75f, .75f);

    public bool uiGroupActive;
    public bool settingActive;
    public bool reCheckActive;


    void swapUI()
    {
        ; OnHandle = false;
        itemlistui.gameObject.SetActive(true);
        itemlistui.ActiveItemListUI();
        UpdateUI();
        uiGroup.gameObject.SetActive(false);
        uiGroupActive = false;
    }

    public void ActiveUI(int index = 0)
    {
        if(GameManager.instance != null)
        GameManager.instance.mouseTimeMove = false;
        Cursor.lockState = CursorLockMode.None;
        pauseui.pauseInteract = false;
        //tokenText.text = PlayerInventory.instance.TokenValue.ToString();
        this.index = index;
        ShowPauseUI();

        //UpdateUI();
    }

    //public void DeactiveUI()
    //{

    //    initlizeUI();
    //    //pauseui.ReturnPauseUI();
    //}
    void TitleBackEvent()
    {
        Time.timeScale = 1;
        GameManager.instance.mouseTimeMove = true;
        //GameManager.instance.LoadingSceneWithKariEffect("TitleTest");
        GameManager.instance.LoadingSceneWithKariEffect("CheckTitleTest");
    }
    void ExitEvent()
    {
        Application.Quit();
    }
    void ButtonselectedDisable()
    {
        buttonselected = false;
    }

    public void MouseClick()
    {
        pauseui.ButtonSoundEffectPlayer_.PlayActiveAudio();
        SelectButton();
    }

    public void SelectButton()
    {
        if (buttonselected)
            return;
        switch (index)
        {
            case 0:
                ResumeGame();
                break;
            case 1:
                //Time.timeScale = 1;
                //GameManager.instance.LoadLastCheckPoint();
                Debug.Log("체크포인트 선택하기");
                StartCheckPointUI();
                break;
            case 2:
                swapUI();
                pauseui.pauseInteract = false;
                break;
            case 3:
                ShowSettingUI();
                coinPanel.SetActive(false);
                pauseIconPanel.SetActive(false);
                pauseui.pauseInteract = false;
                break;
            case 4://재확인 시키기
                ButtonList[index].GetComponent<Image>().color = originColor;
                ButtonList[index].transform.localScale = originScale;
                SelectedUI.SetActive(false);

                testRecheckUI.ActiveUI("타이틀로 돌아갑니다.", TitleBackEvent, ButtonselectedDisable);
                pauseui.pauseInteract = false;
                buttonselected = true;
                break;
            case 5://재확인 시키기
                ButtonList[index].GetComponent<Image>().color = originColor;
                ButtonList[index].transform.localScale = originScale;
                SelectedUI.SetActive(false);

                testRecheckUI.ActiveUI("게임을 종료합니다.", ExitEvent, ButtonselectedDisable);
                pauseui.pauseInteract = false;
                buttonselected = true;
                break;
        }
    }
    public void setindex(int n)
    {
        beforeIndex = index;
        index = n;
        UpdateUI();
        pauseui.ButtonSoundEffectPlayer_.PlaySelectAudio();
    }

    public void RecheckBackSetting()
    {
        ButtonList[index].GetComponent<Image>().color = choiceColor;
        ButtonList[index].transform.localScale = choiceScale;
        SelectedUI.SetActive(true);
        SelectedUI.transform.SetParent(ButtonList[index].transform.GetChild(1));
        SelectedUI.transform.position = ButtonList[index].transform.GetChild(1).position;
        SelectedUI.transform.localScale = screwScale;
    }

    void UpdateUI()
    {

        if (OnHandle)
        {
            SelectedUI.SetActive(true);
            SelectedUI.transform.SetParent(ButtonList[index].transform.GetChild(1));
            SelectedUI.transform.position = ButtonList[index].transform.GetChild(1).position;
            screwAnimator.Play("ScrewRotate", 0, 0f);

            DeInteractUI();
            SelectedUI.transform.localScale = screwScale;
            InteractUI();
            SelectedUI.transform.localScale = screwScale;
        }
        else
        {
            SelectedUI.SetActive(false);
            ButtonList[index].GetComponent<Image>().color = originColor;
            ButtonList[index].transform.localScale = originScale;
        }
        //SelectedUI.transform.position = ButtonList[index].transform.position;
    }

    private void OnDisable()
    {
        OnHandle = false;

        ButtonList[index].GetComponent<Image>().color = originColor;
        ButtonList[index].transform.localScale = originScale;
    }
    public void ResumeGame()
    {
        pauseui.PauseUiActive();
    }
    private void OnEnable()
    {
        ////initlizeUI();
        ActiveUI();
    }
    public bool moved;
    float moveValue;
    // Update is called once per frame
    void Update()
    {
        if (!OnHandle || buttonselected)
            return;
        if (uiGroupActive && !settingActive)
        {
            if(Input.GetKey(KeySettingManager.instance.upKeycode))
            {
                moveValue = 1;
            }
            else if(Input.GetKey(KeySettingManager.instance.downKeycode))
            {
                moveValue = -1;
            }
            else
            moveValue = Input.GetAxisRaw("Vertical");

            if ((Input.GetKeyDown(KeyCode.UpArrow) || moveValue > 0) && !moved)
            {
                moved = true;
                if (index > 0)
                {
                    beforeIndex = index;
                    index--;
                    UpdateUI();
                }
            }

            if (Input.GetKeyUp(KeyCode.UpArrow) || moveValue == 0)
                moved = false;
            
            /*if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                swapUI();
            }*/
            if ((Input.GetKeyDown(KeyCode.DownArrow) || moveValue < 0) && !moved)
            {
                moved = true;
                if (index < ButtonList.Count - 1)
                {
                    beforeIndex = index;
                    index++;
                    UpdateUI();
                }
            }

            if (Input.GetKeyUp(KeyCode.DownArrow) || moveValue == 0)
                moved = false;

            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Space) 
                || Input.GetKeyDown(KeyCode.Joystick1Button0)|| Input.GetKeyDown(KeyCode.Return))
            {
                SelectButton();
            }
        }
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    DeactiveUI();
        //}
    }    

    #region 추가작업
    public void ShowPauseUI()
    {
        uiAnimator.Play("ShowPauseUI");
        StartCoroutine(StartUiGroup());
    }

    IEnumerator StartUiGroup()
    {
        Debug.Log("첫 선택 UI 들어옴");
        yield return new WaitForSecondsRealtime(0.1f);

        Debug.Log(uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShowPauseUI"));
        if (uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShowPauseUI"))
        {
            while (uiAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            uiGroupActive = true;
            UpdateUI();
            SelectedUI.gameObject.SetActive(true);
            pauseui.pauseInteract = true;
            OnHandle = true;
        }
    }

    public void PauseBackSetting()
    {
        uiAnimator.Play("PauseBackSetting");
        StartCoroutine(PBS());
    }

    IEnumerator PBS()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("PauseBackSetting"))
        {
            Debug.Log("설정에서 일시정지 UI로");
            while (uiAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
            settingActive = false;
            pauseui.pauseInteract = true;
            SelectedUI.transform.SetParent(ButtonList[index].transform.GetChild(1));
            SelectedUI.transform.position = ButtonList[index].transform.GetChild(1).position;
        }
    }


    public void KJSUpdateUI()
    {
        /*if (settingActive)
            return;*/
        SelectedUI.transform.SetParent(ButtonList[index].transform.GetChild(1));
        SelectedUI.transform.position = ButtonList[index].transform.GetChild(1).position;
        screwAnimator.Play("ScrewRotate", 0, 0f);

        DeInteractUI();
        SelectedUI.transform.localScale = originScale;

        InteractUI();
        SelectedUI.transform.localScale = choiceScale;
    }

    public void InteractUI()
    {
        ButtonList[index].GetComponent<Image>().color = choiceColor;
        ButtonList[index].transform.localScale = choiceScale;
    }

    public void DeInteractUI()
    {
        ButtonList[beforeIndex].GetComponent<Image>().color = originColor;
        ButtonList[beforeIndex].transform.localScale = originScale;
    }

    public void ShowSettingUI()
    {
        settingActive = true;
        SelectedUI.SetActive(false);
        uiAnimator.Play("PauseChangeSetting");
        StartCoroutine(StartSettingUi());
    }

    public void StartCheckPointUI()
    {
        OnHandle = false;
        pauseui.pauseInteract = false;
        coinPanel.SetActive(false);
        pauseIconPanel.SetActive(false);
        uiAnimator.Play("PauseChangeSetting");
        StartCoroutine(ShowCheckPointUi());
    }

    IEnumerator ShowCheckPointUi()
    {
        Debug.Log("사운드/해상도 선택 UI 들어옴");
        yield return new WaitForSecondsRealtime(0.1f);

        if (uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("PauseChangeSetting"))
        {
            while (uiAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                //Debug.Log(uiAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                yield return null;
            }

            uiGroup.SetActive(false);
            checkPointUI.SetActive(true);
        }
    }

    IEnumerator StartSettingUi()
    {
        Debug.Log("사운드/해상도 선택 UI 들어옴");
        yield return new WaitForSecondsRealtime(0.1f);

        if (uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("PauseChangeSetting"))
        {
            while (uiAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                Debug.Log(uiAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                yield return null;
            }

            settingUI.SetActive(true);
            uiGroup.SetActive(false);
        }
    }
    #endregion
}

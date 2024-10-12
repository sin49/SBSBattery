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
    public List<TextMeshProUGUI> fontList= new List<TextMeshProUGUI>();

    //void initlizeUI()
    //{
    //    index = 0;
    //    OnHandle = true;
    //    UpdateUI();
    //}

    public GameObject uiGroup;
    public GameObject settingUI;

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
        tokenText.text = PlayerInventory.instance.TokenValue.ToString();
        OnHandle = true;
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
        GameManager.instance.LoadingSceneWithKariEffect("TitleTest");
    }
    void ExitEvent()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    void ButtonselectedDisable()
    {
        buttonselected = false;
    }
    void SelectButton()
    {
        if (buttonselected)
            return;
        switch (index)
        {
            case 0:
                ResumeGame();
                break;
            case 1:
                Time.timeScale = 1;
                GameManager.instance.LoadingSceneWithKariEffect(GameManager.instance.LoadLastestStage());
                break;
            case 2:
                swapUI();
                break;
            case 3:
                ShowSettingUI();
                break;
            case 4://재확인 시키기
                testRecheckUI.ActiveUI("타이틀로 돌아갑니다.", TitleBackEvent, ButtonselectedDisable);
                buttonselected = true;
                break;
            case 5://재확인 시키기
                testRecheckUI.ActiveUI("게임을 종료합니다.", ExitEvent, ButtonselectedDisable);
                buttonselected = true;
                break;
        }
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
    // Update is called once per frame
    void Update()
    {
        if (!OnHandle || buttonselected)
            return;
        if (uiGroupActive && !settingActive)
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
            /*if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                swapUI();
            }*/
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {

                if (index < ButtonList.Count - 1)
                {
                    beforeIndex = index;
                    index++;
                    UpdateUI();
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
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

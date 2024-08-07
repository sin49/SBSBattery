using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public bool OnHandle;
    public GameObject SelectedUI;

    public PauseUI pauseui;
    public RecheckUI recheckui;
    public ItemListUI itemlistui;


    int index;

   
    bool buttonselected;

    public List<GameObject> ButtonList=new List<GameObject>();
  
    //void initlizeUI()
    //{
    //    index = 0;
    //    OnHandle = true;
    //    UpdateUI();
    //}
    void swapUI()
    {
        itemlistui.ActiveItemListUI();
        ;OnHandle = false;
        UpdateUI();
    }
    public void ActiveUI(int index=0)
    {
        
        OnHandle = true;
        this.index = index;
        UpdateUI();
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
        Application.Quit();
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
                Debug.Log("안 만듬");
                break;
            case 3://재확인 시키기
                recheckui.ActiveUI("타이틀로 돌아갑니다.", TitleBackEvent, ButtonselectedDisable);
                buttonselected = true;
                break;
            case 4://재확인 시키기
                recheckui.ActiveUI("게임을 종료합니다.", ExitEvent, ButtonselectedDisable);
                buttonselected = true;
            
                break;
        }
    }
  void UpdateUI()
    {
  
        if (OnHandle)
            SelectedUI.SetActive(true);
        else
            SelectedUI.SetActive(false);
        SelectedUI.transform.position = ButtonList[index].transform.position;


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
        if (!OnHandle||buttonselected)
            return;
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (index > 0)
            {
                index--;
                UpdateUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            swapUI();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (index < ButtonList.Count - 1)
            {
                index++;
                UpdateUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SelectButton();
        }
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    DeactiveUI();
        //}
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckPointUI : UIInteract
{
    public List<Button> buttonList = new List<Button>();
    public List<Image> checkList = new List<Image>();
    public List<GameObject> choiceList = new List<GameObject>();

    [Header("스테이지 선택")]public GameObject buttonPanel;
    [Header("체크포인트 선택")] public GameObject checkPointPanel;

    int index, beforeIndex;

    public bool onHandle;

    public Sprite activeButton, deactiveButton;

    [Header("이건 타이틀 한정이라 그 외 씬에서는 넣는거 금지\n(애초에 타이틀에서만 해당 오브젝트가 있을거임)")]
    public TitleScreen title;
    [Header("일시정지 UI에서 추가")]
    public SelectUI selectUI;

    [Header("체크포인트 타이틀")]
    public TextMeshProUGUI cpTitle;

    [Header("뒤로가기 버튼")]
    public Button backButton;

    private void Awake()
    {
        ResisterLang();
        InitButtonEvent();
    }

    public void InitButtonEvent()
    {
        foreach (Button btn in buttonList)
        {
            btn.onClick.AddListener(SelectButton);
            btn.onClick.AddListener(ActiveSound);
        }

        backButton.onClick.AddListener(OnclickBack);

        gameObject.SetActive(false);
    }
    
    public void OnclickBack()
    {
        CheckListExit();
        DeactiveSound();
    }

    public void SetIndex(int n)
    {
        if (!buttonList[n].interactable) return;

        SelectSound();
        beforeIndex = index;
        index = n;
        UpdateUI();
    }

    private void OnEnable()
    {
        InitSaveUI();
    }

    public void InitSaveUI()
    {
        onHandle = true;
        buttonPanel.SetActive(true);

        checkList[beforeIndex].sprite = deactiveButton;
        fontList[beforeIndex].color = deactiveFontColor;

        checkList[index].sprite = activeButton;
        fontList[index].color = activeFontColor;

        backButton.gameObject.SetActive(true);
        if (backButton.transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI tmp))
        {
            if(LanguageManager.instance.isKor)
            {
                tmp.text = "뒤로가기";
            }
            else
            {
                tmp.text = "Back";
            }
        }
        
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
                SelectSound();
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || moveValue == 0)
            moved = false;

        if ((Input.GetKeyDown(KeyCode.DownArrow) || moveValue < 0) && !moved)
        {
            moved = true;
            if (index < checkList.Count - 1)
            {
                beforeIndex = index;
                index++;
                ButtonInteractCheck();
                UpdateUI();
                SelectSound();
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) || moveValue == 0)
            moved = false;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) 
            || Input.GetKeyDown(KeyCode.JoystickButton0)|| Input.GetKeyDown(KeyCode.C))
        {
            SelectButton();
            ActiveSound();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            CheckListExit();
            DeactiveSound();
        }

    }

    public void ButtonInteractCheck()
    {
        if (!buttonList[index].interactable)
            index--;
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
        ActiveChoiceListUI();
        //if (index <= 3)
        //{
        //    ActiveChoiceListUI();
        //}
        //else
        //{
        //    CheckListExit();
        //}

        //switch (index)
        //{
        //    case 0:
        //        ActiveChoiceListUI();
        //        break;
        //    case 1:
        //        ActiveChoiceListUI();
        //        break;
        //    case 2:
        //        ActiveChoiceListUI();
        //        break;
        //    case 3:
        //        ActiveChoiceListUI();
        //        break;
        //    case 4:
        //        ActiveChoiceListUI();
        //        break;
        //    case 5:                
        //        CheckListExit();
        //        break;
        //    default:
        //        break;
        //}
    }

    public void ActiveChoiceListUI()
    {
        onHandle = false;
        buttonPanel.SetActive(false);
        checkPointPanel.GetComponent<ChoiceCheckPointUI>().currentStageButton = choiceList[index];
        checkPointPanel.SetActive(true);
        choiceList[index].SetActive(true);
        backButton.gameObject.SetActive(false);
    }

    public void CheckListExit()
    {
        onHandle = false;
        gameObject.SetActive(false);
        buttonPanel.SetActive(false);


        if (SceneManager.GetActiveScene().name == "CheckTitleTest")
        {
            title.onHandle = true;
        }
        else
        {
            selectUI.OnHandle = true;
            selectUI.uiGroup.SetActive(true);
            selectUI.PauseBackSetting();

            selectUI.coinPanel.SetActive(true);
            selectUI.pauseIconPanel.SetActive(true);
        }
    }

    public void ReturnFromChoiceUI()
    {
        onHandle = true;
        buttonPanel.SetActive(true);
        UpdateUI();
    }

    public void SelectSound()
    {
        if (CheckTitleScene())
            title.ButtionSoundEffectPlayer_.PlaySelectAudio();
        else
            selectUI.pauseui.ButtonSoundEffectPlayer_.PlaySelectAudio();
    }

    public void ActiveSound()
    {
        if (CheckTitleScene())
            title.ButtionSoundEffectPlayer_.PlayActiveAudio();
        else
            selectUI.pauseui.ButtonSoundEffectPlayer_.PlayActiveAudio();
    }

    public void DeactiveSound()
    {
        if (CheckTitleScene())
            title.ButtionSoundEffectPlayer_.PlayDeActiveAudio();
        else
            selectUI.pauseui.ButtonSoundEffectPlayer_.PlayDeActiveAudio();
    }

    public bool CheckTitleScene()
    {
        if (SceneManager.GetActiveScene().name == "CheckTitleTest")
            return true;
        else
            return false;
            
    }
    #region 언어변경
    public void ResisterLang()
    {
        if (LanguageManager.instance != null)
            LanguageManager.instance.LanguageEventResister(ChangeLanguage);
        else
            Debug.Log("언어 이벤트 호출 안됨");
    }

    public void ChangeLanguage()
    {
        if (LanguageManager.instance.isKor)
        {
            cpTitle.text = LanguageManager.instance.cpKor[0];
            cpTitle.characterSpacing = LanguageManager.instance.cpSpacingKor[0];
        }
        else
        {
            cpTitle.text = LanguageManager.instance.cpEng[0];
            cpTitle.characterSpacing = LanguageManager.instance.cpSpacingEng[0];
        }
    }
    #endregion
}

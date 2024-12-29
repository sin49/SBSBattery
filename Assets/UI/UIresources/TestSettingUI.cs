using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestSettingUI : UIInteract
{
    //public SelectUI uiGroup;
    //[HideInInspector]public TestPauseUI uiGroup;

    public SelectUI uiSelect;
    public TitleScreen title;

    public bool settingActive;

    public List<Image> buttonList = new List<Image>();
    int index, beforeIndex;
    int rangeIndex, beforeRangeIndex;

    public Sprite activeButton;
    public Sprite deactiveButton;

    public Animator settingAnimator;
    Vector3 settingScale = new(0.8f, 0.8f, 0.8f);


    bool choiceSetting;

    public GameObject choice, sound, graphic, keyCustom, padCustom;

    [HideInInspector] public GameObject canvas;

    public List<string> textList = new List<string>();
    public List<float> spacingList = new List<float>();
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {        
        for (int i = 0; i < buttonList.Count; i++)
        {
            fontList[i].text = textList[i];
            fontList[i].characterSpacing = spacingList[i];
        }
        InitButtonUI();
    }

    private void OnDisable()
    {
        index = 0;
        rangeIndex = 0;

        beforeIndex = 0;
        beforeRangeIndex = 0;
    }

    bool moved;
    float movevalue;
    // Update is called once per frame
    void Update()
    {

        if (settingActive)
        {
            if (Input.GetKey(KeySettingManager.instance.upKeycode))
            {
                movevalue = 1;
            }
            else if (Input.GetKey(KeySettingManager.instance.downKeycode))
            {
                movevalue = 1;
            }
            else
            movevalue = Input.GetAxisRaw("Vertical");

            if (!choiceSetting)
            {
                if ((Input.GetKeyDown(KeyCode.UpArrow) || movevalue > 0) && !moved)
                {
                    moved = true;
                    if (index > 0)
                    {
                        beforeIndex = index;
                        index--;

                        beforeRangeIndex = rangeIndex;
                        rangeIndex--;
                        UpdateUI();
                    }                    
                }

                if (Input.GetKeyUp(KeyCode.UpArrow) || movevalue ==0)
                    moved = false;                  

                if ((Input.GetKeyDown(KeyCode.DownArrow) || movevalue < 0) && !moved)
                {
                    moved = true;
                    if (index < textList.Count - 1)
                    {
                        beforeIndex = index;
                        index++;

                        beforeRangeIndex = rangeIndex;
                        rangeIndex++;
                        UpdateUI();
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.DownArrow) || movevalue==0)
                moved = false;


            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Space) || 
                Input.GetKeyDown(KeyCode.Joystick1Button0)|| Input.GetKeyDown(KeyCode.Return))
            {
                ChoiceInteractUI();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                SettingExit();
            }
        }
    }
    public bool CheckTitle()
    {
        bool check = false;
        if (SceneManager.GetActiveScene().name == "CheckTitleTest" || SceneManager.GetActiveScene().name == "TitleTest")
        {
            check = true;
        }

        return check;
    }

    public void ChoiceInteractUI()
    {
        switch (index)
        {
            case 0:
                Debug.Log("소리 설정");
                NextSelectSetting(sound);
                if (SceneManager.GetActiveScene().name != "CheckTitleTest")
                    uiSelect.pauseui.ButtonSoundEffectPlayer_.PlayActiveAudio();
                else
                    title.ButtionSoundEffectPlayer_.PlayActiveAudio();
                break;
            case 1:
                Debug.Log("그래픽 설정");
                NextSelectSetting(graphic);
                if (SceneManager.GetActiveScene().name != "CheckTitleTest")
                    uiSelect.pauseui.ButtonSoundEffectPlayer_.PlayActiveAudio();
                else
                    title.ButtionSoundEffectPlayer_.PlayActiveAudio();
                break;
            case 2:
                NextSelectSetting(keyCustom);
                if (SceneManager.GetActiveScene().name != "CheckTitleTest")
                    uiSelect.pauseui.ButtonSoundEffectPlayer_.PlayActiveAudio();
                else
                    title.ButtionSoundEffectPlayer_.PlayActiveAudio();
                break;
            case 3:
                NextSelectSetting(padCustom);
                if (SceneManager.GetActiveScene().name != "CheckTitleTest")
                    uiSelect.pauseui.ButtonSoundEffectPlayer_.PlayActiveAudio();
                else
                    title.ButtionSoundEffectPlayer_.PlayActiveAudio();
                break;
            case 4:
                SettingExit();
                if (SceneManager.GetActiveScene().name != "CheckTitleTest")
                    uiSelect.pauseui.ButtonSoundEffectPlayer_.PlayDeActiveAudio();
                else
                    title.ButtionSoundEffectPlayer_.PlayDeActiveAudio();
                break;
            default:
                Debug.Log("범위 초과함");
                break;
        }
    }

    public void NextSelectSetting(GameObject selectSetting)
    {
        settingActive = false;
        choiceSetting = true;
        choice.SetActive(false);
        selectSetting.SetActive(true);
    }

    public void SettingExit()
    {
        fontList[rangeIndex].color = deactiveFontColor;
        buttonList[rangeIndex].sprite = deactiveButton;
        settingActive = false;
        if (SceneManager.GetActiveScene().name != "CheckTitleTest" && SceneManager.GetActiveScene().name != "TitleTest")
        {
            settingAnimator.Play("SettingChangePause");
            StartCoroutine(SCP());
        }
        else
        {
            gameObject.SetActive(false);
            title.SettingBackScreen();
        }

    }

    IEnumerator SCP()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        if (settingAnimator.GetCurrentAnimatorStateInfo(0).IsName("SettingChangePause"))
        {
            while (settingAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }

            gameObject.SetActive(false);

            uiSelect.uiGroup.SetActive(true);
            uiSelect.PauseBackSetting();

            uiSelect.coinPanel.SetActive(true);
            uiSelect.pauseIconPanel.SetActive(true);
        }
    }

    public void UpdateUI()
    {
        int count = 0;
        if (rangeIndex < 0)
        {
            rangeIndex = 0;            
            while (count < buttonList.Count)
            {
                fontList[rangeIndex + count].text = textList[index + count];
                count++;
            }
        }
        else if (rangeIndex > buttonList.Count -1)
        {
            rangeIndex = buttonList.Count - 1;
            while (count < buttonList.Count)
            {
                fontList[rangeIndex - count].text = textList[index - count];
                count++;
            }
        }

        DeactiveButton();
        ActiveButton();
    }

    public void setIndex(int n)
    {
        beforeIndex = index;
        index = n;
        UpdateUI();
        if (SceneManager.GetActiveScene().name != "CheckTitleTest")
            uiSelect.pauseui.ButtonSoundEffectPlayer_.PlaySelectAudio();
        else
            title.ButtionSoundEffectPlayer_.PlaySelectAudio();
    }

    public void InitButtonUI()
    {
        beforeIndex = index;
        index = 0;
        DeactiveButton();
        ActiveButton();

        if (SceneManager.GetActiveScene().name != "CheckTitleTest" && SceneManager.GetActiveScene().name != "TitleTest")
        {
            StartCoroutine(EndSettingAnimation());
        }
        else
            settingActive = true;
    }

    IEnumerator EndSettingAnimation()
    {
        while (settingAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        settingActive = true;
    }

    public void ShowChoiceScreen()
    {
        choice.SetActive(true);
        buttonList[beforeRangeIndex].sprite = deactiveButton;
        buttonList[rangeIndex].sprite = activeButton;
        choiceSetting = false;
        settingActive = true;

    }

    public void ActiveButton()
    {
        buttonList[rangeIndex].GetComponent<Image>().sprite = activeButton;
        fontList[rangeIndex].color = activeFontColor;
    }

    public void DeactiveButton()
    {
        buttonList[beforeRangeIndex].GetComponent<Image>().sprite = deactiveButton;
        fontList[beforeRangeIndex].color = deactiveFontColor;
    }
}

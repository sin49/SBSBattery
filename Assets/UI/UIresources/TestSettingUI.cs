using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TestSettingUI : MonoBehaviour
{
    //public SelectUI uiGroup;
    //[HideInInspector]public TestPauseUI uiGroup;

    public SelectUI uiSelect;      

    public bool settingActive;

    public List<Image> buttonList = new List<Image>();
    int index, beforeIndex;

    public Sprite activeButton;
    public Sprite deactiveButton;

    public Animator settingAnimator;
    Vector3 settingScale = new(0.8f, 0.8f, 0.8f);


    bool choiceSetting;

    public GameObject choice, sound, graphic;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {        
        InitButtonUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (settingActive)
        {
            if (!choiceSetting)
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
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (index < buttonList.Count - 1)
                    {
                        beforeIndex = index;
                        index++;
                        UpdateUI();
                    }
                }
            }            

            if (Input.GetKeyDown(KeyCode.C))
            {
                ChoiceInteractUI();
            }
        }
    }

    public void ChoiceInteractUI()
    {
        switch (index)
        {
            case 0:
                Debug.Log("�Ҹ� ����");
                NextSelectSetting(sound);
                break;
            case 1:
                Debug.Log("�׷��� ����");
                NextSelectSetting(graphic);
                break;
            case 2:
                SettingExit();
                break;
            default:
                Debug.Log("���� �ʰ���");
                break;
        }
    }

    public void NextSelectSetting(GameObject selectSetting)
    {
        choiceSetting = true;
        choice.SetActive(false);
        selectSetting.SetActive(true);
    }

    public void SettingExit()
    {
        settingActive = false;
        settingAnimator.Play("SettingChangePause");
        StartCoroutine(SCP());
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
        }
    }

    public void UpdateUI()
    {
        buttonList[beforeIndex].sprite = deactiveButton;
        buttonList[index].sprite = activeButton;
    }

    public void InitButtonUI()
    {
        index = 0;
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].sprite = deactiveButton;
        }        
        buttonList[index].sprite = activeButton;

        StartCoroutine(EndSettingAnimation());
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
        buttonList[beforeIndex].sprite = deactiveButton;
        buttonList[index].sprite = activeButton;
        choiceSetting = false;

    }
}

using Cinemachine.Editor;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PauseSoundSetting : UIInteract
{
    public TestSettingUI choiceSetUI;

    public GameObject screw;

    public List<GameObject> interactList = new List<GameObject>();
    public List<GameObject> volumeSlider = new List<GameObject>();

    public Sprite activeButton, deactivebutton;
    public Sprite activeVolume, deactiveVolume;

    [Range(0, 1)] public float masterSlider;
    [Range(0, 1)] public float bgmSlider;
    [Range(0, 1)] public float seSlider;

    [Range(0, 1)]public float volumeValue;

    int index, beforeIndex;
    bool onButton;

    public List<GameObject> buttonList = new List<GameObject>();

    Vector2 scale = new Vector2(0.7f, 0.7f);

    private void OnEnable()
    {
        InitSoundSetting();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (index < interactList.Count - 1)
            {
                if (onButton)
                {
                    return;
                }
                beforeIndex = index;
                index++;
                UpdateUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {            
            if (index > 0)
            {
                beforeIndex = index;
                index--;
                UpdateUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (onButton)
            {
                if (index > interactList.Count-2)
                {
                    beforeIndex = index;
                    index--;
                    UpdateUI();
                }
            }
            else
            {
                UpdateMinusVolume();
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (onButton)
            {
                if (index < interactList.Count - 1)
                {
                    beforeIndex = index;
                    index++;
                    UpdateUI();
                }
            }
            else
            {
                UpdatePlusVolume();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            switch (index)
            {
                case 0:
                case 1:
                case 2:
                    break;
                case 3:
                    SaveSoundValue();
                    break;
                case 4:
                    SettingCancel();
                    break;
            }
        }
    }

    //사운드 설정 초기화(활성화시)
    public void InitSoundSetting()
    {
        foreach (GameObject obj in buttonList)
        {
            obj.SetActive(true);
        }

        if (index > interactList.Count - 3)
            onButton = true;
        else
        {
            onButton = false;
        }

        switch (beforeIndex)
        {
            case 0:
            case 1:
            case 2:
                interactList[beforeIndex].GetComponent<Image>().sprite = deactiveVolume;
                break;
            case 3:
            case 4:
                screw.SetActive(false);
                DeactiveButton();
                break;
            default:
                Debug.Log("out of range");
                break;
        }

        switch (index)
        {
            case 0:
            case 1:
            case 2:
                interactList[index].GetComponent<Image>().sprite = activeVolume;
                break;
            case 3:
            case 4:                
                screw.SetActive(true);
                ActiveButton();
                break;
            default:
                Debug.Log("out of range");
                break;
        }

        originMaster = AudioManager.instance.MasterVolume;
        originBG = AudioManager.instance.BGVolume;
        originSE = AudioManager.instance.SEVolume;

        masterSlider = PlayerPrefs.GetFloat("LastestMasterVolume", AudioManager.instance.MasterVolume);
        bgmSlider = PlayerPrefs.GetFloat("LastestBgmVolume", AudioManager.instance.BGVolume);
        seSlider = PlayerPrefs.GetFloat("LastestSeVolume", AudioManager.instance.SEVolume);

        SetSlider();
    }

    public void SetSlider()
    {
        volumeSlider[0].transform.localScale = new(masterSlider, 1, 1);
        volumeSlider[1].transform.localScale = new(bgmSlider, 1, 1);
        volumeSlider[2].transform.localScale = new(seSlider, 1, 1);
    }

    public float originMaster, originBG, originSE;
    public void SettingCancel()
    {
        AudioManager.instance.MasterVolume = originMaster;
        AudioManager.instance.BGVolume = originBG;
        AudioManager.instance.SEVolume = originSE;



        masterSlider = originMaster;
        bgmSlider = originBG;
        seSlider = originSE;

        SetSlider();

        CurrentSettingExit();
    }

    //현재 화면에서 나감
    public void CurrentSettingExit()
    {
        onButton = false;
        foreach (GameObject obj in buttonList)
        {
            obj.SetActive(false);            
        }
        switch(index)
        {
            case 0:
            case 1:
            case 2:
                interactList[index].GetComponent<Image>().sprite = deactiveVolume;
                break;
            case 3:
            case 4:
                screw.transform.SetParent(buttonList[index - 3].transform.GetChild(1));
                DeactiveButton();
                break;
        }

        screw.SetActive(false);
        gameObject.SetActive(false);
        choiceSetUI.ShowChoiceScreen();
    }
    //설정 저장
    public void SaveSoundValue()
    {
        AudioManager.instance.MasterVolume = masterSlider;
        AudioManager.instance.BGVolume = bgmSlider;
        AudioManager.instance.SEVolume = seSlider;
        
        PlayerPrefs.SetFloat("LastestMasterVolume", masterSlider);
        PlayerPrefs.SetFloat("LastestBgmVolume", bgmSlider);
        PlayerPrefs.SetFloat("LastestSeVolume", seSlider);

        if (PlayerPrefs.HasKey("LastestMasterVolume") && PlayerPrefs.HasKey("LastestBgmVolume") && PlayerPrefs.HasKey("LastestSeVolume"))
        {
            Debug.Log("사운드 설정값이 저장되었습니다!");
        }

        CurrentSettingExit();
    }

    //설정 갱신
    public void UpdateUI()
    {
        if (index > interactList.Count - 3)
        {
            if (!onButton)
            {
                onButton = true;
                screw.SetActive(true);
                screw.transform.localScale = scale;
            }
            screw.transform.SetParent(interactList[index].transform.GetChild(1));
            screw.transform.position = interactList[index].transform.GetChild(1).position;
            ActiveButton();
        }
        else
        {
            if (onButton)
            {
                onButton = false;
                screw.SetActive(false);
            }
            interactList[index].GetComponent<Image>().sprite = activeVolume;
        }

        if (beforeIndex > interactList.Count - 3)
        {
            DeactiveButton();
        }
        else
        {
            interactList[beforeIndex].GetComponent<Image>().sprite = deactiveVolume;
        }

    }
    //볼륨 증가
    public void UpdatePlusVolume()
    {
        switch (index)
        {
            case 0:
                if(masterSlider < 1)
                masterSlider += volumeValue;
                AudioManager.instance.MasterVolume = masterSlider;
                volumeSlider[index].transform.localScale = new(masterSlider, 1, 1);
                break;
            case 1:
                if(bgmSlider < 1)
                bgmSlider += volumeValue;
                AudioManager.instance.BGVolume = bgmSlider;
                volumeSlider[index].transform.localScale = new(bgmSlider, 1, 1);
                break;
            case 2:
                if(seSlider < 1)
                seSlider += volumeValue;
                AudioManager.instance.SEVolume = seSlider;
                volumeSlider[index].transform.localScale = new(seSlider, 1, 1);
                break;
        }
    }
    //볼륨 감소
    public void UpdateMinusVolume()
    {
        switch (index)
        {
            case 0:
                if(masterSlider > 0)
                masterSlider -= volumeValue;
                AudioManager.instance.MasterVolume = masterSlider;
                volumeSlider[index].transform.localScale = new(masterSlider, 1, 1);
                break;
            case 1:
                if(bgmSlider > 0)
                bgmSlider -= volumeValue;
                AudioManager.instance.BGVolume = bgmSlider;
                volumeSlider[index].transform.localScale = new(bgmSlider, 1, 1);
                break;
            case 2:
                if(seSlider > 0)
                seSlider -= volumeValue;
                AudioManager.instance.SEVolume = seSlider;
                volumeSlider[index].transform.localScale = new(seSlider, 1, 1);
                break;
        }
    }

    public void ActiveButton()
    {
        interactList[index].GetComponent<Image>().sprite = activeButton;
        fontList[index].color = activeFontColor;
    }

    public void DeactiveButton()
    {
        interactList[beforeIndex].GetComponent<Image>().sprite = deactivebutton;
        fontList[beforeIndex].color = deactiveFontColor;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSettingUI : MonoBehaviour
{
    //public SelectUI uiGroup;
    //[HideInInspector]public TestPauseUI uiGroup;
    public SelectUI uiGroup;

    public List<Image> settingVolume = new List<Image>();
    public List<Image> settingButton= new List<Image>();
    int buttonIndex, beforeButtonIndex;
    int volumeIndex, beforeVolumeIndex;
    public GameObject screw;

    bool settingActive;

    public Sprite choiceButton;
    public Sprite originButton;

    public Sprite choiceVolume;
    public Sprite originVolume;

    public Animator settingAnimator;
    Vector3 settingScale = new(0.8f, 0.8f, 0.8f);

    bool uiVolume, uiButton;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        screw.SetActive(true);
        InitVolumeUI();
        InitButtonUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(settingActive)
        {
            /*if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                beforeVolumeIndex = volumeIndex;
                if (volumeIndex <= 0)
                {
                    volumeIndex = settingVolume.Count - 1;
                }
                else
                    volumeIndex--;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                beforeButtonIndex = buttonIndex;
                if (volumeIndex >= settingVolume.Count)
                {
                    volumeIndex = 0;
                }
                else
                    volumeIndex++;
            }*/

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                beforeButtonIndex = buttonIndex;
                if (buttonIndex <= 0)
                    buttonIndex = settingButton.Count - 1;
                else
                    buttonIndex--;
                UpdateSettingButton();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                beforeButtonIndex = buttonIndex;
                if (buttonIndex >= settingButton.Count - 1)
                    buttonIndex = 0;
                else
                    buttonIndex++;
                UpdateSettingButton();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (screw.activeSelf)
                {
                    switch (buttonIndex)
                    {
                        case 0:
                            SettingSaveButton();
                            break;
                        case 1:
                            SettingSaveButton();
                            break;
                    }
                }                
            }
        }
    }

    public void SettingSaveButton()
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
            uiGroup.uiGroup.SetActive(true);
            uiGroup.PauseBackSetting();
        }
    }

    public void UpdateSettingVolume()
    {

    }

    public void InitVolumeUI()
    {
        for (int i = 0; i < settingVolume.Count; i++)
        {

        }
    }

    public void UpdateSettingButton()
    {
        screw.transform.SetParent(settingButton[buttonIndex].transform.GetChild(1));
        screw.transform.position = settingButton[buttonIndex].transform.GetChild(1).position;

        settingButton[buttonIndex].sprite = choiceButton;
        settingButton[beforeButtonIndex].sprite = originButton;
    }

    public void InitButtonUI()
    {
        settingActive = true;
        for (int i = 0; i < settingButton.Count; i++)
        {
            settingButton[i].sprite = originButton;
        }
    }


    public void store()
    {
        screw.SetActive(true);
        screw.transform.localScale = settingScale;
        screw.transform.SetParent(settingButton[buttonIndex].transform.GetChild(1));
        screw.transform.position = settingButton[buttonIndex].transform.GetChild(1).position;
        settingButton[buttonIndex].sprite = choiceButton;
    }   
}

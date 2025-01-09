using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : UIInteract
{
    public List<GameObject> uiList = new List<GameObject>();

    public List<Image> buttonList = new List<Image>();

    public Sprite deactiveButton, activeButton;

    [Range(0, 3)] public float activeTimer;

    public bool onHandle;
    int index, beforeIndex;

    public Animator animator;

    private void Awake()
    {
        InitDeactive();
        ResisterLang();
        ChangeLanguage();
    }

    public void InitDeactive()
    {

        for (int i = 0; i < uiList.Count; i++)
        {
            uiList[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(activeGameOverUI());
    }

    private void OnDisable()
    {
        onHandle = false;

        buttonList[index].sprite = deactiveButton;
        fontList[index].color = deactiveFontColor;
    }

    IEnumerator activeGameOverUI()
    {
        index = 0;
        yield return new WaitForSecondsRealtime(0.1f);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ActiveGameOver"))
        {
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            }

            yield return new WaitForSecondsRealtime(activeTimer);

            buttonList[index].sprite = activeButton;
            fontList[index].color = activeFontColor;

            onHandle = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (onHandle)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (index < buttonList.Count - 1)
                {
                    beforeIndex = index;
                    index++;
                    UpdateUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (index > 0)
                {
                    beforeIndex = index;
                    index--;
                    UpdateUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                ChoiceGameOverButton();
            }
        }
    }

    public void SetIndex(int n)
    {
        beforeIndex = index;
        index = n;
        UpdateUI();        
    }

    public void ChoiceGameOverButton()
    {
        onHandle = false;
        switch (index)
        {
            case 0:
                Debug.Log("다시하기 실행");
                Time.timeScale = 1;
                GameManager.instance.LoadLastCheckPoint();
                break;
            case 1:
                Debug.Log("타이틀로 돌아가기 실행");
                Time.timeScale = 1;
                GameManager.instance.LoadingSceneWithKariEffect("CheckTitleTest");
                break;
        }
    }

    public void UpdateUI()
    {
        if (onHandle)
        {
            buttonList[index].sprite = activeButton;
            buttonList[beforeIndex].sprite = deactiveButton;
            fontList[index].color = activeFontColor;
            fontList[beforeIndex].color = deactiveFontColor;
        }
    }

    public void ResisterLang()
    {
        LanguageManager.instance.LanguageEventResister(ChangeLanguage);
    }

    public void ChangeLanguage()
    {
        if (LanguageManager.instance.isKor)
        {
            for (int i = 0; i < fontList.Count; i++)
            {
                fontList[i].text = LanguageManager.instance.gameoverKor[i];
            }
        }
        else
        {
            for (int i = 0; i < fontList.Count; i++)
            {
                fontList[i].text = LanguageManager.instance.gameoverEng[i];
            }
        }
    }
}

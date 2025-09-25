using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchInterface : MonoBehaviour
{
    public GameObject buttonParent;
    public GameObject joystick;

    TextMeshProUGUI[] fontList;
    public Color fontColor;
    public Button pauseBtn;

    public List<TextMeshProUGUI> touchName = new List<TextMeshProUGUI>();

    private void Awake()
    {

        //Debug.Log("안드로이드");
        //fontList = buttonParent.GetComponentsInChildren<TextMeshProUGUI>();
        //foreach (var font in fontList)
        //{
        //    font.color = fontColor;
        //}

        //pauseBtn.onClick.AddListener(OnClickPause);
        //ResisterLang();

        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("안드로이드");
            fontList = buttonParent.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var font in fontList)
            {
                font.color = fontColor;
            }

            pauseBtn.onClick.AddListener(OnClickPause);
            ResisterLang();
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    public PauseUI pauseui;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            PlayerHandler.instance.PlayerDeathEvent += DeactiveTUI;
            //pauseui.ResisterPauseDeactive(ActiveTUI);
            ResisterInteraction();
            if (LanguageManager.instance != null)
                ChangeLanguage();
        }
    }

    public void OnClickPause()
    {
        pauseui.PauseUiActive();
        DeactiveTUI();
    }

    public void DeactiveTUI()
    {
        pauseBtn.gameObject.SetActive(false);
        buttonParent.SetActive(false);
        joystick.SetActive(false);
    }

    public void ActiveTUI()
    {
        pauseBtn.gameObject.SetActive(true);
        buttonParent.SetActive(true);
        joystick.SetActive(true);
        ChangeLanguage();
    }

    public void TouchInteraction()
    {
        if (GameManager.instance != null)
        {
            if (!GameManager.instance.pauseActive)
                ActiveTUI();
            else
                DeactiveTUI();
        }
    }

    public void ResisterInteraction()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ResisterTouchInteraction(TouchInteraction);
        }
    }

    public void ResisterLang()
    {
        if(LanguageManager.instance != null)
        LanguageManager.instance.LanguageEventResister(ChangeLanguage);
    }

    public void ChangeLanguage()
    {
        if (LanguageManager.instance.isKor)
        {
            for (int i = 0; i < touchName.Count; i++)
            {
                touchName[i].text = LanguageManager.instance.touchKor[i];
            }
        }
        else
        {
            for (int i = 0; i < touchName.Count; i++)
            {
                touchName[i].text = LanguageManager.instance.touchEng[i];
            }

        }
    }
}

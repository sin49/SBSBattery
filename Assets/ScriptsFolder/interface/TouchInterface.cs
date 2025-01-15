using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TouchInterface : MonoBehaviour
{
    public GameObject buttonParent;
    public GameObject joystick;

    TextMeshProUGUI[] fontList;
    public Color fontColor;
    public Button pauseBtn;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("안드로이드");
            fontList = buttonParent.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var font in fontList)
            {
                font.color = fontColor;
            }

            pauseBtn.onClick.AddListener(OnClickPause);
        }
        else
            Debug.Log("유니티");
    }

    public PauseUI pauseui;

    private void Start()
    {
        if(Application.platform == RuntimePlatform.Android)
            PlayerHandler.instance.PlayerDeathEvent += DeactiveTUI;
        pauseui.ResisterPauseDeactive(ActiveTUI);
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
    }
}

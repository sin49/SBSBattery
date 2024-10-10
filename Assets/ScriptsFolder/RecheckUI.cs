using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecheckUI : MonoBehaviour
{
    public TextMeshProUGUI Description;

    public Transform SelectedUI;
    public Transform YesButton;
    public Transform NoButton;

    public bool ok;

    public event Action OKEvent;
    public event Action CancelEvent;

    public Sprite activeButton, deactiveButton;
    public List<GameObject> buttonList = new List<GameObject>();
    int index, beforeIndex;

    public Animator recheckAnimator;
    private void Awake()
    {
        //if (this.gameObject.activeSelf)
        //    this.gameObject.SetActive(false);
    }
    public void ActiveUI(string Desc, Action OKEvent, Action CancelEvent)
    {
        Description.text = Desc;
        this.OKEvent += OKEvent;
        this.CancelEvent += CancelEvent;
        gameObject.SetActive(true);
        Debug.Log("UI 활성화­");
    }
    void initializeUI()
    {
        InitButton();
        StartCoroutine(SettingChangeReCheck());
        //UpdateUI();
    }
    private void OnEnable()
    {
        initializeUI();
    }

    IEnumerator SettingChangeReCheck()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        if (recheckAnimator.GetCurrentAnimatorStateInfo(0).IsName("SettingUI"))
        {
            while (recheckAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            ok = true;            
        }
    }

    public void InitButton()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponent<Image>().sprite = deactiveButton;
        }
        index = 0;
        buttonList[index].GetComponent<Image>().sprite = activeButton;
    }

    void UpdateUI()
    {
        SelectedUI.gameObject.SetActive(true);


        /*if (ok)
            SelectedUI.transform.position = YesButton.transform.position;
        else
            SelectedUI.transform.position = NoButton.transform.position;*/
        if (ok)
        {
            SelectedUI.position = YesButton.position;
            buttonList[index].GetComponent<Image>().sprite = activeButton;
            buttonList[beforeIndex].GetComponent<Image>().sprite = deactiveButton;
        }
        else
        {
            SelectedUI.position = NoButton.position;
            buttonList[index].GetComponent<Image>().sprite = activeButton;
            buttonList[beforeIndex].GetComponent<Image>().sprite = deactiveButton;
        }

    }
    void OkButtonInput()
    {
        OKEvent?.Invoke();
        DeActiveUI();
    }
    void CancelButtonInput()
    {
        CancelEvent?.Invoke();
        DeActiveUI();
    }
    void handleUI()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!ok)
                ok = true;
            if (index > 0)
            {
                beforeIndex = index;
                index--;
                UpdateUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (ok)
                ok = false;
            if (index < buttonList.Count - 1)
            {
                beforeIndex = index;
                index++;
                UpdateUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (ok)
                OkButtonInput();
            else
                CancelButtonInput();
        }

    }
    private void OnDisable()
    {
        CancelButtonInput();
    }
    void DeActiveUI()
    {

        CancelEvent = null;
        OKEvent = null;
        this.gameObject.SetActive(false);
        Debug.Log("UI 비활성화­");
    }
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
            handleUI();
    }
}
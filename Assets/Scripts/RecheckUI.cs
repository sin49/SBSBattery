using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecheckUI : MonoBehaviour
{
    public TextMeshProUGUI Description;

    public Transform SelectedUI;
    public Transform YesButton;
    public Transform NoButton;

    public bool ok;

    public event Action OKEvent;
    public event Action CancelEvent;
    private void Awake()
    {
        //if (this.gameObject.activeSelf)
        //    this.gameObject.SetActive(false);
    }
    public void ActiveUI(string Desc,Action OKEvent,Action CancelEvent)
    {
        Description.text = Desc;
        this.OKEvent += OKEvent;
       this. CancelEvent += CancelEvent;
        gameObject.SetActive(true);
        Debug.Log("UI 활성화");
    }
    void initializeUI()
    {
        ok = true;
        UpdateUI();
    }
    private void OnEnable()
    {
        initializeUI();
    }
    void UpdateUI()
    {
        SelectedUI.gameObject.SetActive(true);

        if(ok)
            SelectedUI.transform.position=YesButton.transform.position;
        else
            SelectedUI.transform.position=NoButton.transform.position;
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
            UpdateUI();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (ok)
                ok = false;
            UpdateUI();
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
        Debug.Log("UI 비활성화");
    }
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
            handleUI();
    }
}

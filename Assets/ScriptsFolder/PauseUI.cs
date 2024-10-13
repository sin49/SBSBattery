using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public RectTransform pauseUI;
    bool pauseActive;
    public ButtonSoundEffectPlayer ButtonSoundEffectPlayer_;

    public bool pauseInteract;
    //public string TitleSceneName;
    
    //public ItemListUI itemListUI;
    //public SettingUI SettingUI;


    //GameObject SelectedUI;
   
   // public Transform SelectedUIChecker;
   // Image SelectedUIImage;

   // public Transform ItemTitle;
   // public Transform SettingTitle;

   ////bool UISelected;

   //public Color NotSelectedColor;
   // public Color SelectedColor;

    private void Awake()
    {
        pauseUI.gameObject.SetActive(false);
        pauseActive = false;
        pauseInteract = true;
        Time.timeScale = 1f;
        ButtonSoundEffectPlayer_=gameObject.GetComponent<ButtonSoundEffectPlayer>();
        //SelectedUIImage= SelectedUIChecker.GetComponent<Image>();
    }
    //public void ReturnPauseUI()
    //{
    //   UISelected = false;
    //    UpdatePauseUI();
    //}
    //void initializeUI()
    //{
    //    SelectedUI = SettingUI.gameObject;

    //    //UISelected = false;

    //    UpdatePauseUI();
      
    //}
    //void activeUI()
    //{
    
    //        SettingUI.ActiveUI();
     
    //}
    //void UpdatePauseUI()
    //{
    //    if (SelectedUI == SettingUI.gameObject)
    //    {
    //        SelectedUIChecker.position = SettingTitle.transform.position;
    //    }
    //    else
    //    {
    //        SelectedUIChecker.position = ItemTitle.transform.position;
    //    }
    //    //if (UISelected)
    //    //    SelectedUIImage.color = SelectedColor;
    //    //else
    //    //    SelectedUIImage.color = NotSelectedColor;
    //}
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && pauseInteract)
            PauseUiActive();

        if (pauseActive)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow)
                || Input.GetKeyDown(KeyCode.RightArrow))
            {
                ButtonSoundEffectPlayer_.PlaySelectAudio();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                ButtonSoundEffectPlayer_.PlayActiveAudio();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                ButtonSoundEffectPlayer_.PlayDeActiveAudio();
            }
        }
        //if (pauseActive/*e&&!UISelected*/)
        //{
            //if (Input.GetKeyDown(KeyCode.LeftArrow))
            //{
            //    if (SelectedUI != itemListUI.gameObject)
            //    {
            //        SelectedUI = itemListUI.gameObject;
            //        UpdatePauseUI();
            //    }
            //}
            //if (Input.GetKeyDown(KeyCode.RightArrow))
            //{
            //    if (SelectedUI != SettingUI.gameObject)
            //    {
            //        SelectedUI = SettingUI.gameObject;
            //        UpdatePauseUI();
            //    }
            //}
            //if (Input.GetKeyDown(KeyCode.X))
            //{
            //    PauseUiActive();

            //}

            //if (Input.GetKeyDown(KeyCode.B))
            //    ReturnTitle();

            //if (Input.GetKeyDown(KeyCode.C))
            //{
            //    activeUI();
            //}
            //if (Input.GetKeyDown(KeyCode.X))
            //{
            //    selectedItemUI = false;
            //    itemListUI.DeactiveItemListUI();
            //}
            //여기에 구현하면 안됨 UI 쪽에 구현 
        //}
    }

    //public void ReturnTitle()
    //{
    //    if (pauseActive)
    //    {
    //        GameManager.instance.LoadingSceneWithKariEffect("TitleTest");
    //        PauseUiActive();
    //    }
    //}    

    public void PauseUiActive()
    {
        //initializeUI();
        pauseActive = !pauseActive;
        GameManager.instance.pauseActive = pauseActive;
        if (pauseActive)
        {
            Time.timeScale = 0f;
            ButtonSoundEffectPlayer_.PlayDeActiveAudio();
            //activeUI();
        }
        else
        {
            ButtonSoundEffectPlayer_.PlayActiveAudio();
            Time.timeScale = 1f;
        }
        pauseUI.gameObject.SetActive(pauseActive);
    }
}

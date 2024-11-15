using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : UIInteract
{
    public List<TItleText> titletexts;
    public int index;
    public TextMeshProUGUI ResetText;
    public string startscenename;
    public ButtonSoundEffectPlayer ButtionSoundEffectPlayer_;

    [Header("서브프로그래머 추가 작업")]
    public Sprite activeButton;
    public Sprite deactiveButton;

    public bool onHandle;
    public TestSettingUI settingUI;
    public TitleSceneAudio settingAudio;

    public TestRecheckUI recheckUI;

    public int LastIndex;

    [Header("체크포인트 UI")]public CheckPointUI checkPointUI;
    public void StartNewGame()
    {
        Debug.Log(startscenename);
        GameManager.instance.DeleteSaveSetting();
        GameManager.instance.saveCheckPointIndexKey(0);
        GameManager.instance.LoadingSceneWithKariEffect(startscenename);
        GameManager.instance.loadcheckpointTransformType = 0;

    }
    public void ContinueGame()
    {
        //GameManager.instance.LoadLastCheckPoint();
        onHandle = false;
        checkPointUI.gameObject.SetActive(true);
    }
    public void Setting()
    {
        onHandle = false;
        if (!settingUI.gameObject.activeSelf)
        {
            Debug.Log("설정창 활성화시키자");
            settingUI.gameObject.SetActive(true);
            settingAudio.active = true;
        }
        titletexts[index].ImageHub.GetComponent<Image>().sprite = deactiveButton;
        fontList[index].color = deactiveFontColor;
    }

    public void ResetData()
    {
        onHandle = false;
        recheckUI.gameObject.SetActive(true);
        settingAudio.active = true;
        titletexts[index].ImageHub.GetComponent<Image>().sprite = deactiveButton;
        fontList[index].color = deactiveFontColor;

        //GameManager.instance.DeleteSaveSetting();
        //ResetText.gameObject.SetActive(true);
    }

    public void DeleteData()
    {
        onHandle = true;
        settingAudio.active = false;

        GameManager.instance.DeleteSaveSetting();
        ResetText.gameObject.SetActive(true);
    }

    public void removeEvents()
    {
        //foreach (TItleText t in titletexts)
        //{
        //    t.removeevent();
        //}
    }

    public void SettingBackScreen()
    {
        onHandle = true;
        settingAudio.active = false;

        titletexts[index].ImageHub.GetComponent<Image>().sprite = activeButton;
        fontList[index].color = activeFontColor;
    }

    public void handletitle()
    {
        //int LastIndex;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            LastIndex = index;
            index++;
            ButtionSoundEffectPlayer_.PlaySelectAudio();
            if (!PlayerPrefs.HasKey("CheckPointIndex") && index == 1)
            {
                Debug.Log("저장된 데이터가 없음");
                index++;
            }
            if (index >= titletexts.Count)
                index = titletexts.Count - 1;
            changehub(LastIndex, index);

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LastIndex = index;
            index--;
            ButtionSoundEffectPlayer_.PlaySelectAudio();
            if (!PlayerPrefs.HasKey("CheckPointIndex") && index == 1)
            {
                Debug.Log("저장된 데이터가 없음");
                index--;
            }
            if (index < 0)
                index = 0;
            changehub(LastIndex, index);

        }
        else if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            //ButtionSoundEffectPlayer_.PlayActiveAudio();
            //titletexts[index].ButtonActive();
            SelectButton();
        }

    }

    public void SelectButton()
    {
        if (!PlayerPrefs.HasKey("CheckPointIndex") && index == 1) return;
        ButtionSoundEffectPlayer_.PlayActiveAudio();
        titletexts[index].ButtonActive();
    }
    public void SetIndex(int n)
    {
        ButtionSoundEffectPlayer_.PlaySelectAudio();
        LastIndex = index;
        index = n;
        changehub(LastIndex, index);
    }

    public void changehub(int before, int after)
    {
        /*titletexts[before].DeActiveImageHub();
        titletexts[after].ActiveImageHub();*/

        titletexts[before].ImageHub.GetComponent<Image>().sprite = deactiveButton;
        fontList[before].color = deactiveFontColor;
        titletexts[after].ImageHub.GetComponent<Image>().sprite = activeButton;
        fontList[after].color = activeFontColor;
    }
    public void quitgame()
    {
        removeEvents();
        Application.Quit();
    }
    public void InitText()
    {
        if (PlayerPrefs.HasKey("CheckPointIndex"))
            index = 1;
        else
            index = 0;
        titletexts[index].ActiveImageHub();
        titletexts[0].ButtonEffect += StartNewGame;
        titletexts[1].ButtonEffect += ContinueGame;
        titletexts[2].ButtonEffect += Setting;
        titletexts[3].ButtonEffect += ResetData;
        titletexts[titletexts.Count - 1].ButtonEffect += quitgame;
        ResetText.gameObject.SetActive(false);
    }

    private void Awake()
    {
        ButtionSoundEffectPlayer_ = gameObject.GetComponent<ButtonSoundEffectPlayer>();
        onHandle = true;
    }
    private void Start()
    {
        InitText();


    }
    // Update is called once per frame
    void Update()
    {
        if (!onHandle)
            return;
        handletitle();
    }
}

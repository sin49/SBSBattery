using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class EpilogueVideoPlayer : MonoBehaviour
{

    public VideoPlayer videoPlayer;  // VideoPlayer ÄÄÆ÷³ÍÆ® ÂüÁ¶
    public string sceneName;         // ÀüÈ¯ÇÒ ¾À ÀÌ¸§
    public GameObject skipButton;
    public Image gauge;
    public bool epiloguecomplete;
    public BackGroundAudioPlayer backgroundaudioplayer;

    [Range(0,3)]public float timer;
    public bool gaugeOver;
    void Start()
    {
        if (!epiloguecomplete)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            skipButton.SetActive(false);
            backgroundaudioplayer.AudioPlay();

            timer = 0;
            gauge.fillAmount = 0;
        }
        DeleteTutorialKey();

        skipTMP.text = SkipLanguage();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        epiloguecomplete = true;
        GameManager.instance.LoadingSceneWithKariEffect(sceneName);

    }

    private void Update()
    {
        if (videoPlayer.time > 2)
        {
            skipButton.SetActive(true);
            if (skipButton.activeSelf)
            {
                if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Space))
                    UpdateSkipGauge();
                else
                    CancelSKipKey();

                gauge.fillAmount = timer / 1f;
                if (gauge.fillAmount >= 1f && !gaugeOver)
                {
                    gaugeOver = true;
                    videoPlayer.Stop();
                    OnVideoEnd(videoPlayer);
                    skipButton.SetActive(false);
                }
            }
        }
        if(epiloguecomplete)
        {
            backgroundaudioplayer.AudioStop();
        }
    }

    public void UpdateSkipGauge()
    {        
        timer += Time.deltaTime;
    }

    public void CancelSKipKey()
    {
        timer = 0;
    }

    public void SkipButtonClick()
    {
        videoPlayer.Stop();
        OnVideoEnd(videoPlayer);
        skipButton.SetActive(false);
    }

    public void DeleteTutorialKey()
    {
        if (PlayerPrefs.HasKey("AttackTuto")) PlayerPrefs.DeleteKey("AttackTuto");
        if (PlayerPrefs.HasKey("JumpTuto")) PlayerPrefs.DeleteKey("JumpTuto");
        if (PlayerPrefs.HasKey("MoveTuto")) PlayerPrefs.DeleteKey("MoveTuto");
        if (PlayerPrefs.HasKey("DownTuto")) PlayerPrefs.DeleteKey("DownTuto");
        if (PlayerPrefs.HasKey("InteractTuto")) PlayerPrefs.DeleteKey("InteractTuto");
        if (PlayerPrefs.HasKey("DownAttackTuto")) PlayerPrefs.DeleteKey("DownAttackTuto");
        if (PlayerPrefs.HasKey("DimensionTuto")) PlayerPrefs.DeleteKey("DimensionTuto");
        if (PlayerPrefs.HasKey("TutorialEnd")) PlayerPrefs.DeleteKey("TutorialEnd");

        GameManager.instance.attackTuto = false; GameManager.instance.jumpTuto = false; GameManager.instance.moveTuto = false;
        GameManager.instance.downTuto = false; GameManager.instance.interactTuto = false; GameManager.instance.downAttackTuto = false;
        GameManager.instance.dimensionTuto = false; GameManager.instance.tutoInteract = false; GameManager.instance.downTuto = false;
        GameManager.instance.tutorialEnd = false;
    }

    [Header("½ºÅµ ¾ð¾î")]
    public TextMeshProUGUI skipTMP;
    //public string kor, eng;
    public string SkipLanguage()
    {
        if (LanguageManager.instance != null)
        {
            if (LanguageManager.instance.isKor)
                return "½ºÅµ: SPACE";
            else
                return "SKIP: SPACE";
        }

        return "";
    }
}

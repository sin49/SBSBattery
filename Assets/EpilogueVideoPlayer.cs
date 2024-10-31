using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class EpilogueVideoPlayer : MonoBehaviour
{

    public VideoPlayer videoPlayer;  // VideoPlayer 컴포넌트 참조
    public string sceneName;         // 전환할 씬 이름
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
}

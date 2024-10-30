using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EpilogueVideoPlayer : MonoBehaviour
{

    public VideoPlayer videoPlayer;  // VideoPlayer 컴포넌트 참조
    public string sceneName;         // 전환할 씬 이름
    public GameObject skipButton;
    public bool epiloguecomplete;
    public BackGroundAudioPlayer backgroundaudioplayer;
    void Start()
    {
        if (!epiloguecomplete)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            skipButton.SetActive(false);
            backgroundaudioplayer.AudioPlay();
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
            if (skipButton.activeSelf && Input.GetKeyDown(KeyCode.C))
            {
                videoPlayer.Stop();
                OnVideoEnd(videoPlayer);
            }
        }
        if(epiloguecomplete)
        {
            backgroundaudioplayer.AudioStop();
        }
    }
}

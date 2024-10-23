using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EpilogueVideoPlayer : MonoBehaviour
{

    public VideoPlayer videoPlayer;  // VideoPlayer ������Ʈ ����
    public string sceneName;         // ��ȯ�� �� �̸�
    public GameObject skipButton;

    void Start()
    {

        videoPlayer.loopPointReached += OnVideoEnd;
        skipButton.SetActive(false);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        GameManager.instance.LoadingSceneWithKariEffect(sceneName);

    }

    private void Update()
    {
        //if(videoPlayer.time)
    }
}

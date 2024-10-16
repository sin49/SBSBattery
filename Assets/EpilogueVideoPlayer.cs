using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EpilogueVideoPlayer : MonoBehaviour
{
  
        public VideoPlayer videoPlayer;  // VideoPlayer ������Ʈ ����
        public string sceneName;         // ��ȯ�� �� �̸�

        void Start()
        {
           
            videoPlayer.loopPointReached += OnVideoEnd;
        }

        void OnVideoEnd(VideoPlayer vp)
        {
        GameManager.instance.LoadingSceneWithKariEffect(sceneName);

        }
    
}

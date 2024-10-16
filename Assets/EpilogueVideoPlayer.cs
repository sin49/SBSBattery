using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EpilogueVideoPlayer : MonoBehaviour
{
  
        public VideoPlayer videoPlayer;  // VideoPlayer 컴포넌트 참조
        public string sceneName;         // 전환할 씬 이름

        void Start()
        {
           
            videoPlayer.loopPointReached += OnVideoEnd;
        }

        void OnVideoEnd(VideoPlayer vp)
        {
        GameManager.instance.LoadingSceneWithKariEffect(sceneName);

        }
    
}

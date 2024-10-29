using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LoadingEffectKari : MonoBehaviour
{
    public float EffectTime = 0.5f;
    public float IntesityTime = 0.5f;
    public Camera gameovercamera;
    public Camera gameovercamera2;
    public Volume volume;
    UnityEngine.Rendering.Universal.Vignette vignette;
    UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;
    float Effectspeed;
    float intensityspeed;
    public bool FadeOff;
    public bool LoadingComplete;
    public bool gameover;
    public string LoadSceneName;
    float alpha;
    public event Action<string> EffectEnd;
    Image image_;
    public GameObject loadingImage;

    private void Awake()
    {
        image_ = GetComponent<Image>();
        if (volume.profile.TryGet(out vignette))
        {
            // Vignette 초기 설정
            vignette.intensity.value = 0f;
            vignette.smoothness.overrideState = false;
            vignette.rounded.value = true;
        }
        if (volume.profile.TryGet(out colorAdjustments))
        {
        colorAdjustments.saturation.overrideState = false;
            colorAdjustments.saturation.value = 0;
        }
    }

  public  void GAmeOverPostProcessing()
    {
        StartCoroutine(gameovercorutine());
    }
    IEnumerator gameovercorutine()
    {
        PlayerHandler.instance.isDie = true;
        vignette.center.value = PlayerHandler.instance.CurrentCamera.WorldToViewportPoint(PlayerHandler.instance.CurrentPlayer.transform.position);
        gameovercamera.transform.position = PlayerHandler.instance.CurrentCamera.transform.position;
        gameovercamera2.transform.position = gameovercamera.transform.position;
        gameovercamera.transform.rotation = PlayerHandler.instance.CurrentCamera.transform.rotation;
        gameovercamera2.transform.position = gameovercamera.transform.position;
        gameovercamera2.farClipPlane=PlayerHandler.instance.CurrentCamera.farClipPlane;
        GameObject.Find("BackGroundAudioPlayer").GetComponent<BackGroundAudioPlayer>().AudioStop();
        if (PlayerHandler.instance.CurrentCamera.orthographic) {
            gameovercamera.orthographic = true;
            gameovercamera2.orthographic = true;
        }
     
        PlayerHandler.instance.CurrentPlayer.DieANimationPlay();
        yield return null;
        Time.timeScale = 0;
     
        colorAdjustments.saturation.overrideState = true;
        colorAdjustments.saturation.value = -100;
        PlayerHandler.instance.CurrentCamera.cullingMask &= ~(1 << 16);
        PlayerHandler.instance.CurrentCamera.gameObject.SetActive(false);
        gameovercamera.gameObject.SetActive(true);
        gameovercamera2.gameObject.SetActive(true);
   
        while (intensity < 1)
        {
            vignette.intensity.value = intensity;
            intensity += intensityspeed * Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        yield return new WaitForSecondsRealtime(1.5f);
        vignette.intensity.value = 1;
        alpha = 0;
        while (alpha < 1)
        {
            alpha += Effectspeed * Time.unscaledDeltaTime;
            image_.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        image_.color= new Color(0, 0, 0, 1);
        vignette.intensity.value = 0;
        colorAdjustments.saturation.overrideState = false;
        //게임 오버 UI 활성화

    }
    void loadingVigintteoff()
    {
        if (LoadingComplete)
        {
            if (loadingImage != null)
                loadingImage.SetActive(false);
            if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentPlayer)
            {
                vignette.center.value = PlayerHandler.instance.CurrentCamera.WorldToViewportPoint(PlayerHandler.instance.CurrentPlayer.transform.position);
            }
            else
            {
                vignette.center.value = new Vector2(0.5f, 0.5f);
            }
            if (alpha > 0)
            {
                alpha -= Effectspeed * Time.unscaledDeltaTime;

                image_.color = new Color(0, 0, 0, alpha);
            }
            else
            if (alpha <= 0)
            {
                intensity -= intensityspeed * Time.unscaledDeltaTime;


                vignette.intensity.value = intensity;
            }
            if (intensity <= 0)
            {
                FadeOff = false;
                LoadingComplete = false;
                this.gameObject.SetActive(false);
            }
        }
    }
    void LoadingVigintteOn()
    {
        if (!FadeOff)
        {

            if (intensity < 1)
                intensity += intensityspeed * Time.unscaledDeltaTime;

            if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentPlayer)
            {
                vignette.center.value = PlayerHandler.instance.CurrentCamera.WorldToViewportPoint(PlayerHandler.instance.CurrentPlayer.transform.position);
            }
            else
            {
                vignette.center.value = new Vector2(0.5f, 0.5f);
            }
            vignette.intensity.value = intensity;
            if (intensity >= 1)
            {
                alpha += Effectspeed * Time.unscaledDeltaTime;
                image_.color = new Color(0, 0, 0, alpha);
            }
            if (alpha >= 1)
            {
                if (loadingImage != null)
                    loadingImage.SetActive(true);
                FadeOff = true;
                EffectEnd?.Invoke(LoadSceneName);
                //this.gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        EffectEnd = null;
    }
    float intensity;
    // Update is called once per frame
    void Update()
    {
        Effectspeed = 1 / EffectTime;
        intensityspeed = 1 / IntesityTime;
       
        if (!gameover)
        {
            LoadingVigintteOn();
            loadingVigintteoff();
        }
    }
}

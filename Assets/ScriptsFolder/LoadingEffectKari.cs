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
    public Volume volume;
    UnityEngine.Rendering.Universal.Vignette vignette;
    float Effectspeed;
    float intensityspeed;
    public bool FadeOff;
    public bool LoadingComplete;

    public string LoadSceneName;
    float alpha;
    public event Action<string> EffectEnd;
    Image image_;
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
                FadeOff = true;
                EffectEnd?.Invoke(LoadSceneName);
                //this.gameObject.SetActive(false);
            }
        }
        else if(LoadingComplete)
        {
            if (alpha > 0)
            {
                alpha -= Effectspeed * Time.unscaledDeltaTime;

                image_.color = new Color(0, 0, 0, alpha);
            }
            else
            if (alpha <= 0)
            {
                intensity -= intensityspeed * Time.unscaledDeltaTime;

                if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentPlayer)
                {
                    vignette.center.value = PlayerHandler.instance.CurrentCamera.WorldToViewportPoint(PlayerHandler.instance.CurrentPlayer.transform.position);
                }
                else
                {
                    vignette.center.value = new Vector2(0.5f, 0.5f);
                }
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
}

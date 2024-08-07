using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingEffectKari : MonoBehaviour
{
    public float EffectTime = 0.5f;
    float Effectspeed;
    public bool FadeOff;
    public bool LoadingComplete;
    public string LoadSceneName;
    float alpha;
    public event Action<string> EffectEnd;
    Image image_;
    private void Awake()
    {
        image_ = GetComponent<Image>();
    }
    private void OnDisable()
    {
        EffectEnd = null;
    }
    // Update is called once per frame
    void Update()
    {
        Effectspeed = 1 / EffectTime;

        if (!FadeOff)
        {
            alpha += Effectspeed * Time.unscaledDeltaTime;
            image_.color = new Color(0, 0, 0, alpha);
            if (alpha >= 1)
            {
                FadeOff = true;
                EffectEnd?.Invoke(LoadSceneName);
                //this.gameObject.SetActive(false);
            }
        }
        else if(LoadingComplete)
        {
            
            alpha -= Effectspeed * Time.unscaledDeltaTime;
            image_.color = new Color(0, 0, 0, alpha);
            if (alpha <= 0)
            {
                FadeOff = false;
                LoadingComplete = false;
                this.gameObject.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
[ExecuteAlways]
public class SMHChanger : volumeParameterChanger,colliderDisplayer
{
    public GameObject ColliderDisplay;

    public void registerColliderDIsplay()
    {
        if (ColliderDisplayManager.Instance != null)
        {
            ColliderDisplayManager.Instance.register(this);
        }
    }
    [Header("로컬/월드 전환(On=월드,off=로컬)")]
    public bool loadworldProcessing;


    bool active;
    private void Start()
    {
        registerColliderDIsplay();
    }


    protected override void Awake()
    {
        base.Awake();
  
    }
    //public void SavePreset()
    //{
    //    ShadowsMidtonesHighlights smh;
    //    if (volume.profile.TryGet<ShadowsMidtonesHighlights>(out smh))
    //    {
    //        //smh = PresetSetting;
    //        SetParameter(PresetSetting.highlights, smh.highlights);
    //        SetParameter(PresetSetting.shadows, smh.shadows);
    //        SetParameter(PresetSetting.midtones, smh.midtones);
    //        SetParameter(PresetSetting.shadowsStart, smh.shadowsStart);
    //        SetParameter(PresetSetting.shadowsEnd, smh.shadowsEnd);
    //        SetParameter(PresetSetting.highlightsStart, smh.highlightsStart);
    //        SetParameter(PresetSetting.highlightsEnd, smh.highlightsEnd);

    //        Debug.Log("프리셋 로딩 완료");
    //    }
    //}

    public override void LoadPreset()
    {
        //if (PresetSetting == null)
        //    return;

        //ShadowsMidtonesHighlights smh;
        //if (volume.profile.TryGet<ShadowsMidtonesHighlights>(out smh))
        //{
        //    //smh = PresetSetting;
        //    SetParameter(smh.highlights, PresetSetting.highlights);
        //    SetParameter(smh.shadows, PresetSetting.shadows);
        //    SetParameter(smh.midtones, PresetSetting.midtones);
        //    SetParameter(smh.shadowsStart, PresetSetting.shadowsStart);
        //    SetParameter(smh.shadowsEnd, PresetSetting.shadowsEnd);
        //    SetParameter(smh.highlightsStart, PresetSetting.highlightsStart);
        //    SetParameter(smh.highlightsEnd, PresetSetting.highlightsEnd);

        //}

    }

    private void OnTriggerStay(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            if(!loadworldProcessing)
            {
                LoadPreset();
                volume.enabled = true;
                if (GlobalPostProcessingManager.instance != null)
                    GlobalPostProcessingManager.instance.DisableGlobalPreset(this);
            }
            else
            {
                if (GlobalPostProcessingManager.instance != null)
                    GlobalPostProcessingManager.instance.EnableGlobalPreset();
            }
     
        }
    }

    public void ActiveColliderDisplay()
    {
        ColliderDisplay.SetActive(true);
    }

    public void DeactiveColliderDisplay()
    {
        ColliderDisplay.SetActive(false);
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    volume.enabled = false;
    //    if (GlobalPostProcessingManager.instance != null)
    //        GlobalPostProcessingManager.instance.EnableGlobalPreset();
    //}
}

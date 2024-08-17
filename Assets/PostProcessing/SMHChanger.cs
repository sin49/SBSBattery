using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class SMHChanger : volumeParameterChanger
{


    [Header("������")]
    public ShadowsMidtonesHighlights PresetSetting;
    [Header("����/���� ��ȯ(On=����,off=����)")]
    public bool loadworldProcessing;
    public void SavePreset()
    {
        ShadowsMidtonesHighlights smh;
        if (volume.profile.TryGet<ShadowsMidtonesHighlights>(out smh))
        {
            //smh = PresetSetting;
            SetParameter(PresetSetting.highlights, smh.highlights);
            SetParameter(PresetSetting.shadows, smh.shadows);
            SetParameter(PresetSetting.midtones, smh.midtones);
            SetParameter(PresetSetting.shadowsStart, smh.shadowsStart);
            SetParameter(PresetSetting.shadowsEnd, smh.shadowsEnd);
            SetParameter(PresetSetting.highlightsStart, smh.highlightsStart);
            SetParameter(PresetSetting.highlightsEnd, smh.highlightsEnd);

            Debug.Log("������ �ε� �Ϸ�");
        }
    }
   
    public override void LoadPreset()
    {

    
            ShadowsMidtonesHighlights smh;
            if (volume.profile.TryGet<ShadowsMidtonesHighlights>(out smh))
            {
            //smh = PresetSetting;
            SetParameter(smh.highlights, PresetSetting.highlights);
            SetParameter(smh.shadows, PresetSetting.shadows);
            SetParameter(smh.midtones, PresetSetting.midtones);
            SetParameter(smh.shadowsStart, PresetSetting.shadowsStart);
            SetParameter(smh.shadowsEnd, PresetSetting.shadowsEnd);
            SetParameter(smh.highlightsStart, PresetSetting.highlightsStart);
            SetParameter(smh.highlightsEnd, PresetSetting.highlightsEnd);
            
            Debug.Log("������ �ε� �Ϸ�");
            }
        
    }
   
    private void OnTriggerEnter(Collider other)
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
    //private void OnTriggerExit(Collider other)
    //{
    //    volume.enabled = false;
    //    if (GlobalPostProcessingManager.instance != null)
    //        GlobalPostProcessingManager.instance.EnableGlobalPreset();
    //}
}

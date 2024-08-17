using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class SMHChanger : volumeParameterChanger
{


    [Header("프리셋")]
    public ShadowsMidtonesHighlights PresetSetting;
   
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
            
            Debug.Log("프리셋 로딩 완료");
            }
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadPreset();
            volume.enabled = true;
            GlobalPostProcessingManager.instance.DisableGlobalPreset();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        volume.enabled = false;
        GlobalPostProcessingManager.instance.EnableGlobalPreset();
    }
}

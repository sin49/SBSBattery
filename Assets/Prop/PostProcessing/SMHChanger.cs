using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
[ExecuteAlways]
public class SMHChanger : volumeParameterChanger,colliderDisplayer
{
    public Renderer ColliderDisplay;

    public void registerColliderDIsplay()
    {
        if (ColliderDisplayManager.Instance != null)
        {
            ColliderDisplayManager.Instance.register(this);
        }
    }
    [Header("포스트 프로세싱 끄는 기능(이거 키면 프로세싱 교체가 아니라 그냥 끄는거)")]
    public bool postprocessingoff;

    VolumeProfile profile;

    private void Start()
    {
        registerColliderDIsplay();
     
    }


    protected override void Awake()
    {
        base.Awake();

        profile = volume.profile;
        volume.enabled = false;
    }
  

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
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            if(!postprocessingoff)
            GlobalPostProcessingManager.instance.volume.profile = profile;
            else
                GlobalPostProcessingManager.instance.volume.profile = null;

        }
    }
   
    public void ActiveColliderDisplay()
    {
        ColliderDisplay.enabled = true;
    }

    public void DeactiveColliderDisplay()
    {
        ColliderDisplay.enabled = false;
    }
  
}

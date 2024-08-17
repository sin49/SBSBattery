using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class GlobalPostProcessingManager : volumeParameterChanger
{

    [Header("������ SHM ������ �̸�")]
    public string CurrentSettingName;


    public static GlobalPostProcessingManager instance;
    protected override void Awake()
    {
        base.Awake();

        instance = this;

    }
    private void Start()
    {
        volume.enabled = true;
    }

    public void DisableGlobalPreset()
    {
        volume.enabled = false;
    }
    public void EnableGlobalPreset()
    {
        volume.enabled = true;
    }


    public void LoadPreset(ShadowsMidtonesHighlights GlobalPresetSetting)
    {

        ShadowsMidtonesHighlights smh;
        if (volume.profile.TryGet<ShadowsMidtonesHighlights>(out smh))
        {
            smh.highlights = GlobalPresetSetting.highlights;
            smh.shadows = GlobalPresetSetting.shadows;
            smh.midtones = GlobalPresetSetting.midtones;
            smh.shadowsStart = GlobalPresetSetting.shadowsStart;
            smh.shadowsEnd = GlobalPresetSetting.shadowsEnd;
            smh.highlightsStart = GlobalPresetSetting.highlightsStart;
            smh.highlightsEnd = GlobalPresetSetting.highlightsEnd;
            Debug.Log("������ �ε� �Ϸ�");
        }

    }

    public override void LoadPreset()
    {
        throw new System.NotImplementedException();
    }
}

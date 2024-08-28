using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public abstract class volumeParameterChanger : MonoBehaviour
{

    [Header("포스트 프로세싱")]
    public Volume volume;



    protected virtual void Awake()
    {
        volume = GetComponent<Volume>();
        volume.enabled = false;
    }

    public abstract void LoadPreset();
    

    protected void SetParameter(VolumeParameter parameter, VolumeParameter presetParameter)
    {
        if (parameter != null && presetParameter != null)
        {
            parameter.overrideState = presetParameter.overrideState;
            parameter.SetValue(presetParameter);
        }
    }
}

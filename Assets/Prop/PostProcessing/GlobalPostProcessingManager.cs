using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class GlobalPostProcessingManager : volumeParameterChanger
{




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

   


   

    public override void LoadPreset()
    {
        throw new System.NotImplementedException();
    }
}

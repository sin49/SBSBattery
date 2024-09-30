using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PlayerfrontMaterialalSwitcher : MonoBehaviour
{
    public Material materialA;  
    public Material materialB;  

    public UniversalRendererData forwardRendererData;  

    void Start()
    {

        SetMaterial(materialA);
    }

    void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetMaterial(materialA);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetMaterial(materialB);
        }
    }

    void SetMaterial(Material newMaterial)
    {
        if (forwardRendererData == null) return;

        foreach (var feature in forwardRendererData.rendererFeatures)
        {
            if (feature is RenderObjects renderObjectsFeature && renderObjectsFeature.name == "Playerfront")
            {

                renderObjectsFeature.settings.overrideMaterial = newMaterial;
                forwardRendererData.SetDirty();
            }
        }
    }
}
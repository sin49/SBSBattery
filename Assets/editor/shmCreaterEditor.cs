using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
[CustomEditor(typeof(GlobalPostProcessingManager))]
public class shmCreaterEditor : Editor
{
    GlobalPostProcessingManager shmmanager;

    private void Awake()
    {
        shmmanager = (GlobalPostProcessingManager)target;
    }
    [MenuItem("Assets/Create/SMH Preset")]
    public static void CreatePreset()
    {
        ShadowsMidtonesHighlights preset = ScriptableObject.CreateInstance<ShadowsMidtonesHighlights>();
        AssetDatabase.CreateAsset(preset, "Assets/PostProcessing/SHMPreset/NewSMHPreset.asset");
        AssetDatabase.SaveAssets();
    }
    public static void CreatePreset(ShadowsMidtonesHighlights shm,string name)
    {
        ShadowsMidtonesHighlights preset = ScriptableObject.CreateInstance<ShadowsMidtonesHighlights>();
        preset = shm;
        AssetDatabase.CreateAsset(preset, $"Assets/PostProcessing/SHMPreset/{name}.asset");
        AssetDatabase.SaveAssets();
    }
    public override void OnInspectorGUI()
    {
 
        if (GUILayout.Button("현재 SHM 설정을 저장한다"))
        {
            if (shmmanager.volume != null)
            {
                ShadowsMidtonesHighlights smh;
                if (shmmanager.volume.profile.TryGet<ShadowsMidtonesHighlights>(out smh))
                {
                
                }
                
            }
        }
        base.OnInspectorGUI();
        
    }
}

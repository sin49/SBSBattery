using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CineMachineBasicCamera))]
public class CineMachineCAmeraManager : Editor
{
    CineMachineBasicCamera m_camera;

    private void Awake()
    {
        m_camera=target as CineMachineBasicCamera;
        m_camera.virtualcamera= m_camera.GetComponent<CinemachineVirtualCamera>();
    }
    public void Createcampreset()
    {
        CameraSetting cam = ScriptableObject.CreateInstance<CameraSetting>();
        m_camera.setting = cam;
        m_camera.SaveCamSetting();
        AssetDatabase.CreateAsset(cam, $"Assets/CineMachine/preset/{m_camera.SettingName}.asset");
        AssetDatabase.SaveAssets();
    }
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("프리셋 생성"))
            Createcampreset();
        if (GUILayout.Button("프리셋 저장"))
            m_camera.SaveCamSetting();
        if (GUILayout.Button("프리셋 불려오기"))
            m_camera.ApplySettings();
        base.OnInspectorGUI();

    }
}

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

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

    }
}

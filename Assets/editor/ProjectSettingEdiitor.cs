using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ProjectSetting))]
public class ProjectSettingEdiitor : Editor
{
    ProjectSetting setting;

    SerializedProperty movespeed;
    SerializedProperty jumpforce;
    SerializedProperty trackingtime;
    private void OnEnable()
    {
        setting = (ProjectSetting)target;

        movespeed = serializedObject.FindProperty("movespeed");
        jumpforce = serializedObject.FindProperty("jumpforce");
        trackingtime = serializedObject.FindProperty("CameraTrackingTime");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("최신 값 불려오기"))
        {
            if (PlayerPrefs.HasKey("jumpforce"))
            {
                jumpforce.floatValue = PlayerPrefs.GetFloat("jumpforce");
            }
            if (PlayerPrefs.HasKey("movespeed"))
            {
                movespeed.floatValue = PlayerPrefs.GetFloat("movespeed");
            }
            if (PlayerPrefs.HasKey("CameraTrackingTime"))
            {
                trackingtime.floatValue = PlayerPrefs.GetFloat("CameraTrackingTime");
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}

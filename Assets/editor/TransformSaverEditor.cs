using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(TransformSaver))]
public class TransformSaverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TransformSaver transformSaver = (TransformSaver)target;

        if (GUILayout.Button("저장"))
        {
            transformSaver.SaveTransform();
        }

        if (GUILayout.Button("불려오기(플레이모드 종료하고 누르기)"))
        {
            transformSaver.LoadTransform();
        }
    }
}

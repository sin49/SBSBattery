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

        if (GUILayout.Button("����"))
        {
            transformSaver.SaveTransform();
        }

        if (GUILayout.Button("�ҷ�����(�÷��̸�� �����ϰ� ������)"))
        {
            transformSaver.LoadTransform();
        }
    }
}

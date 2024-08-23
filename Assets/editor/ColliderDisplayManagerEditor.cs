using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ColliderDisplayManager))]
public class ColliderDisplayManagerEditor : Editor
{
    ColliderDisplayManager manager;
    private void Awake()
    {
        manager = (ColliderDisplayManager)target;
    }
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("�浹���� ���÷��� Ȱ��ȭ"))
        {
            manager.ActiveCollderDisplay();
        }
        if (GUILayout.Button("�浹���� ���÷��� ��Ȱ��ȭ"))
        {
            manager.DeactiveColliderDisplay();
        }
        base.OnInspectorGUI();
    }
}

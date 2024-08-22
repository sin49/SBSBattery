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
        if (GUILayout.Button("충돌범위 디스플레이 활성화"))
        {
            manager.ActiveCollderDisplay();
        }
        if (GUILayout.Button("충돌범위 디스플레이 비활성화"))
        {
            manager.DeactiveColliderDisplay();
        }
        base.OnInspectorGUI();
    }
}

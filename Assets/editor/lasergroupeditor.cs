using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
[CustomEditor(typeof(bosslasergroup)),CanEditMultipleObjects]
public class lasergroupeditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("������ �׷� ������Ʈ"))
        {
            bosslasergroup t = (bosslasergroup)target;
            t.updatelaser();
        }
        base.OnInspectorGUI();
    }
}

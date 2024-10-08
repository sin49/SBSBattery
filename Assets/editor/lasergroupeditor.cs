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
        if(GUILayout.Button("레이저 그룹 업데이트"))
        {
            bosslasergroup t = (bosslasergroup)target;
            t.updatelaser();
        }
        base.OnInspectorGUI();
    }
}

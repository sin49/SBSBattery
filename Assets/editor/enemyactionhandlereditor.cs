using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EnemyAttackHandler))]
public class enemyactionhandlereditor : Editor
{
    EnemyAttackHandler instance;
    public override void OnInspectorGUI()
    {
        instance=target as EnemyAttackHandler;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("type"),
            new GUIContent("적 행동 타입"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("createtransform"),
          new GUIContent("생성 위치(비우면 본인)"));
        if (GUILayout.Button("생성"))
        {
            if (instance. mainaction!= null)
            {
                if(Application.isPlaying)
                Destroy(instance.mainaction);
                else
                    DestroyImmediate(instance.mainaction);
                instance.mainaction = null;
            }
            instance.createaction();
        }
     serializedObject.ApplyModifiedProperties();

    }
}

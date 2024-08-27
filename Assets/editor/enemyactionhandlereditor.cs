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
            new GUIContent("�� �ൿ Ÿ��"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("createtransform"),
          new GUIContent("���� ��ġ(���� ����)"));
        if (GUILayout.Button("����"))
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

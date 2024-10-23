using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
 EnemySpawner m_EnemySpawner;

    // CSV에서 불러온 데이터를 저장할 객체

    private void OnEnable()
    {
       m_EnemySpawner = (EnemySpawner)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("저장"))
        {
            m_EnemySpawner. SaveEnemyData();
        }
        if (GUILayout.Button("생성"))
        {
            m_EnemySpawner.CreateEnemy();
        }
        GUILayout.EndHorizontal();
    }
}
   

   
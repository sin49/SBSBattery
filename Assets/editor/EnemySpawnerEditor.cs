using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    SerializedProperty id;
    SerializedProperty enemyModelNumber;
    SerializedProperty enemyStatusNumber;
    SerializedProperty attackColliderNumber;
    SerializedProperty enemyAttackNumber;
    SerializedProperty enemyMoveNumber;
    SerializedProperty enemyPatrolNumber;

    // CSV에서 불러온 데이터를 저장할 객체

    private void OnEnable()
    {
       
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //serializedObject.Update();

    

        //serializedObject.ApplyModifiedProperties();
    }

   

   
}
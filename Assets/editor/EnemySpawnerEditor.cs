using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
 EnemySpawner m_EnemySpawner;

    SerializedProperty e_Stat;

    string[] EnemyModelNames;
    string[] ENemyAttackNames;
    string[] EnemyMoveNames;
    

    // CSV에서 불러온 데이터를 저장할 객체

    private void OnEnable()
    {
       m_EnemySpawner = (EnemySpawner)target;
        e_Stat = serializedObject.FindProperty("enemyData")
            ;
    }
   
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty
("CreateEnemyOnAawake"), new GUIContent("awkae에서 적 생성시킬지 여부"));


        EditorGUILayout.PropertyField(serializedObject.FindProperty
("id"), new GUIContent("식별 코드"));

        if (GUILayout.Button("csv 불려오기"))
        {
            m_EnemySpawner.LoadEnemyDataFromCSV(m_EnemySpawner.id);
        }

        EditorGUILayout.LabelField("적 능력치");

        EditorGUILayout.PropertyField(e_Stat.FindPropertyRelative
("id"), new GUIContent("능력치 식별 코드"));

        EditorGUILayout.PropertyField(e_Stat.FindPropertyRelative
            ("name"),new GUIContent("이름"));
    
        EditorGUILayout.PropertyField(e_Stat.FindPropertyRelative
         ("hp"), new GUIContent("체력"));
    
        EditorGUILayout.PropertyField(e_Stat.FindPropertyRelative
         ("movespeed"), new GUIContent("이동 속도"));
 
        EditorGUILayout.PropertyField(e_Stat.FindPropertyRelative
 ("initattackdelay"), new GUIContent("공격 전 딜레이"));
  
        EditorGUILayout.PropertyField(e_Stat.FindPropertyRelative
 ("afterattackdelay"), new GUIContent("공격 후 딜레이"));


        EditorGUILayout.PropertyField(e_Stat.FindPropertyRelative
 ("searchstateID"), new GUIContent("정찰 여부"));

        EditorGUILayout.PropertyField(e_Stat.FindPropertyRelative
 ("movestateid"), new GUIContent("적 이동 패턴"));

        EditorGUILayout.LabelField("적 사용 모델링");
        for (int i = 0;i<m_EnemySpawner. EnemyModelList.Count; i++)
        {
            bool modeltoggle = EditorGUILayout.Toggle(m_EnemySpawner.EnemyModelList[i].name, m_EnemySpawner.ENemyModelNumber == i);
            if (modeltoggle)
            {
                m_EnemySpawner.ENemyModelNumber = i;
            }
        }
        EditorGUILayout.LabelField("적 공격 방식");
        for (int i = 0; i < m_EnemySpawner.AttackCOlliderList.Count; i++)
        {
            bool attacktoggle = EditorGUILayout.Toggle(m_EnemySpawner.AttackCOlliderList[i].name, m_EnemySpawner.ENemyModelNumber == i);
            if (attacktoggle)
            {
                m_EnemySpawner.enemyattacknumber = i;
                m_EnemySpawner.enemyData.attackstateID = i;
            }
        }

    

 



       

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

        m_EnemySpawner.Zip = EditorGUILayout.Foldout(m_EnemySpawner.Zip, "오브젝트");
        if (m_EnemySpawner.Zip)
        {
            EditorGUILayout.LabelField("적 모델링");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EnemyModelList"));

            EditorGUILayout.LabelField("적 공격");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AttackCOlliderList"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("EStatCSV"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
   

   
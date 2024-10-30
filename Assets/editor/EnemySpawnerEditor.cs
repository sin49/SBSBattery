using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Codice.Client.GameUI.Update;
using Codice.CM.Client.Differences.Graphic;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
 EnemySpawner m_EnemySpawner;

    SerializedProperty e_Stat;
    public void SaveEnemyData()
    {

        if (m_EnemySpawner.enemyData != null)
        {
            string filename = m_EnemySpawner. EStatCSV.name;

            string csvFilePath = Application.dataPath + $"/1.CSVDATA/EnemyStatData.csv";

            List<string> lines = new List<string>(File.ReadAllLines(csvFilePath));

            bool idExists = false;
            for (int i = 2; i < lines.Count; i++)
            {
                string[] values = lines[i].Split(',');

                if (int.Parse(values[0]) == m_EnemySpawner. enemyData.id) // id가 일치하는 데이터를 찾음
                {
                    // 데이터 업데이트
                    values[1] = m_EnemySpawner. enemyData.name;
                    values[2] = m_EnemySpawner. enemyData.hp.ToString();
                    values[3] = m_EnemySpawner. enemyData.movespeed.ToString();
                    values[4] = ((int)m_EnemySpawner.enemyData.attackstateID).ToString();
                    values[5] = ((int)m_EnemySpawner.enemyData.searchstateID).ToString();
                    values[6] = ((int)m_EnemySpawner.enemyData.movestateid).ToString();
                    values[7] = m_EnemySpawner. enemyData.initattackdelay.ToString();
                    values[8] = m_EnemySpawner.enemyData.afterattackdelay.ToString();

                    lines[i] = string.Join(",", values);
                    idExists = true;
                    break;
                }
            }

            if (!idExists)
            {
                // ID가 없을 경우 새로운 데이터를 추가
                string newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                m_EnemySpawner.    enemyData.id,
                  m_EnemySpawner.  enemyData.name,
                 m_EnemySpawner.   enemyData.hp,
              m_EnemySpawner.  enemyData.movespeed,
             m_EnemySpawner.   enemyData.attackstateID,
          m_EnemySpawner.      enemyData.searchstateID,
            m_EnemySpawner.    enemyData.movestateid,
              m_EnemySpawner.  enemyData.initattackdelay,
             m_EnemySpawner.       enemyData.afterattackdelay);

                lines.Add(newLine);
                Debug.Log($"New enemy data with ID {m_EnemySpawner.enemyData.id} has been added to CSV.");
            }

            // CSV 파일을 다시 저장
            File.WriteAllLines(csvFilePath, lines);
            AssetDatabase.Refresh();
            Debug.Log($"Enemy data for ID {m_EnemySpawner.enemyData.id} has been successfully saved to CSV.");
        }
        else
        {
            Debug.LogError("No enemy data available to save.");
        }
    }
    string[] EnemyModelNames;
    string[] ENemyAttackNames;
    string[] EnemyMoveNames;
    

    // CSV에서 불러온 데이터를 저장할 객체

    private void OnEnable()
    {
       m_EnemySpawner = (EnemySpawner)target;
        e_Stat = serializedObject.FindProperty("enemyData");
    }
   
    public override void OnInspectorGUI()
    {


        EditorGUILayout.PropertyField(serializedObject.FindProperty
("CreateEnemyOnAawake"), new GUIContent("awkae에서 적 생성시킬지 여부"));



        EditorGUILayout.PropertyField(serializedObject.FindProperty
("id"), new GUIContent("불려올 식별 코드"));



        if (GUILayout.Button("csv 불려오기"))
        {
            m_EnemySpawner.LoadEnemyDataFromCSV(m_EnemySpawner.id);
        }




        EditorGUILayout.LabelField("적 능력치");



        EditorGUILayout.PropertyField(e_Stat.FindPropertyRelative
("id"), new GUIContent("저장 식별 코드"));



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
            bool attacktoggle = EditorGUILayout.Toggle(m_EnemySpawner.AttackCOlliderList[i].name, m_EnemySpawner.enemyattacknumber == i);
            if (attacktoggle)
            {
                if (!m_EnemySpawner.updatedata)
                {
                    m_EnemySpawner.enemyattacknumber = i;
                    m_EnemySpawner.enemyData.attackstateID = i;
                }
            }
        }

    

 



       

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("저장"))
        {
          SaveEnemyData();
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
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EAttackCSV"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
   

   
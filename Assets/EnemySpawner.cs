using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;
using System;

public enum enemymodedlnumber {defaultform,breathform,jumpform,rushform,bulbform }
[Serializable]
public class enemystattest
{
    public int id;
    public string name;
    public float hp;
    public float movespeed;


    public int attackstateID;

    public EnemyMovePattern searchstateID;
    public EnemyMoveType movestateid;
    public float initattackdelay;
    public float afterattackdelay;
}
public class EnemySpawner : MonoBehaviour
{


    public int  ENemyModelNumber;
    public int enemystatusNumber;

 
    public int enemyattacknumber;
 


    public enemystattest enemyData;

    public TextAsset EStatCSV;

    public bool isloaded;

    public bool Zip;


    public List<GameObject> EnemyModelList= new List<GameObject>();
    public List<GameObject> AttackCOlliderList = new List<GameObject>();

    List<enemystattest> enemystattest_=new List<enemystattest>();

    public int id;
    void loadEnemyStatcsv()
    {
        if (isloaded) return;
        StringReader reader;
        bool firstlinereturn = true;
        if (EStatCSV != null)
        {


            reader = new StringReader(EStatCSV.text);

            while (true)
            {
                string line = reader.ReadLine();
                if (line == null) break;

                if (firstlinereturn)
                {
                    firstlinereturn = false;
                    continue;
                }

                string[] vaules = line.Split(',');


                enemystattest Estat = new enemystattest();
                Estat.id = int.Parse(vaules[0]);
                Estat.name = vaules[1];
                Estat.hp = float.Parse(vaules[2]);
                Estat.movespeed = float.Parse(vaules[3]);

                Estat.attackstateID = int.Parse(vaules[4]);
                Estat.searchstateID = (EnemyMovePattern)int.Parse(vaules[5]);
            Estat.movestateid = (EnemyMoveType)int.Parse(vaules[6]);
                Estat.initattackdelay = float.Parse(vaules[7]);
                Estat.afterattackdelay = float.Parse(vaules[8]);
                enemystattest_.Add(Estat);
            }
            isloaded = true;
        }
        else
        {
            Debug.Log("No Data...");
        }




    }

    public void LoadEnemyDataFromCSV(int statusId)
    {
        
      if(!isloaded)
        {
            loadEnemyStatcsv();
        }

        enemyData = enemystattest_[statusId];
    }
   
    public void SaveEnemyData()
    {

        if ( enemyData != null)
        {
            string csvFilePath = Application.dataPath + "/1.CSVDATA/EnemyStatData.csv";

            // CSV 파일에서 모든 줄을 읽음
            List<string> lines = new List<string>(File.ReadAllLines(csvFilePath));

            bool idExists = false;
            for (int i = 1; i < lines.Count; i++)
            {
                string[] values = lines[i].Split(',');

                if (int.Parse(values[0]) == enemyData.id) // id가 일치하는 데이터를 찾음
                {
                    // 데이터 업데이트
                    values[1] = enemyData.name;
                    values[2] = enemyData.hp.ToString();
                    values[3] = enemyData.movespeed.ToString();
                    values[4] = enemyData.attackstateID.ToString();
                    values[5] = enemyData.searchstateID.ToString();
                    values[6] = enemyData.movestateid.ToString();
                    values[7] = enemyData.initattackdelay.ToString();
                    values[8] = enemyData.afterattackdelay.ToString();

                    lines[i] = string.Join(",", values);
                    idExists = true;
                    break;
                }
            }

            if (!idExists)
            {
                // ID가 없을 경우 새로운 데이터를 추가
                string newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                    enemyData.id,
                    enemyData.name,
                    enemyData.hp,
                    enemyData.movespeed,
                    enemyData.attackstateID,
                    enemyData.searchstateID,
                    enemyData.movestateid,
                    enemyData.initattackdelay,
                    enemyData.afterattackdelay);

                lines.Add(newLine);
                Debug.Log($"New enemy data with ID {enemyData.id} has been added to CSV.");
            }

            // CSV 파일을 다시 저장
            File.WriteAllLines(csvFilePath, lines);
            Debug.Log($"Enemy data for ID {enemyData.id} has been successfully saved to CSV.");
        }
        else
        {
            Debug.LogError("No enemy data available to save.");
        }
    }
    private void Awake()
    {
        LoadEnemyDataFromCSV(0);
    }
    public void CreateEnemy()
    {
        GameObject model = EnemyModelList[ENemyModelNumber];
        Enemy e = Instantiate(model, this.transform.position, this.transform.rotation).transform.GetChild(0).GetComponent<Enemy>();

            enemystattest enemystattest = enemyData;
            enemystattest.attackstateID = enemyattacknumber;
        Vector3 v = AttackCOlliderList[enemyattacknumber].transform.position;
        GameObject attackcollider = Instantiate(AttackCOlliderList[enemyattacknumber], e.transform);
        attackcollider.transform.localPosition = v;
        e.attackCollider = attackcollider;
        //enemystattest.movestateid = enemymovenumber;
            e.LoadDataFromStatusDatas(enemystattest);

       
        e.CreateBySpawner = true;

       ;



    }
}

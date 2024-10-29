using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;
using System;
using UnityEditor;

public enum enemymodedlnumber {defaultform,breathform,jumpform,rushform,bulbform }
[Serializable]
public class enemystattest
{
    public int id;
    public string name;
    public float hp;
    public float movespeed;


    public int attackstateID;
   

    public int searchstateID;
    public int movestateid;
    public float initattackdelay;
    public float afterattackdelay;
}
public class EnemySpawner : MonoBehaviour
{


    public int  ENemyModelNumber;
    public int enemystatusNumber;

 
    public int enemyattacknumber;

    public bool CreateEnemyOnAawake;

    public enemystattest enemyData;

    public TextAsset EStatCSV;

    public TextAsset EAttackCSV;
    public TextAsset EnemySerachCsv;


    public bool Zip;

    public List<enemyattacktest> enemyattacks = new List<enemyattacktest>();
    public List<enemySearchTEst> enemysearchs = new List<enemySearchTEst>();
    enemyattacktest returnenemyattacktest(int n)
    {
        loadEnemyAttackCSV();
        return enemyattacks[n];
    }
    enemySearchTEst returnenemysearchtest(int n)
    {
        loadenemysearchcsv();
        return enemysearchs[n];
    }
    void loadenemysearchcsv()
    {
        if (EnemySerachCsv != null)
        {
            
            enemysearchs.Clear();
            StringReader reader = new StringReader(EnemySerachCsv.text);
            bool firstlinereturn = true;
            bool secondlinereturn = true;
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null) break;

                if (firstlinereturn)
                {
                    firstlinereturn = false;
                    continue;
                }
                if (secondlinereturn)
                {
                    secondlinereturn = false;
                    continue;
                }
                string[] vaules = line.Split(',');

                enemySearchTEst Esearch = new enemySearchTEst();
                Esearch.searchid = int.Parse(vaules[0]);
                Esearch.name = int.Parse(vaules[2]);
                Esearch.activeX = int.Parse(vaules[3]);
                Esearch.activeY = int.Parse(vaules[4]);
                Esearch.activeZ = int.Parse(vaules[5]);
                Esearch.activeoffsetX = int.Parse(vaules[6]);
                Esearch.activeoffsetY = int.Parse(vaules[7]);
                Esearch.activeoffsetZ = int.Parse(vaules[8]);
                Esearch.searchX = int.Parse(vaules[9]);
                Esearch.searchY = int.Parse(vaules[10]);
                Esearch.searchZ = int.Parse(vaules[11]);
                Esearch.searchoffsetX = int.Parse(vaules[12]);
                Esearch.searchoffsetY = int.Parse(vaules[13]);
                Esearch.searchoffsetZ = int.Parse(vaules[14]);

                enemysearchs.Add(Esearch);

            }
        }
    }
    void loadEnemyAttackCSV()
    {
        if (EAttackCSV != null)
        {
            enemyattacks.Clear();
            bool firstlinereturn = true;
            StringReader reader = new StringReader(EAttackCSV.text);

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


                enemyattacktest EAttack = new enemyattacktest();
                EAttack.attackid = int.Parse(vaules[0]);
                EAttack.attacktype = int.Parse(vaules[2]);


                //여기서 id를 읽어서 컴포넌트에 추가로 들어가는거 까지 해야함?
                EAttack.attackname = vaules[1];
                //EAttack.damage = int.Parse(vaules[3]);

                //적 어택 id읽은 다음 거기에 맞춰서 list에 얼만큼 추가할지가 들어가야 할듯?
                switch (EAttack.attacktype)
                {

                    case 3:
                    case 4:
                        for (int n = 3; n < 6; n++)
                        {
                            EAttack.SpecialVaule.Add(float.Parse(vaules[n]));
                        }
                        break;
                    default:
                        break;
                }


                enemyattacks.Add(EAttack);

            }
        }
    }


    public List<GameObject> EnemyModelList= new List<GameObject>();
    public List<GameObject> AttackCOlliderList = new List<GameObject>();
    public List<enemySearchTEst> searchTEsts = new List<enemySearchTEst>();
    List<enemystattest> enemystattest_=new List<enemystattest>();

    public int id;
    void loadEnemyStatcsv()
    {
        
        StringReader reader;
        bool firstlinereturn = true;
        bool secondlinereturn = true;
        if (EStatCSV != null)
        {

            enemystattest_.Clear();
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
                if (secondlinereturn)
                {
                    secondlinereturn = false;
                    continue;
                }

                string[] vaules = line.Split(',');


                enemystattest Estat = new enemystattest();
 
                Estat.id = int.Parse(vaules[0]);
                Estat.name = vaules[1];
                Estat.hp = float.Parse(vaules[2]);
                Estat.movespeed = float.Parse(vaules[3]);

                Estat.attackstateID = int.Parse(vaules[4]);
                enemyattacknumber = int.Parse(vaules[4]);
                Estat.searchstateID = int.Parse(vaules[5]);
            Estat.movestateid = int.Parse(vaules[6]);
                Estat.initattackdelay = float.Parse(vaules[7]);
                Estat.afterattackdelay = float.Parse(vaules[8]);
             
                enemystattest_.Add(Estat);
            }
 
        }
        else
        {
            Debug.Log("No Data...");
        }




    }

    public void LoadEnemyDataFromCSV(int statusId)
    {
        
     
            loadEnemyStatcsv();


        enemyData = enemystattest_[statusId];
    }
   
    public void SaveEnemyData()
    {

        if ( enemyData != null)
        {
            string filename = EStatCSV.name;
           
            string csvFilePath = Application.dataPath + $"/1.CSVDATA/EnemyStatData.csv";

            List<string> lines = new List<string>(File.ReadAllLines(csvFilePath));

            bool idExists = false;
            for (int i = 2; i < lines.Count; i++)
            {
                string[] values = lines[i].Split(',');

                if (int.Parse(values[0]) == enemyData.id) // id가 일치하는 데이터를 찾음
                {
                    // 데이터 업데이트
                    values[1] = enemyData.name;
                    values[2] = enemyData.hp.ToString();
                    values[3] = enemyData.movespeed.ToString();
                    values[4] = ((int)enemyData.attackstateID).ToString();
                    values[5] = ((int)enemyData.searchstateID).ToString();
                    values[6] = ((int)enemyData.movestateid).ToString();
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
            AssetDatabase.Refresh();
            Debug.Log($"Enemy data for ID {enemyData.id} has been successfully saved to CSV.");
        }
        else
        {
            Debug.LogError("No enemy data available to save.");
        }
    }
    private void Awake()
    {
        if (CreateEnemyOnAawake)
        {
            
                LoadEnemyDataFromCSV(id);
            CreateEnemy();
        }
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
       
        
        e.LoadDataFromStatusDatas(enemystattest
            ,returnenemysearchtest(enemystattest.searchstateID), returnenemyattacktest(enemyattacknumber));

       
        e.CreateBySpawner = true;

    



    }
}

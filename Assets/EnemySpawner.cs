using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    public bool updatedata;
    public List<enemyattacktest> enemyattacks = new List<enemyattacktest>();

    enemyattacktest returnenemyattacktest(int n)
    {
        loadEnemyAttackCSV();
        return enemyattacks[n];
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
  
    List<enemystattest> enemystattest_=new List<enemystattest>();

    public int id;
    void loadEnemyStatcsv()
    {
        updatedata = true;
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

        updatedata = false;


    }

    public void LoadEnemyDataFromCSV(int statusId)
    {
        
     
            loadEnemyStatcsv();


        enemyData = enemystattest_[statusId];
        updatedata = true;
        enemyattacknumber = enemyData.attackstateID;
        updatedata = false;
    }
   
   
    private void Awake()
    {
        if (CreateEnemyOnAawake)
        {
            
                LoadEnemyDataFromCSV(id);
            CreateEnemy();
            this.gameObject.SetActive(false);
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
            , returnenemyattacktest(enemyattacknumber));

       
        e.CreateBySpawner = true;

    



    }
}

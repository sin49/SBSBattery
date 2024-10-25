using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UIElements;

[Serializable]
public class enemyattacktest
{
    public int attackid;
    public int attacktype;
    public string attackname;
    public List<float> SpecialVaule = new List<float>();

   
}
[ExecuteAlways]
public class ETableManager : MonoBehaviour
{
    public static ETableManager instance;
    public TextAsset EnemyStatCsV;
    public TextAsset EnemyActionCsv;
    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> AttackCollider = new List<GameObject>();
    public List<enemystattest> enemystats = new List<enemystattest>();
    public List<enemyattacktest> enemyattacks = new List<enemyattacktest>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        loadEnemyStatcsv();
    }
    public enemystattest returnenemydata(int prioritynumber)
    {
       
        if (prioritynumber < enemystats.Count)
        {
            enemystattest estat = enemystats[prioritynumber];


            return estat;
        }
        else
            return null;
    }
    void loadEnemyStatcsv()
    {
        StringReader reader;
        bool firstlinereturn = true;
        if (EnemyStatCsV != null)
        {


            reader = new StringReader(EnemyStatCsV.text);

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
                    Estat.movestateid =(EnemyMoveType) int.Parse(vaules[6]);
                    Estat.initattackdelay = float.Parse(vaules[7]);
                    Estat.afterattackdelay = float.Parse(vaules[8]);
                    enemystats.Add(Estat);
                
            }
        }
        firstlinereturn = true;
        if (EnemyActionCsv != null)
        {
            reader = new StringReader(EnemyActionCsv.text);

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
                    switch (EAttack.attacktype) {

                    case 3:
                        case 4:
                          for(int n = 3; n < 6; n++)
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
}

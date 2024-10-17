using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class enemystattest {
    public int id;
    public string name;
    public float hp;
    public float movespeed;


    public int attackstateID;

    public int searchstateID;
}
public class enemyattacktest
{
    public int attackid;
    public int attacktype;
    public string attackname;
    public int attack;
    public float initattackdelay;
    public float afterattackdelay;
    public float attackspeed;
    public List<float> attackComponentList = new List<float>();
}

public class ETableManager : MonoBehaviour
{
    public static ETableManager instance;
    public TextAsset EnemyStatCsV;
    public TextAsset EnemyActionCsv;
    public int StatColumns;
    List<enemystattest> enemystats = new List<enemystattest>();
    List<enemyattacktest> enemyattacks = new List<enemyattacktest>();
    private void Awake()
    {
        if (instance == null)
            instance = this;
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

                if (vaules.Length == StatColumns)
                {
                    enemystattest Estat = new enemystattest();
                    Estat.id = int.Parse(vaules[0]);
                    Estat.name = vaules[1];
                    Estat.hp = float.Parse(vaules[2]);
                    Estat.movespeed = float.Parse(vaules[3]);

                    Estat.attackstateID = int.Parse(vaules[4]);
                    Estat.searchstateID = int.Parse(vaules[5]);

                    enemystats.Add(Estat);
                }
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

                if (vaules.Length == StatColumns)
                {
                    enemyattacktest EAttack = new enemyattacktest();
                    EAttack.attackid = int.Parse(vaules[0]);
                    EAttack.attacktype = int.Parse(vaules[1]);
                    //여기서 id를 읽어서 컴포넌트에 추가로 들어가는거 까지 해야함?
                    EAttack.attackname = vaules[2];
                    EAttack.attack = int.Parse(vaules[3]);
                    EAttack.initattackdelay = float.Parse(vaules[4]);
                    EAttack.afterattackdelay = float.Parse(vaules[5]);
                    //적 어택 id읽은 다음 거기에 맞춰서 list에 얼만큼 추가할지가 들어가야 할듯?
                    switch (EAttack.attacktype) { }


                    enemyattacks.Add(EAttack);
                }
            }
        }


        void Update()
        {

        }
    }
}

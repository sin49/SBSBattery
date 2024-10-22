using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UIElements;
[Serializable]
public class enemystattest {
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
[Serializable]
public class enemyattacktest
{
    public int attackid;
    public int attacktype;
    public string attackname;
    public List<float> SpecialVaule = new List<float>();

   
}

public class ETableManager : MonoBehaviour
{
    public static ETableManager instance;
    public TextAsset EnemyStatCsV;
    public TextAsset EnemyActionCsv;

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
        enemystattest estat= enemystats[prioritynumber];


        return estat;
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
                    Estat.searchstateID = int.Parse(vaules[5]);
                    Estat.movestateid = int.Parse(vaules[6]);
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
      

        //enemy 스크립트에서 사용할 번호(식별키)를 따고 그걸 기반해서 적 스탯+
        //이동 방식+공격 방식 가져와서 적용 enemy awake에 넣기

        //변경 ->커스텀에디터를 이용해 식별키를 넣고 숫자 바꾸면 바꿔지게
        //저장-> 커에로 저장 버튼 만들기
        //이동 버튼 메터리얼,탐색,능력치,공격 으로 이동하는 버튼 만들기
        //하위 저장) 능력치, 공격은 csv 저장 나머지는 만든다면 스크립터블 오브젝트로

    }
}

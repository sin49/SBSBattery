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
                 

                    //���⼭ id�� �о ������Ʈ�� �߰��� ���°� ���� �ؾ���?
                    EAttack.attackname = vaules[1];
                    //EAttack.damage = int.Parse(vaules[3]);
        
                    //�� ���� id���� ���� �ű⿡ ���缭 list�� ��ŭ �߰������� ���� �ҵ�?
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
      

        //enemy ��ũ��Ʈ���� ����� ��ȣ(�ĺ�Ű)�� ���� �װ� ����ؼ� �� ����+
        //�̵� ���+���� ��� �����ͼ� ���� enemy awake�� �ֱ�

        //���� ->Ŀ���ҿ����͸� �̿��� �ĺ�Ű�� �ְ� ���� �ٲٸ� �ٲ�����
        //����-> Ŀ���� ���� ��ư �����
        //�̵� ��ư ���͸���,Ž��,�ɷ�ġ,���� ���� �̵��ϴ� ��ư �����
        //���� ����) �ɷ�ġ, ������ csv ���� �������� ����ٸ� ��ũ���ͺ� ������Ʈ��

    }
}

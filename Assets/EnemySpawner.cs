using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public enum enemymodedlnumber {defaultform,breathform,jumpform,rushform,bulbform }

public class EnemySpawner : MonoBehaviour
{


    public int  ENemyModelNumber;
    public int enemystatusNumber;
    public int AttackColliderNumber;
 
    public int enemyattacknumber;
    public int enemymovenumber;
    public int enemypatorolnumber;

    public enemystattest enemyData;


    public List<GameObject> EnemyModelList= new List<GameObject>();
    public List<GameObject> AttackCOlliderList = new List<GameObject>();


    public int id;

    // CSV���� �����͸� �ҷ����� �޼���
    private void LoadEnemyDataFromCSV(int statusId)
    {
        var tableManager = ETableManager.instance;
        if (tableManager != null)
        {
            enemyData = tableManager.returnenemydata(statusId);

            if (enemyData != null)
            {
                Debug.Log($"Loaded enemy data for ID {statusId} from CSV.");
            }
            else
            {
                Debug.LogWarning("ID not found. Preparing for new entry creation.");
                enemyData = new enemystattest(); // �� �����͸� ���� �غ�
                enemyData.id = statusId;
            }
        }
        else
        {
            Debug.LogError("ETableManager instance not found.");
        }
    }
    // CSV ���Ͽ� �����ϴ� �޼��� (�������� �ʴ� ��� �߰�)
    public void SaveEnemyData()
    {

        if ( enemyData != null)
        {
            string csvFilePath = Application.dataPath + "/1.CSVDATA/EnemyStatData.csv";

            // CSV ���Ͽ��� ��� ���� ����
            List<string> lines = new List<string>(File.ReadAllLines(csvFilePath));

            bool idExists = false;
            for (int i = 1; i < lines.Count; i++)
            {
                string[] values = lines[i].Split(',');

                if (int.Parse(values[0]) == enemyData.id) // id�� ��ġ�ϴ� �����͸� ã��
                {
                    // ������ ������Ʈ
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
                // ID�� ���� ��� ���ο� �����͸� �߰�
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

            // CSV ������ �ٽ� ����
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
        Enemy e = model.transform.GetChild(0).GetComponent<Enemy>();

            enemystattest enemystattest = ETableManager.instance.returnenemydata(enemystatusNumber);
            enemystattest.attackstateID = enemyattacknumber;
            enemystattest.movestateid = enemymovenumber;
            e.LoadDataFromStatusDatas(enemystattest);

       
        e.CreateBySpawner = true;
        e.eStat.movepattern = (EnemyMovePattern)enemypatorolnumber;
        Instantiate(model, this.transform.position, this.transform.rotation);



    }
}

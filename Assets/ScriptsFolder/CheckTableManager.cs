using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckTableManager : MonoBehaviour
{

    public static CheckTableManager instance;

    public TextAsset CheckTable;

    public List<CheckPointData> checkpoints = new List<CheckPointData>();

    public int Listlength;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        loadCheckCSV();
    }
    public CheckPointData ReturnCheckCSVData(int n)
    {
        if (SceneManager.GetActiveScene().name == "Stage1-6" || n > checkpoints.Count - 1)
            n--;
        return checkpoints[n];
    }
    private void Update()
    {
        //µð¹ö±ë¿ë
        //if (checkpoints != null)
        //    Listlength = checkpoints.Count;
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //    GameManager.instance.loadscenebycheckpoint(0);
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //    GameManager.instance.loadscenebycheckpoint(1);
    }
    void loadCheckCSV()
    {
        StringReader reader;
        bool firstlinereturn = true;
        if (CheckTable != null)
        {


            reader = new StringReader(CheckTable.text);

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


                CheckPointData Cdata = new CheckPointData();
                Cdata.index = int.Parse(vaules[0]);
                Cdata.scenename = vaules[1];
                Cdata.PlayerTransformtype = int.Parse(vaules[2]);

                checkpoints.Add(Cdata);

            }

            //OnlyTestAllCheckPoint();
        }

    }

    public List<int> group = new List<int>();

    public void OnlyTestAllCheckPoint()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "CheckPointData.json");
        for (int i = 0; i < checkpoints.Count; i++)
        {
            group.Add(checkpoints[i].index);
        }
        
        SavePoint sp = new SavePoint();
        sp.points = group;

        string data = JsonUtility.ToJson(sp);
        File.WriteAllText(filePath, data);

        PlayerPrefs.SetInt("CheckPointIndex", 1);
    }
}

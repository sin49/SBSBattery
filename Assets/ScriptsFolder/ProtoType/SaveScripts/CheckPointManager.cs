using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SavePoint
{
    public List<int> points = new List<int>();
}

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;

    public List<int> checkPoints = new List<int>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("SavePoint"))
        {
            LoadCheckList();
        }
    }

    public void LoadCheckList()
    {
        string path = Path.Combine(Application.persistentDataPath, "CheckPointData.json");

        if (File.Exists(path))
        {
            var a = File.ReadAllText(path);
            SavePoint sp = JsonUtility.FromJson<SavePoint>(a);

            checkPoints = sp.points;
        }
    }

    public void SaveCheckPointData(int num)
    {
        if (CheckSamePoint(num)) return;

        SavePoint sp = new SavePoint();

        checkPoints.Add(num);

        sp.points = checkPoints;

        string json = JsonUtility.ToJson(sp);
        string filePath = Path.Combine(Application.persistentDataPath, "CheckPointData.json");

        File.WriteAllText(filePath, json);
    }

    public bool CheckSamePoint(int num)
    {
        bool check = false;

        for (int i = 0; i < checkPoints.Count; i++)
        {
            if (checkPoints[i] == num)
            {
                check = true;
                return check;
            }                
        }
        return check;
    }
}

using JetBrains.Annotations;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PoolObjects
{
    public string poolName;
    public GameObject poolPrefab;
    public GameObject[] poolPrefabs;
    public Queue<GameObject> poolQueue;
}

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager instance;

    public GameObject hittedEffect;

    public GameObject[] attackEffect;
    public GameObject[] landingEffect;
    public GameObject[] jumpEffect;

    public PoolObjects[] poolObjects;
    public Action<GameObject> actionPooling;

    private void Awake()
    {
        instance = this;

        TestPoolingInit();
    }    

    // 풀링 초기화
    void TestPoolingInit()
    {
        foreach (var pool in poolObjects)
        {
            pool.poolQueue = new Queue<GameObject>();           
            
            Debug.Log($"큐 사이즈: {pool.poolQueue.Count}");
        }

        /*for (int i = 0; i < poolObjects.Length; i++)
        {


            GameObject[] poolPrefabs = poolObjects[i].poolPrefabs;
            for (int j = 0; j < poolPrefabs.Length; j++)
            {
                GameObject prefab = Instantiate(poolObjects[i].poolPrefab, transform.position, Quaternion.identity);
                prefab.transform.SetParent(this.transform);
                prefab.SetActive(false);
                poolPrefabs[j] = prefab;
            }
        }*/
    }
    
    

    //풀링 오브젝트 대출
    public void GetPoolObject(string poolObj, Transform obj)
    {
        for (int z = 0; z < poolObjects.Length; z++)
        {
            GameObject addPoolObj;
            if (poolObjects[z].poolName == poolObj)
            {
                if (poolObjects[z].poolQueue.Count == 0)
                {
                    addPoolObj = Instantiate(poolObjects[z].poolPrefab, transform.position, Quaternion.identity);
                    int index = addPoolObj.name.IndexOf("(Clone)");
                    addPoolObj.name = addPoolObj.name.Substring(0, index);
                    addPoolObj.transform.SetParent(transform);
                }
                else
                {
                    addPoolObj = poolObjects[z].poolQueue.Dequeue();
                    addPoolObj.SetActive(true);
                }
                addPoolObj.transform.position = obj.position;
                addPoolObj.transform.rotation = obj.rotation;                
                Debug.Log($"꺼낸 후 큐 사이즈:{poolObjects[z].poolQueue.Count}");
                return;
            }
        }

        /*for (int i = 0; i < poolObjects.Length; i++)
        {
            if (poolObjects[i].poolName == poolObj)
            {
                Debug.Log($"{obj} 발사해야함");
                //GameObject[] poolPrefabs = poolObjects[i].poolPrefabs;

                for (int j = 0; j < poolObjects[i].poolPrefabs.Length; j++)
                {
                    if (poolObjects[i].poolPrefabs[j].activeSelf)
                    {
                        Debug.Log("이미 발사 중이다.");
                        continue;
                    }
                    else
                    {
                        Debug.Log("생성 하자.");
                        poolObjects[i].poolPrefabs[j].transform.position = obj.position;
                        poolObjects[i].poolPrefabs[j].transform.rotation = obj.rotation;
                        poolObjects[i].poolPrefabs[j].SetActive(true);
                        return;
                    }
                }
            }
        }*/
    }

    //풀링 오브젝트 반납
    public void ReturnPoolObject(GameObject obj)
    {
        //obj.transform.SetParent(transform);
        obj.SetActive(false);        
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (obj.name == poolObjects[i].poolPrefab.name)
            {
                poolObjects[i].poolQueue.Enqueue(obj);
                Debug.Log($"풀링 오브젝트 반납받은 후 큐 사이즈:{poolObjects[i].poolQueue.Count}");
            }
        }
    }
}

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
    public List<GameObject> poolList;
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

    private void Awake()
    {
        instance = this;

        TestPoolingInit();
    }    

    // Ǯ�� �ʱ�ȭ
    void TestPoolingInit()
    {
        /*foreach (var pool in poolObjects)
        {
            pool.poolQueue = new Queue<GameObject>();
            GameObject poolPre = Instantiate(pool.poolPrefab, transform.position, Quaternion.identity);
            for (int i = 0; i < pool.poolList.Capacity; i++)
            {
                poolPre.SetActive(false);
                pool.poolQueue.Enqueue(poolPre);
                poolPre.transform.SetParent(transform);
            }
        }*/

        for (int i = 0; i < poolObjects.Length; i++)
        {


            GameObject[] poolPrefabs = poolObjects[i].poolPrefabs;
            for (int j = 0; j < poolPrefabs.Length; j++)
            {
                GameObject prefab = Instantiate(poolObjects[i].poolPrefab, transform.position, Quaternion.identity);
                prefab.transform.SetParent(this.transform);
                prefab.SetActive(false);
                poolPrefabs[j] = prefab;
            }
        }
    }
    
    
    //Ǯ�� ������Ʈ ����
    public void GetPoolObject(string poolObj, Transform obj)
    {
        /*for (int z = 0; z < poolObjects.Length; z++)
        {
            if (poolObjects[z].poolName == poolObj)
            {
                GameObject QueuePre = poolObjects[z].poolQueue.Dequeue();
                QueuePre.SetActive(true);
            }
        }*/

        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (poolObjects[i].poolName == poolObj)
            {
                Debug.Log($"{obj} �߻��ؾ���");
                //GameObject[] poolPrefabs = poolObjects[i].poolPrefabs;

                for (int j = 0; j < poolObjects[i].poolPrefabs.Length; j++)
                {
                    if (poolObjects[i].poolPrefabs[j].activeSelf)
                    {
                        Debug.Log("�̹� �߻� ���̴�.");
                        continue;
                    }
                    else
                    {
                        Debug.Log("���� ����.");
                        poolObjects[i].poolPrefabs[j].transform.position = obj.position;
                        poolObjects[i].poolPrefabs[j].transform.rotation = obj.rotation;
                        poolObjects[i].poolPrefabs[j].SetActive(true);
                        return;
                    }
                }
            }
        }
    }

    //Ǯ�� ������Ʈ �ݳ�
    public void ReturnPoolObject(GameObject obj)
    {
        obj.transform.SetParent(transform);
        obj.SetActive(false);        
    }
}

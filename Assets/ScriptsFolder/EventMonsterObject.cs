using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading;
using UnityEngine;

public class EventMonsterObject : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform parent;
    public Transform[] spawnGroup = new Transform[0];
    public GameObject group;
    int count;
    public int maxCount;
    private void Start()
    {
        //StartCoroutine(EventMonsterSpawn());
        DeActive();
    }

    public void DeActive()
    {
        group.SetActive(false);
    }
    #region 이전 코드
    /*IEnumerator EventMonsterSpawn()
    {
        while(count < maxCount)
        {
            InstantiateMonster();
            count++;
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(3f);
    }

    public void InstantiateMonster()
    {
        for (int i = 0; i < spawnGroup.Length; i++)
        {
            var obj = Instantiate(enemyPrefab, spawnGroup[i].position, MonsterRotation());
            obj.transform.SetParent(parent);
        }
    }

    public Quaternion MonsterRotation()
    {
        int rotX = Random.Range(0, 360);
        int rotY = Random.Range(0, 360);
        int rotZ = Random.Range(0, 360);
        Quaternion rot = Quaternion.Euler(rotX, rotY, rotZ);

        return rot;
    }*/
    #endregion
}

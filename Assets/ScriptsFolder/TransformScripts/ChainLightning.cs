using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class SearchObject
{
    public GameObject enemy;
    public float distance;
}

public class ChainLightning : MonoBehaviour
{
    // 체인    
    public float damage;
    public float moveSpeed;
    public float sphereRange;

    public SearchObject searchObj;
    public List<SearchObject> searchGroup; // 탐지될 적 오브젝트 리스트
    public List<GameObject> endEnemy; // 1회 피격당한 적 오브젝트 리스트

    [Header("최단 거리에 있는 적 오브젝트")]
    public float closetDistance; // 최단 거리
    bool targetEnd;
    public GameObject target; // 타겟 지정

    public LineRenderer lineRenderer;

    private void Awake()
    {
        damage = PlayerStat.instance.atk*2f;
        searchGroup = new List<SearchObject>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (target != null)
        {
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position), 15 * Time.deltaTime);
            transform.LookAt(target.transform.position);
        }
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);        
    }

    public void SearchEnemy()
    {
        searchGroup.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRange);
        
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Enemy"))
            {

                if (checkEndChainObject(colliders[i].gameObject))
                    continue;

                searchObj = new SearchObject();

                //Debug.Log($"탐지된 오브젝트:{colliders[i].gameObject}");
                float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
                searchObj.enemy = colliders[i].gameObject;
                searchObj.distance = distance;
                //Debug.Log($"저장된 오브젝트: {searchObj.enemy}\n저장된 거리값:{searchObj.distance}");
                searchGroup.Add(searchObj);
                //Debug.Log($"저장된 리스트:{searchGroup[i].enemy}");                
            }            
        }



        if (searchGroup.Count > 0)
        {
            closetDistance = searchGroup[0].distance;

            for (int i = 0; i < searchGroup.Count; i++)
            {
                if (closetDistance >= searchGroup[i].distance)
                {
                    closetDistance = searchGroup[i].distance;
                    target = searchGroup[i].enemy;
                }
            }
        }
        else
        {
            Destroy(gameObject, 0.2f);
        }
    }

    public bool checkEndChainObject(GameObject chainTarget)
    {
        bool complete = false;

        for (int i = 0; i < endEnemy.Count; i++)
        {
            if (chainTarget == endEnemy[i])
            {
                complete = true;
                return complete;
            }
        }
        return complete;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!checkEndChainObject(other.gameObject))
            {
                if (other.GetComponent<Enemy>())
                {
                    Enemy enemy = other.GetComponent<Enemy>();
                    enemy.Damaged(damage);
                }
                else
                {
                    other.GetComponent<BoxTestt>().Damaged(damage);
                }
              
                endEnemy.Add(other.gameObject);
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, other.transform.position);
                SearchEnemy();                
            }
        }        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRange);
    }
}

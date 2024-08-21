using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySearchCollider : MonoBehaviour, colliderDisplayer
{
    Enemy enemy;
    BoxCollider searchCollider;
    public MeshRenderer childMat;

    public void ActiveColliderDisplay()
    {
        if(childMat != null)
        childMat.gameObject.SetActive(true);
    }

    public void DeactiveColliderDisplay()
    {
        if(childMat != null)
        childMat.gameObject.SetActive(false);
    }

    public void registerColliderDIsplay()
    {
        if (ColliderDisplayManager.Instance != null && childMat != null)
        {
            ColliderDisplayManager.Instance.register(this);
        }
    }

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        searchCollider = GetComponent<BoxCollider>();        
        if (childMat == null && transform.childCount != 0)
        {
            childMat = GetComponentInChildren<MeshRenderer>();
        }
    }

    private void Start()
    {
        registerColliderDIsplay();
    }

    private void FixedUpdate()
    {
        if (enemy.searchCollider != null && searchCollider != null)
        {
            searchCollider.center = enemy.searchColliderPos;
            searchCollider.size = enemy.searchColliderRange;
            if (childMat != null)
            {
                childMat.transform.localPosition = searchCollider.center;
                childMat.transform.localScale = searchCollider.size;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (CharColliderColor.instance != null && childMat != null)
        {
            childMat.sharedMaterials[0].color = CharColliderColor.instance.searchRange;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.target = other.transform;
            if (!enemy.wallCheck)
            {
                enemy.searchPlayer = true;
                enemy.onPatrol = false;
            }
            else
            {
                enemy.searchPlayer = false;
                enemy.onPatrol = true;
            }            
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
     
        if (other.CompareTag("Player"))
        {
            if (enemy.onPatrol)
            {
                enemy.searchPlayer = false;
            }
        }
    }*/
}

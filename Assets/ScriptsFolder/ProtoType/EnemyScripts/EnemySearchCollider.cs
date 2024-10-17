using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class EnemySearchCollider : MonoBehaviour, colliderDisplayer
{
    BoxCollider searchCollider;
    public MeshRenderer childMat;
    public bool searchPlayer, onPatrol;

    public void ActiveColliderDisplay()
    {
        if(childMat != null)
        childMat.enabled = true;
    }

    public void DeactiveColliderDisplay()
    {
        if(childMat != null)
        childMat.enabled = false;
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
        if (/*tap.searchCollider != null &&*/ searchCollider != null)
        {
            //searchCollider.center = tap.searchColliderPos;
            //searchCollider.size = tap.searchColliderRange;
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
    public bool wallCheck;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //enemy.target = other.transform;
            if (!wallCheck)
            {
                searchPlayer = true;
                onPatrol = false;
            }
            else
            {
                searchPlayer = false;
                onPatrol = true;
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

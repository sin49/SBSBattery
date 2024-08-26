using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AttackColliderRange : MonoBehaviour, colliderDisplayer
{
    public Enemy enemy;
    BoxCollider rangeCollider;
    public MeshRenderer childMat;

    public void registerColliderDIsplay()
    {
        if (ColliderDisplayManager.Instance != null && childMat != null)
        {
            ColliderDisplayManager.Instance.register(this);
        }
        else
        {
            Debug.Log("콜라이더 디스플레이 받지 못한 오브젝트 있음");
        }
    }

    public void ActiveColliderDisplay()
    {
        if (childMat != null)
            childMat.enabled = true;
    }

    public void DeactiveColliderDisplay()
    {
        if (childMat != null)
            childMat.enabled = false;
    }

    private void Awake()
    {
        enemy = transform.parent.GetComponent<Enemy>();
        rangeCollider = GetComponent<BoxCollider>();
        if (enemy.rangeCollider != null)
        {
            enemy.rangePos = rangeCollider.center;
            enemy.rangeSize = rangeCollider.size;
        }

        if (childMat == null && transform.childCount != 0)
        {
            childMat = GetComponentInChildren<MeshRenderer>();
        }
    }

    private void Start()
    {
        registerColliderDIsplay();
    }

    private void Update()
    {
        if (rangeCollider != null)
        {
            rangeCollider.center = enemy.rangePos;
            rangeCollider.size = enemy.rangeSize;

            if (childMat != null)
            {
                childMat.transform.localPosition = rangeCollider.center;
                childMat.transform.localScale = rangeCollider.size;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (CharColliderColor.instance != null && childMat != null)
        {
            childMat.sharedMaterials[0].color = CharColliderColor.instance.attackActiveRange;
        }


        /*Collider collider = GetComponent<Collider>();
        if (collider is BoxCollider box)
        {
            Gizmos.color = Color.magenta;
            Transform boxTransform = transform;
            Vector3 center = box.bounds.center;
            Vector3 halfSize = box.bounds.size / 2;
            Vector3[] points = new Vector3[8];
            points[0] = (center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z));
            points[1] = (center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z));
            points[2] = (center + new Vector3(-halfSize.x, halfSize.y, halfSize.z));
            points[3] = (center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z));
            points[4] = (center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z));
            points[5] = (center + new Vector3(halfSize.x, halfSize.y, -halfSize.z));
            points[6] = (center + new Vector3(halfSize.x, -halfSize.y, halfSize.z));
            points[7] = (center + new Vector3(halfSize.x, halfSize.y, halfSize.z));

            Gizmos.DrawLine(points[0], points[1]);
            Gizmos.DrawLine(points[0], points[3]);
            Gizmos.DrawLine(points[0], points[4]);

            Gizmos.DrawLine(points[1], points[2]);
            Gizmos.DrawLine(points[1], points[5]);


            Gizmos.DrawLine(points[2], points[3]);
            Gizmos.DrawLine(points[2], points[7]);

            Gizmos.DrawLine(points[3], points[6]);

            Gizmos.DrawLine(points[4], points[5]);
            Gizmos.DrawLine(points[4], points[6]);

            Gizmos.DrawLine(points[5], points[7]);

            Gizmos.DrawLine(points[6], points[7]);
        }*/
    }


    private void OnTriggerStay(Collider other)
    {
        //ebug.Log($"트리거 감지 중 {other.gameObject}");   
        if (other.CompareTag("Player") &&enemy.target!=null && !enemy.onStun && !enemy.onAttack)
        {
            if (!enemy.wallCheck)
            {
                if (enemy.transform.position.x < enemy.target.position.x)
                {
                    enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    enemy.transform.rotation = Quaternion.Euler(0, -90, 0);
                }

                enemy.onAttack = true;
            }

            if (!enemy.wallCheck)            
                enemy.attackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enemy.target != null && !enemy.onStun)
        {
            enemy.attackRange = false;
        }
    }
}

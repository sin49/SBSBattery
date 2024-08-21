using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AttackColliderRange : MonoBehaviour
{
    public Enemy enemy;
    BoxCollider rangeCollider;
    private void Awake()
    {
        enemy = transform.parent.GetComponent<Enemy>();
        rangeCollider = GetComponent<BoxCollider>();
        if (enemy.rangeCollider != null)
        {
            enemy.rangePos = rangeCollider.center;
            enemy.rangeSize = rangeCollider.size;
        }
    }

    private void Update()
    {
        if (rangeCollider != null)
        {
            rangeCollider.center = enemy.rangePos;
            rangeCollider.size = enemy.rangeSize;
        }
    }

    private void OnDrawGizmos()
    {
        if(CharColliderColor.instance != null)
            Gizmos.color = CharColliderColor.instance.attackActiveRange;

        Collider collider = GetComponent<Collider>();
        
        if (collider is BoxCollider box)
        {
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

            /*for (int i = 0; i < points.Length; i++)
            {
                transform.TransformPoint(points[i]);
            }*/

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

            
            /*Vector3[] points = new Vector3[4]
            {
                center+new Vector3(-halfSize.x, -halfSize.y, -halfSize.z),
                center+new Vector3(halfSize.x, -halfSize.y, -halfSize.z),
                center+new Vector3(-halfSize.x, halfSize.y, -halfSize.z),
                center+new Vector3(-halfSize.x, -halfSize.y, halfSize.z)
            };
            Vector3[] secondPoints = new Vector3[4]
            {
                transform.TransformPoint(center+new Vector3(-halfSize.x, halfSize.y, halfSize.z)),
                transform.TransformPoint(center+new Vector3(-halfSize.x, -halfSize.y, halfSize.z)),
                transform.TransformPoint(center+new Vector3(halfSize.x, halfSize.y, halfSize.z)),
                transform.TransformPoint(center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z))
            };
            Vector3[] thirdPoints = new Vector3[4]
            {
                transform.TransformPoint(center+new Vector3(halfSize.x, halfSize.y, -halfSize.z)),
                transform.TransformPoint(center+new Vector3(-halfSize.x, halfSize.y, -halfSize.z)),
                transform.TransformPoint(center+new Vector3(halfSize.x, halfSize.y, halfSize.z)),
                transform.TransformPoint(center+new Vector3(halfSize.x, -halfSize.y, -halfSize.z))
            };
            Vector3[] fourthPoints = new Vector3[4]
            {
                transform.TransformPoint(center+new Vector3(halfSize.x, -halfSize.y, halfSize.z)),
                transform.TransformPoint(center+new Vector3(-halfSize.x, -halfSize.y,halfSize.z)),
                transform.TransformPoint(center+new Vector3(halfSize.x, halfSize.y, halfSize.z)),
                transform.TransformPoint(center+new Vector3(halfSize.x, -halfSize.y, -halfSize.z))
            };
            for (int j = 1; j < points.Length; j++)
            {
                Gizmos.DrawLine(points[0], points[j]);
                Gizmos.DrawLine(secondPoints[0], secondPoints[j]);
                Gizmos.DrawLine(thirdPoints[0], thirdPoints[j]);
                Gizmos.DrawLine(fourthPoints[0], fourthPoints[j]);
            }*/
        }        
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

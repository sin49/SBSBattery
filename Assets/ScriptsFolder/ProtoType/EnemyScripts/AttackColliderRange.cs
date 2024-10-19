using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
[ExecuteAlways]
public class AttackColliderRange : MonoBehaviour, colliderDisplayer
{
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
            Debug.Log("�ݶ��̴� ���÷��� ���� ���� ������Ʈ ����");
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
        //enemy = transform.parent.GetComponent<Enemy>();
        rangeCollider = GetComponent<BoxCollider>();
        //if (enemy.tap.rangeCollider != null)
        //{
        //    enemy.tap.rangePos = rangeCollider.center;
        //    enemy.tap.rangeSize = rangeCollider.size;
        //}

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
            //rangeCollider.center = enemy.tap.rangePos;
            //rangeCollider.size = enemy.tap.rangeSize;

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
    }

    public Enemy enemy;
    public EnemyTrackingAndPatrol tap;
    private void OnTriggerStay(Collider other)
    {
        //ebug.Log($"Ʈ���� ���� �� {other.gameObject}");   
        if (other.CompareTag("Player") /*&&enemy.target!=null*/ && !enemy.onStun && !enemy.activeAttack)
        {
            if (!tap.wallCheck && !enemy.onStun)
            {
                //Vector3 point = other.transform.position - enemy.transform.position;
                //point.y = 0;
                //enemy.transform.rotation = Quaternion.LookRotation(point);

                /*if (enemy.transform.position.x < enemy.target.position.x)
                {
                    enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    enemy.transform.rotation = Quaternion.Euler(0, -90, 0);
                }*/

                enemy.activeAttack = true;
            }

           
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player")  )
    //    {
    //        enemy.activeAttack = false;
    //    }
    //}
}

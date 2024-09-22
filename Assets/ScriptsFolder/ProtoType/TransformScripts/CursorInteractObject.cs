using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorInteractObject : MonoBehaviour
{
    public bool caught, thrown;

    public Collider cursorTargetCollider;
    SphereCollider sphere;

    /*private void Awake()
    {
        cursorTargetCollider = GetComponent<Collider>();
        if (cursorTargetCollider is SphereCollider)
        {
            sphere = GetComponent<SphereCollider>();
        }
    }

    public void ColliderEndPoint()
    {
        if (cursorTargetCollider is SphereCollider)
        {

            Vector3 center = sphere.bounds.center;
            float radius = sphere.radius;

            Vector3 upPoint = center + transform.up * radius;
            Vector3 forwardPoint = center + transform.forward * radius;
        }

    }*/

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("InteractiveObject")
            || collision.gameObject.CompareTag("Ground"))
        {
            Enemy enemy;
            if (TryGetComponent<Enemy>(out enemy))
            {
                enemy.Dead();
            }
        }
    }*/

    /*private void OnTriggerEnter(Collider other)
    {
        Enemy enemy;
        if (TryGetComponent<Enemy>(out enemy))
        {
            if (other.CompareTag("Enemy"))
            {
                DamagedByPAttack script;
                if (other.TryGetComponent<DamagedByPAttack>(out script))
                {
                    script.Damaged(1);
                }
            }
            enemy.Dead();
        }
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Dead();
        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("티비 몬스터 인식");
            collision.gameObject.GetComponent<Enemy>().Dead();
        }

    }*/
}

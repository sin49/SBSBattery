using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiateObject : MonoBehaviour
{
    public GameObject enemyPrefab;
    public bool checkPlayer;

    public Vector3 spawnPos;
    public Animator boxAnim;
    public GameObject deadEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !checkPlayer)
        {
            checkPlayer = true;
            if(boxAnim!=null)
            boxAnim.SetTrigger("Open");
        }
    }

    public void SpawnBoxEnemy()
    {
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        /*obj.transform.position = new(transform.position.x, transform.position.y, PlayerHandler.instance.CurrentPlayer.transform.localPosition.z);*/
    }
}

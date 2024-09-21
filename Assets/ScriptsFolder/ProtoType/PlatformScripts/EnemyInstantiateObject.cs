using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum createType { normal, parabola }

public class EnemyInstantiateObject : MonoBehaviour
{
    [Header("����� 0��:���� ���� ����")]
    public GameObject enemyPrefab;
    public bool checkPlayer;
     SoundEffectListPlayer soundEffectListPlayer;
    public Animator boxAnim;
    public GameObject deadEffect;
    [Header("���� Ȱ��ȭ �ݶ��̴� ����")]
    public BoxCollider activeCollider;
    public Vector3 activeRange;
    public Vector3 activePos;
    private void Awake()
    {
        soundEffectListPlayer = GetComponent<SoundEffectListPlayer>();
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !checkPlayer)
        {
            checkPlayer = true;
            if(boxAnim!=null)
            boxAnim.SetTrigger("Open");
        }
    }*/

    public void SpawnBoxEnemy()
    {
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation);
        //soundEffectListPlayer.PlayAudio(0);
    }

    private void OnDrawGizmos()
    {
        if (activeCollider != null)
        {
            activeCollider.size = activeRange;
            activeCollider.center = activePos;
        }
    }
}

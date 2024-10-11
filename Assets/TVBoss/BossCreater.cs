using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class BossCreater : MonoBehaviour
{





    //[Header("백그라운드 뮤직")]
    //public BackGroundAudioPlayer


    [Header("보스 프리팹 ")]
    public GameObject BossObject;

    [Header("보스 트랜스폼 ")]
    public Transform BossTransform;




    void CreateBoss()
    {
        var a = Instantiate(BossObject, BossTransform.position, BossTransform.rotation);
        ;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {


            CreateBoss();
            Destroy(this.gameObject);
        }
    }



}
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class BossCreater : MonoBehaviour
{





    //[Header("��׶��� ����")]
    //public BackGroundAudioPlayer


    [Header("���� ������ ")]
    public GameObject BossObject;

    [Header("���� Ʈ������ ")]
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
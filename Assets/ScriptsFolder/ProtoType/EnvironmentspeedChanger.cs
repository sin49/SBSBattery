using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentspeedChanger : MonoBehaviour
{
    [Header("이름 고치면 다 수정해야되서 그냥 컨베이어 그대로 끌고 옴")]
    public float conveyorSpeed = 5f; // 컨베이어 벨트의 속도
    public Vector3 conveyorDirection = Vector3.right; // 컨베이어 벨트의 방향
   
    protected virtual void changevector(environmentObject obj)
    {
        Vector3 force = conveyorDirection * conveyorSpeed;
        obj.AddEnviromentPower(force); // AddForce와 ForceMode를 사용
    }
    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy")||other.gameObject.layer==14)
        {
            environmentObject player = other.GetComponent<environmentObject>(); // Rigidbody로 수정
            if (player != null)
            {

                changevector(player);
            }
        }
       
    }
}

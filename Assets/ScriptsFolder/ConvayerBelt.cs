using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvayerBelt : MonoBehaviour
{
    //팬에서도 쓰이니 이름을 바꿀 것


    public float conveyorSpeed = 5f; // 컨베이어 벨트의 속도
    public Vector3 conveyorDirection = Vector3.right; // 컨베이어 벨트의 방향

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")|| other.CompareTag("Enemy"))
        {
            Player player = other.GetComponent<Player>(); // Rigidbody로 수정
            if (player != null)
            {

                Vector3 force = conveyorDirection * conveyorSpeed;
                player.AddEnviromentPower(force); // AddForce와 ForceMode를 사용
            }
        }
        else if (other.gameObject.layer==14)
        {
            Debug.Log("prob감지");
            physicsprob prob = other.GetComponent<physicsprob>();
            if (prob != null)
            {
                Vector3 force = conveyorDirection * conveyorSpeed;
                prob.getenvironmentforce(force);
            }
        }
    }
}

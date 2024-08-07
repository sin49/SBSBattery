using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvayerBelt : MonoBehaviour
{
    //�ҿ����� ���̴� �̸��� �ٲ� ��


    public float conveyorSpeed = 5f; // �����̾� ��Ʈ�� �ӵ�
    public Vector3 conveyorDirection = Vector3.right; // �����̾� ��Ʈ�� ����

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>(); // Rigidbody�� ����
            if (player != null)
            {
         
                Vector3 force = conveyorDirection * conveyorSpeed;
                player.AddEnviromentPower(force); // AddForce�� ForceMode�� ���
            }
        }
    }
}

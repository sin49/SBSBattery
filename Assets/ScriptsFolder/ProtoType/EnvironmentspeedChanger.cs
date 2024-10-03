using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentspeedChanger : MonoBehaviour
{
    [Header("�̸� ��ġ�� �� �����ؾߵǼ� �׳� �����̾� �״�� ���� ��")]
    public float conveyorSpeed = 5f; // �����̾� ��Ʈ�� �ӵ�
    public Vector3 conveyorDirection = Vector3.right; // �����̾� ��Ʈ�� ����
   
    protected virtual void changevector(environmentObject obj)
    {
        Vector3 force = conveyorDirection * conveyorSpeed;
        obj.AddEnviromentPower(force); // AddForce�� ForceMode�� ���
    }
    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy")||other.gameObject.layer==14)
        {
            environmentObject player = other.GetComponent<environmentObject>(); // Rigidbody�� ����
            if (player != null)
            {

                changevector(player);
            }
        }
       
    }
}

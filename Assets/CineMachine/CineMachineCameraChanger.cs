using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineMachineCameraChanger : MonoBehaviour
{
    [Header("���� �ϳ��� �����ص� ��")]

    [Header("ī�޶� �ε���")]
  public  int n;
    [Header("��ȯ ī�޶�(�켱 ���� ����)")]
    public CinemachineVirtualCamera virtualCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager m;
            if(PlayerHandler.instance.CurrentCamera.gameObject.TryGetComponent<CameraManager>(out m))
            {
                if (virtualCamera != null)
                    m.ActiveCamera(virtualCamera);
                else
                m.ActiveCamera(n);
            }
        }
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineMachineCameraChanger : MonoBehaviour
{
    [Header("둘중 하나만 설정해도 됨")]

    [Header("카메라 인덱스")]
  public  int n;
    [Header("전환 카메라(우선 순위 높음)")]
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

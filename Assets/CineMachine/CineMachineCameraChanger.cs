using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineMachineCameraChanger : MonoBehaviour
{
    [Header("캐릭터 이동을 바꿈")]
    public PlayerMoveState PlayerMoveState;
    [Header("둘중 하나만 설정해도 됨")]



    [Header("카메라 인덱스")]
  public  int n;
    [Header("전환 카메라(우선 순위 높음)")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("CameraMoveRange")]
    public Collider CameraRange;

    [Header("카메라 전환 속도")]
    public float transistionDuration=1.0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("COllider");
            CameraManager m;
            if(PlayerHandler.instance.CurrentCamera.gameObject.TryGetComponent<CameraManager>(out m))
            {
                m.transitionDuration = transistionDuration;
                if (virtualCamera != null)
                    m.ActiveCamera(virtualCamera, CameraRange);
                else
                m.ActiveCamera(n, CameraRange);
            }
            PlayerStat.instance.MoveState = PlayerMoveState;
        }
    }
}

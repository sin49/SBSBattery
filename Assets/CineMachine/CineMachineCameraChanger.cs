using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineMachineCameraChanger : MonoBehaviour
{
    [Header("ĳ���� �̵��� �ٲ�")]
    public PlayerMoveState PlayerMoveState;
    [Header("���� �ϳ��� �����ص� ��")]



    [Header("ī�޶� �ε���")]
  public  int n;
    [Header("��ȯ ī�޶�(�켱 ���� ����)")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("CameraMoveRange")]
    public Collider CameraRange;

    [Header("ī�޶� ��ȯ �ӵ�")]
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

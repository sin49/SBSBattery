using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface colliderDisplayer
{
    public void registerColliderDIsplay();
    public void ActiveColliderDisplay();//������Ʈ ���� �״�
    public void DeactiveColliderDisplay();
}
[ExecuteAlways]
public class CineMachineCameraChanger : MonoBehaviour, colliderDisplayer
{
    public Renderer ColliderDisplay;
    public  Renderer CameraRangeDisplay;
    private void Start()
    {
        registerColliderDIsplay();
    }


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
    protected virtual void OnTriggerEnter(Collider other)
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
            PlayerHandler.instance.CurrentPlayer.rotateBy3Dto2D();
        }
    }

    public void ActiveColliderDisplay()
    {
        ColliderDisplay.enabled=true;
        CameraRangeDisplay.enabled = true;
    }

    public void DeactiveColliderDisplay()
    {
        ColliderDisplay.enabled = false;
        CameraRangeDisplay.enabled = false;
    }

    public void registerColliderDIsplay()
    {
        if (ColliderDisplayManager.Instance != null)
        {
            ColliderDisplayManager.Instance.register(this);
        }
    }
}

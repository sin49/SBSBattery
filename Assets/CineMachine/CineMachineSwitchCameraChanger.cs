using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum camerachangerswitchingstate { none,change2D,change3D}
[ExecuteAlways]
public class CineMachineSwitchCameraChanger : MonoBehaviour,colliderDisplayer
{

 
    [Header("ĳ���� �̵��� �ٲ�")]
    public PlayerMoveState PlayerMoveState;


    [Header("ī�޶�� �����ϸ� ������ camera2D,camera3D�� �ٲ�")]
    [Header("ī�޶� ������� ��")]
    [Header("2D��ȯ ī�޶�")]
    public CinemachineVirtualCamera virtualCamera2D;
    [Header("3D��ȯ ī�޶�")]
    public CinemachineVirtualCamera virtualCamera3D;
    [Header("�ٲ� ī�޶�")]
    [Header("ī�޶� �����ϸ� ī�޶� �ٲٴ°� ����")]
    [Header("�� ����� ���� ���̴� ī�޶� ���� ��ü�Ұǰ� �׷� ���")]
    [Header("none=�ȹٲٰ� ī�޶� �ٲ�,change2D=2Dī�޶� ��ȯ,change3D=3Dī�޶� ��ȯ")]
   public camerachangerswitchingstate switchingstate;

    [Header("�̰͵� ������� ��")]
    [Header("2DCameraMoveRange")]
    public Collider CameraRange2D;
    [Header("3DCameraMoveRange")]
    public Collider CameraRange3D;
    [Header("ī�޶� ��ȯ �ӵ�")]
    public float transistionDuration = 1.0f;

    public Renderer ColliderDisplay;
    public Renderer CameraRangeDisplay2D;
    public Renderer CameraRangeDisplay3D;
    private void Start()
    {
        registerColliderDIsplay();
    }
    public void ActiveColliderDisplay()
    {
        ColliderDisplay.enabled = true;
        CameraRangeDisplay2D.enabled=true;
        CameraRangeDisplay3D.enabled = true;
    }

    public void DeactiveColliderDisplay()
    {
        ColliderDisplay.enabled = false;
        CameraRangeDisplay2D.enabled = false;
        CameraRangeDisplay3D.enabled = false;
    }

    public void registerColliderDIsplay()
    {
        if (ColliderDisplayManager.Instance != null)
        {
            ColliderDisplayManager.Instance.register(this);
        }
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("COllider");
            CameraManager_Switching2D3D m;
            if (PlayerHandler.instance.CurrentCamera.gameObject.TryGetComponent<CameraManager_Switching2D3D>(out m))
            {
                m.transitionDuration = transistionDuration;
                if (virtualCamera2D != null)
                    m.camera2D = virtualCamera2D;
                if (virtualCamera2D != null)
                    m.camera2D = virtualCamera3D;

                if (CameraRange2D != null)
                    m.Set2DCamerabinding(CameraRange2D);
                if (CameraRange3D != null)
                    m.Set3DCamerabinding(CameraRange3D);

                switch (switchingstate)
                {
                    case camerachangerswitchingstate.change2D:
                        m.ActiveCamera(m.camera2D);
                        break;
                    case camerachangerswitchingstate.change3D:
                        m.ActiveCamera(m.camera3D);
                        break;
                }
            }
            if(PlayerMoveState!=PlayerMoveState.none)
            PlayerStat.instance.MoveState = PlayerMoveState;
            PlayerHandler.instance.CurrentPlayer.rotateBy3Dto2D();
        }
    }

}

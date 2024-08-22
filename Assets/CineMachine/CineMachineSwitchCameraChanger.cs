using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum camerachangerswitchingstate { none,change2D,change3D}
[ExecuteAlways]
public class CineMachineSwitchCameraChanger : MonoBehaviour,colliderDisplayer
{

 
    [Header("캐릭터 이동을 바꿈")]
    public PlayerMoveState PlayerMoveState;


    [Header("카메라는 설정하면 닿으면 camera2D,camera3D를 바꿈")]
    [Header("카메라 비워놔도 됌")]
    [Header("2D전환 카메라")]
    public CinemachineVirtualCamera virtualCamera2D;
    [Header("3D전환 카메라")]
    public CinemachineVirtualCamera virtualCamera3D;
    [Header("바꿀 카메라")]
    [Header("카메라 설정하면 카메라 바꾸는건 같음")]
    [Header("이 기능은 현재 보이는 카메라를 뭐로 교체할건가 그런 기능")]
    [Header("none=안바꾸고 카메라만 바꿈,change2D=2D카메라 전환,change3D=3D카메라 전환")]
   public camerachangerswitchingstate switchingstate;

    [Header("이것도 비워놔도 됌")]
    [Header("2DCameraMoveRange")]
    public Collider CameraRange2D;
    [Header("3DCameraMoveRange")]
    public Collider CameraRange3D;
    [Header("카메라 전환 속도")]
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

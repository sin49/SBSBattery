using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class BossCreater : MonoBehaviour,colliderDisplayer
{

    public Renderer ColliderDisplay;
    [Header("보스 카메라 2D")]
    public CinemachineVirtualCamera camera2D;
    [Header("보스 카메라 3D")]
    public CinemachineVirtualCamera camera3D;

    [Header("보스 필드")]
    public Transform bossfield;

    [Header("CameraMoveRange")]
    public Collider CameraRange;

    [Header("카메라 전환 속도")]
    public float transistionDuration = 1.0f;

    [Header("캐릭터 이동을 바꿈")]
    public PlayerMoveState PlayerMoveState;

    [Header("보스 프리팹 ")]
    public GameObject BossObject;

    [Header("보스 트랜스폼 ")]
    public Transform BossTransform;

    [Header("보스가 나타났다 UI ")]
    public GameObject BossUI;

  
 
    void CreateBoss()
    {
     var a=   Instantiate(BossObject, BossTransform.position, BossTransform.rotation);
        a.GetComponent<BossFalling>().bossField = bossfield;
        a.GetComponent<Boss1Sweap>().BossField = bossfield;
        if(BossUI != null) 
        BossUI.SetActive(true);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager_Switching2D3D m;
            if (PlayerHandler.instance.CurrentCamera.gameObject.TryGetComponent<CameraManager_Switching2D3D>(out m))
            {
           
                m.transitionDuration = transistionDuration;

                m.settingBoss1ccamera(camera2D, camera3D, CameraRange, PlayerMoveState);
            }
            PlayerStat.instance.MoveState = PlayerMoveState;
            PlayerHandler.instance.CurrentPlayer.rotateBy3Dto2D();
            CreateBoss();
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        registerColliderDIsplay();
    }

    public void ActiveColliderDisplay()
    {
        ColliderDisplay.enabled = true;
    }
    public void registerColliderDIsplay()
    {
        if (ColliderDisplayManager.Instance != null)
        {
            ColliderDisplayManager.Instance.register(this);
        }
    }
    public void DeactiveColliderDisplay()
    {
      ColliderDisplay.enabled = false;
    }
}

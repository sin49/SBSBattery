using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class BossCreater : MonoBehaviour,colliderDisplayer
{

    public GameObject ColliderDisplay;
    [Header("���� ī�޶� 2D")]
    public CinemachineVirtualCamera camera2D;
    [Header("���� ī�޶� 3D")]
    public CinemachineVirtualCamera camera3D;

    [Header("���� �ʵ�")]
    public Transform bossfield;

    [Header("CameraMoveRange")]
    public Collider CameraRange;

    [Header("ī�޶� ��ȯ �ӵ�")]
    public float transistionDuration = 1.0f;

    [Header("ĳ���� �̵��� �ٲ�")]
    public PlayerMoveState PlayerMoveState;

    [Header("���� ������ ")]
    public GameObject BossObject;

    [Header("���� Ʈ������ ")]
    public Transform BossTransform;

    [Header("������ ��Ÿ���� UI ")]
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
        ColliderDisplay.SetActive(true);
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
      ColliderDisplay.SetActive(false);
    }
}

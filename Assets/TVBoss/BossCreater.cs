using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class BossCreater : MonoBehaviour,colliderDisplayer
{

    public Renderer ColliderDisplay;
    [Header("���� ī�޶� 2D")]
    public CinemachineVirtualCamera camera2D;
    [Header("���� ī�޶� 3D")]
    public CinemachineVirtualCamera camera3D;

    [Header("���� �ʵ�")]
    public Transform bossfield;




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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class ObjectActiveColliderBox : MonoBehaviour,colliderDisplayer
{
    [Header("��ȸ����")]
    [Header("Ȱ��ȭ")]
    public GameObject ActiveObj;
    [Header("��Ȱ��ȭ")]
    public GameObject DeactiveObj;

    MeshRenderer renderer_;

    public void ActiveColliderDisplay()
    {
        renderer_.enabled = true;
    }

    public void DeactiveColliderDisplay()
    {
        renderer_.enabled = false;
    }

    public void registerColliderDIsplay()
    {
        if(ColliderDisplayManager.Instance !=null)
        ColliderDisplayManager.Instance.register(this);
    }

    private void Awake()
    {
        renderer_ = GetComponent<MeshRenderer>();
       
    }
    private void Start()
    {
        registerColliderDIsplay();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(ActiveObj!=null)
            ActiveObj.SetActive(true);
            if(DeactiveObj!=null)
            DeactiveObj.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}

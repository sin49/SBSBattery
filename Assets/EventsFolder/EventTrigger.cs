using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour,colliderDisplayer
{
    [Header("�����ϴ� �̺�Ʈ")]
    public List<EventHandler> starthandlers = new List<EventHandler>();
    [Header("������ �̺�Ʈ")]
    public List<EventHandler> stophandlers = new List<EventHandler>();
  
    public bool actived;
    public MeshRenderer _meshrenderer;
    public void ActiveColliderDisplay()
    {
        _meshrenderer.enabled = false;
    }

    public void DeactiveColliderDisplay()
    {
        _meshrenderer.enabled = true;
    }

    public void registerColliderDIsplay()
    {
        if (ColliderDisplayManager.Instance != null)
        {
            ColliderDisplayManager.Instance.register(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var a in starthandlers)
            {
                a.startevent();
            }
            foreach (var a in stophandlers)
            {
                a.stopevent();
            }
            actived = true;
        }
    }
}

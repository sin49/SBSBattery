using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbDimensionChangeField : MonoBehaviour, colliderDisplayer
{
    [Header("��ȯ ���� �ɼ�")]
    public bool RestirctDimension;


    public Renderer renderer_;
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
        ColliderDisplayManager.Instance.register(this);
    }
    private void Awake()
    {
        registerColliderDIsplay();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if(RestirctDimension)
            PlayerHandler.instance.DImensionChangeDisturb = true;
            else
                PlayerHandler.instance.DImensionChangeDisturb = false;
        }
    }
   
}

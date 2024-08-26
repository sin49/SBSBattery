using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbDimensionChangeField : MonoBehaviour, colliderDisplayer
{
    [Header("전환 금지 옵션")]
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
    private void Start()
    {
        registerColliderDIsplay();
    }

       

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            PlayerHandler.instance.DImensionChangeDisturb = RestirctDimension;

        }
    }
   
}

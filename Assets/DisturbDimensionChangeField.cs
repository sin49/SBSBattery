using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class DisturbDimensionChangeField : MonoBehaviour, colliderDisplayer
{
    [Header("전환 금지 옵션")]
    public bool RestirctDimension;


    public Renderer renderer_;
    public void ActiveColliderDisplay()
    {
        if(renderer_!=null) 
        renderer_.enabled = true;
    }

    public void DeactiveColliderDisplay()
    {
        if (renderer_ != null)
            renderer_.enabled = false;
    }

    public void registerColliderDIsplay()
    {
        if (renderer_ != null)
            ColliderDisplayManager.Instance.register(this);
    }
    private void Start()
    {
        if (renderer_ != null)
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

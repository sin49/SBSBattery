using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class DisturbDimensionChangeField : MonoBehaviour, colliderDisplayer
{
   


    public Renderer renderer_;
    private void Awake()
    {
        renderer_=GetComponent<Renderer>();
    }
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


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerHandler.instance.DImensionChangeDisturb = false;

        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            PlayerHandler.instance.DImensionChangeDisturb = true;

        }
    }
   
}

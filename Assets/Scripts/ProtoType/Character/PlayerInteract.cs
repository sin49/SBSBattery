using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerInteract : MonoBehaviour
{
    Player p;


  
    void InteractrayCast()
    {

        RaycastHit hit;
        Debug.DrawRay(transform.position * (int)PlayerStat.instance.direction, Vector3.right * 0.15f * (int)PlayerStat.instance.direction, Color.red);
        if (Physics.Raycast(this.transform.position * (int)PlayerStat.instance.direction, Vector3.right * (int)PlayerStat.instance.direction, out hit, 0.15f))
        {
   
            if (hit.collider.CompareTag("InteractiveObject"))
            {
                InteractiveObject interactobject;
                if (!hit.collider.TryGetComponent<InteractiveObject>(out interactobject))
                {
                   
                    Debug.Log("Fatal Error? Can't Find Script instance");
                }
                else
                {
                    PlayerHandler.instance.GetInteratObject(interactobject);
                    if (interactobject.InteractOption != InteractOption.ray)
                        PlayerHandler.instance.GetInteratObject(interactobject);
                }
            }




        }

    }
    private void FixedUpdate()
    {
        if (p != null)
            InteractrayCast();

    }
    private void Awake()
    {
        p= GetComponent<Player>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractiveObject"))
        {
            InteractiveObject obj;
            if (!other.TryGetComponent<InteractiveObject>(out obj))
            {
                
                Debug.Log("Fatal Error? Can't Find Script instance");
            }
            else
            {

                if (obj == PlayerHandler.instance.ReturnInteractObject())
                    PlayerHandler.instance.InitInteratObject();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("InteractiveObject"))
        {
            InteractiveObject i;
            if (!other.TryGetComponent<InteractiveObject>(out i))
            {
                
                Debug.Log("Fatal Error? Can't Find Script instance");
            }
            else
            {
                PlayerHandler.instance.GetInteratObject(i);
                if (i.InteractOption != InteractOption.collider)
                    PlayerHandler.instance.GetInteratObject(i);
            }
        }
    }

}

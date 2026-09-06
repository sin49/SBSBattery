using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformObject : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if ( other.CompareTag("Player") )
        {

            if (PlayerHandler.instance.CurrentType != TransformType.Default)
            {
                PlayerHandler.instance.Deform();
                PlayerHandler.instance.OnDeformField = true;
            }
        }
      
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

           
            PlayerHandler.instance.OnDeformField = false;

        }
    }
}

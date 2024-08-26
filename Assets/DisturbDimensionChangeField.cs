using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbDimensionChangeField : MonoBehaviour

{
    [Header("전환금지")]
    public bool restrictdimension;
    private void OnTriggerEnter(Collider other)
    {
       
                PlayerHandler.instance.DImensionChangeDisturb = restrictdimension;
           
    }
  
}

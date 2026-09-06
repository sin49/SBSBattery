using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectRemoveEvent :  OutputEvent
{
    public GameObject obj;
  
    
    

    public override void output()
    {
        if(obj!=null)
      Destroy(obj);
        base.output();
    }

  
}

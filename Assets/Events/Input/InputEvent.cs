using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public interface InputEvent 
{
    public bool input(object o=null);
}

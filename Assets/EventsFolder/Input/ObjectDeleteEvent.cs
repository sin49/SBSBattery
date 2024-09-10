using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ObjectDeleteEvent :  InputEvent
{
    public List<GameObject> gameobjects = new List<GameObject>();

    public override void initialize()
    {
        return;
    }

    public override bool input(object o = null)
    {
      for(int n = 0; n < gameobjects.Count; n++)
        {
            if (gameobjects[n] == null)
            {
                gameobjects.RemoveAt(n);
                n--;
            }
            else
                return false;
        }
        return true;
    }
}

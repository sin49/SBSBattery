using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDeleteEvent : MonoBehaviour,InputEvent
{
    public List<GameObject> gameobjects = new List<GameObject>();

    public bool input(object o = null)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDeactiveInputEvent : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();
    public bool input(object o = null)
    {
        for (int n = 0; n < gameObjects.Count; n++)
        {
            if (!gameObjects[n].activeSelf)
            {
                continue;
            }
            else
                return false;
        }
        return true;
    }
}

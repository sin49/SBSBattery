using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ObjectActiveInputEvent :InputEvent
{
    public List<GameObject> gameObjects = new List<GameObject>();

    public override void initialize()
    {
        return;
    }

    public override bool input(object o = null)
    {
        for (int n = 0; n < gameObjects.Count; n++)
        {
            if (gameObjects[n].activeSelf)
            {
                continue;
            }
            else
                return false;
        }
        return true;
    }


   
}

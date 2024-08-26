using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActiveEvent : OutputEvent
{
    public List<GameObject> gameobjects=new List<GameObject>();
    public override void output()
    {
        foreach (var item1 in gameobjects)
        {
            if(!item1.activeSelf)
                item1.SetActive(true);
        }
        base.output();
    }
}

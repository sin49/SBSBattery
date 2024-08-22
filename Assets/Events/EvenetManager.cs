using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenetManager : MonoBehaviour
{
    
    List<EventHandler> eventhadles=new List<EventHandler>();
    string handlername;
    public void addeventhandler()
    {
      var a=  Instantiate(gameObject, this.transform);
       var handler= a.AddComponent<EventHandler>();
        handler.name = handlername;
        eventhadles.Add(handler);
    }
    public void deleteeventhandler(int index)
    {
        eventhadles.RemoveAt(index);
    }
}

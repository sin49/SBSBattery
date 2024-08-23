using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenetManager : MonoBehaviour
{
    [HideInInspector]
  public  bool showHandlersInScene;

  public  EventTrigger Etrigger;
    public Transform triggertransform;
    public Transform handlertransform;

  public  List<EventHandler> eventhadles=new List<EventHandler>();
    public List<EventTrigger> eventTriggers = new List<EventTrigger>();
   public string handlername;
    public void addeventTrigger()
    {

        var b = Instantiate(Etrigger.gameObject, triggertransform);
        var trigger = b.GetComponent<EventTrigger>();
        eventTriggers.Add(trigger);
        trigger.name = "EventTrigger"+eventTriggers.Count;
       
    }

    public void addeventhandler()
    {
      var a=  Instantiate(new GameObject(), handlertransform);
       var handler= a.AddComponent<EventHandler>();
        if(handlername!=null)
        handler.name = handlername;
        else
            handler.name ="eventhandler"+ eventhadles.Count;
        eventhadles.Add(handler);
    }
    public void deleteeventhandler(int index)
    {
        eventhadles.RemoveAt(index);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXChangePortal : InteractiveObject
{
    //float ZchangeVaule;
    public Transform Destination;

    public bool HasLoadingEffect;
  
    public void MovePosition(string s=null)
    {

            Debug.Log("ししししし");
            PlayerHandler.instance.CurrentPlayer.transform.position = Destination.position;
      

    }
   
    public override void Active(direction direct)
    {
        base.Active(direct);
        if (!HasLoadingEffect)
            MovePosition();
        else
            GameManager.instance.LoadingEffectToAction(MovePosition);
        //PlayerCam.instance.PlayerZVaule += ZchangeVaule;
        //if(PlayerCam.instance.PlayerZVaule!=0)
        //     PlayerCam.instance.ZPin= isZpin;
    }
}

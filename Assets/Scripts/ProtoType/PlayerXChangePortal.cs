using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXChangePortal : InteractiveObject
{
    float ZchangeVaule;
    public PlayerXChangePortal Destination;
    public Transform PlayerTeleportPosition;
    public bool isZpin=true;
    private void Awake()
    {
        if(Destination != null)
        ZchangeVaule=this.transform.position.z- Destination.transform.position.z;

        InteractOption = InteractOption.collider;
    }
    
   
    public override void Active(direction direct)
    {
        PlayerHandler.instance.CurrentPlayer.transform.position = Destination.PlayerTeleportPosition.position;
        //PlayerCam.instance.PlayerZVaule += ZchangeVaule;
        //if(PlayerCam.instance.PlayerZVaule!=0)
        //     PlayerCam.instance.ZPin= isZpin;
    }
}

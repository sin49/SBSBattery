using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fan : RemoteObject
{
    public ConvayerBelt Air;
    public float AirPower;
    private void Start()
    {
        Air.conveyorDirection = this.transform.forward;
        Air.conveyorSpeed = AirPower;
        if (onActive)
        {
            Air.gameObject.SetActive(true);
        }
        else
            Air.gameObject.SetActive(false);
    }
    private void Update()
    {
        Air.conveyorSpeed = AirPower;
    }
    public override void Active()
    {
        if (onActive)
        {
            Deactive();
            return;
        }
        onActive = true;
        Air.gameObject.SetActive(true);
    }

    public override void Deactive()
    {
        onActive = false;
        Air.gameObject.SetActive(false);
    }

    
}

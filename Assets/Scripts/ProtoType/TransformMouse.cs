using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMouse: TransformPlace
{
    public ShootingFIeld ShootingGame;
    private void OnEnable()
    {
       if( ShootingGame.active)
            TransformPlaceEffect.gameObject.SetActive(false);
    }
    void activeshooting()
    {
      
        ShootingGame.gameObject.SetActive(true);
    }
    public override void transformStart(Collider other)
    {
        if (PlayerHandler.instance.CurrentType == TransformType.Default&&!ShootingGame.active)
        {
            other.transform.position = this.transform.position;
            gameObject.SetActive(false);
            PlayerHandler.instance.LastTransformPlace = this;
            other.GetComponent<Player>().FormChange(type,activeshooting);
        }
    }
}

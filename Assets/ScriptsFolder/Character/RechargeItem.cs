using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargeItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;
        Destroy(gameObject);
    }
}

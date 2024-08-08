using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbDimensionChangeField : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!PlayerHandler.instance.DImensionChangeDisturb)
        {
            PlayerHandler.instance.DImensionChangeDisturb = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerHandler.instance.DImensionChangeDisturb)
        {
            PlayerHandler.instance.DImensionChangeDisturb = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbDimensionChangeField : MonoBehaviour
{
    [Header("��ȯ����")]
    public bool restrictdimension;
    private void OnTriggerEnter(Collider other)
    {
        if (restrictdimension)
            if (other.CompareTag("Player") && !PlayerHandler.instance.DImensionChangeDisturb)
            {
                PlayerHandler.instance.DImensionChangeDisturb = true;
            }

            else
            {
                if (other.CompareTag("Player") && !PlayerHandler.instance.DImensionChangeDisturb)
                {
                    PlayerHandler.instance.DImensionChangeDisturb = false;
                }
            }
    }
  
}

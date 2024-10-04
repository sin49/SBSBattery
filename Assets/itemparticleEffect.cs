using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class itemparticleEffect : MonoBehaviour
{
  
   
    void Update()
    {
        transform.position = PlayerHandler.instance.CurrentPlayer.transform.position;
    }
}

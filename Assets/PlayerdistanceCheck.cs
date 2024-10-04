using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerdistanceCheck : MonoBehaviour
{
   
    void Update()
    {
        this.transform.position = PlayerHandler.instance.CurrentPlayer.transform.position;
    }
    
}

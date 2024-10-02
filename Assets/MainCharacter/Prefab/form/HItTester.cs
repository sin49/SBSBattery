using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HItTester : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha6))
        {
            PlayerHandler.instance.CurrentPlayer.Damaged(1);
        }
    }
}

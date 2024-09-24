using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stair : MonoBehaviour
{
    public float upgradeDownForce = 60;
    public float upgradeMoveforce = 4;


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHandler.instance.CurrentPlayer.onstair = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHandler.instance.CurrentPlayer.onstair = false;
        }
    }
}

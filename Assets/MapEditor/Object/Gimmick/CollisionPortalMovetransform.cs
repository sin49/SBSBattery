using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPortalMovetransform :MonoBehaviour
{
    public Transform MoveTransform;
    public void MoveEvent(string none = null)
    {
        PlayerHandler.instance.CurrentPlayer.transform.position = MoveTransform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //GameManager.instance.LoadingScene(SceneName);
            PlayerHandler.instance.CantHandle = true;
            GameManager.instance.LoadingEffectToAction(MoveEvent);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //GameManager.instance.LoadingScene(SceneName);
            PlayerHandler.instance.CantHandle = false;
            GameManager.instance.LoadingEffectDeActive();
        }

    }
}

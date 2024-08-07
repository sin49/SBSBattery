using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIManager : MonoBehaviour
{
    public GameObject RemoteTargetUI;
    public GameObject InteractTargetUI;
    private void Awake()
    {
        RemoteTargetUI.SetActive(false);
        InteractTargetUI.SetActive(false);
    }
    public void UpdateRemoteTargetUI(GameObject target)
    {
        if (target != null)
        {
            RemoteTargetUI.SetActive(true);
            RemoteTargetUI.transform.position = PlayerHandler.instance.CurrentCamera.WorldToScreenPoint(
       target.transform.position
                );

        }
        else
        {
            RemoteTargetUI.SetActive(false);
        }
    }
    public void UpdateInteractUI(GameObject target)
    {

            InteractTargetUI.SetActive(true);
            InteractTargetUI.transform.position = PlayerHandler.instance.CurrentCamera.WorldToScreenPoint(
                    target.transform.GetChild(0).position
                );

     
       
    }

}

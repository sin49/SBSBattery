
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontMoveCollider : MonoBehaviour
{



 
    private void OnTriggerStay(Collider other)
    {        
        if ((other.CompareTag("Ground") && !other.GetComponent<TransformPlace>() )|| other.CompareTag("Enemy") ||
         (   other.CompareTag("InteractivePlatform") && !PlayerHandler.instance.CurrentPlayer.CullingPlatform))
        {
            if ( PlayerHandler.instance.CurrentPlayer != null)
                 PlayerHandler.instance.CurrentPlayer.SetWallcheck(true);
        }        

        //if (other.CompareTag("InteractiveObject"))
        //{
        //    if (other.GetComponent<InteractiveObject>().InteractOption != InteractOption.collider&& PlayerHandler.instance.CurrentPlayer!=null)
        //    {
        //        if (PlayerHandler.instance.CurrentPlayer != null)
        //            PlayerHandler.instance.CurrentPlayer.SetWallcheck(true);
        //    }
        //}


    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Ground") && !other.GetComponent<TransformPlace>() )|| other.CompareTag("Enemy") ||
            (other.CompareTag("InteractivePlatform") && !PlayerHandler.instance.CurrentPlayer.CullingPlatform))
        {
            if (PlayerHandler.instance.CurrentPlayer != null)
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
        }

        //if (other.CompareTag("InteractiveObject"))
        //{
        //    if (other.GetComponent<InteractiveObject>().InteractOption != InteractOption.collider )
        //    {
        //        if (PlayerHandler.instance.CurrentPlayer != null)
        //            PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
        //    }
        //}


    }

    public void OtherCheck(GameObject obj)
    {
        PlayerHandler.instance.CurrentPlayer.wallcheck = false;       
    }
}
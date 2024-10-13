
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontMoveCollider : MonoBehaviour
{

    BoxCollider boxCollider;
    Vector3 originCenter = Vector3.zero;
    Vector3 originSize = new(1, 1, 1);

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider other)
    {        
        if ((other.CompareTag("Ground")  )|| other.CompareTag("Enemy") ||
         (   other.CompareTag("InteractivePlatform") && !PlayerHandler.instance.CurrentPlayer.CullingPlatform) || other.CompareTag("CursorObject"))
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
        if ((other.CompareTag("Ground")  )|| other.CompareTag("Enemy") ||
            (other.CompareTag("InteractivePlatform") && !PlayerHandler.instance.CurrentPlayer.CullingPlatform) || other.CompareTag("CursorObject"))
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

    public void ChangeScaleByFormCursor(float scaleValue)
    {
        boxCollider.center = new(0, 0, scaleValue/2);
        boxCollider.size = new(1, 1, scaleValue);
    }

    public void ReturnScale()
    {
        boxCollider.center = originCenter;
        boxCollider.size = originSize;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deactivein2D : MonoBehaviour
{
    public GameObject obj;
    public bool camerachangeafter;
    // Start is called before the first frame update
    void Start()
    {
        if (camerachangeafter)
        {
            PlayerHandler.instance.registerCameraChangeAfterEvent(() => { activeevent(); });
        }
        else
        {
            PlayerHandler.instance.registerCameraChangeAction(() => { activeevent(); });
        }
    }
    void activeevent()
    {
        if ((int)PlayerStat.instance.MoveState < 4)
            obj.SetActive(false);
        else
            obj.SetActive(true);
    }

   
}

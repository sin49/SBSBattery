using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deactivein2D : MonoBehaviour
{
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((int)PlayerStat.instance.MoveState < 4)
            obj.SetActive(false);
        else
            obj.SetActive(true);
    }
}

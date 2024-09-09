using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSetting : MonoBehaviour
{
    public List<Enemy> enemylist = new List<Enemy>();

    void Update()
    {
        /*bool b = false;
        foreach(Enemy e in enemylist)
        {
            if(e.gameObject.activeSelf)
            {
                b ^= true;
                return;
            }
           
        }
        this.gameObject.SetActive(b);*/
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class racingTerrain : MonoBehaviour
{
    public float speed;
   

    void Update()
    {
        if (!RacingManager.instance.Onhit)
             transform.Translate(Vector3.back * speed * Time.deltaTime);
        if (this.transform.position.z < -50)
        {
            this.gameObject.SetActive(false);
        }
    }
}

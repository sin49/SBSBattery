using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeAcrtiveOnTimer : MonoBehaviour
{
    public float time;
    float timer;
    private void Awake()
    {
        timer = time;
    }
    private void OnDisable()
    {
        timer = time;
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
    
}

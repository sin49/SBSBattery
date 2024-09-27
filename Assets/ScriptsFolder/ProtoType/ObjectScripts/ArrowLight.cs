using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLight : MonoBehaviour
{
    public GameObject onSign;
    public GameObject offSign;

    public float blinkTime;
    float blinkTimer;
    bool active;

    private void Awake()
    {
        onSign.SetActive(false);
        offSign.SetActive(true);
        blinkTimer = blinkTime;
    }

    private void Update()
    {
        if (blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
        }
        else
        {
            blinkTimer = blinkTime;
            if (!active)
            {
                active = true;
                ArrowOn();
            }
            else
            {
                active = false;
                ArrowOff();
            }

        }

    }

    public void ArrowOn()
    {
        onSign.SetActive(true);
        offSign.SetActive(false);
    }

    public void ArrowOff()
    {
        onSign.SetActive(false);
        offSign.SetActive(true);
    }
}

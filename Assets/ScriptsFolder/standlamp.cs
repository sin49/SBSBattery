using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class standlamp : MonoBehaviour
{
    [Header("블링크 효과")]
    public bool blink;
    [Header("블링크 효과 관련")]
    [Header("빛이 변하는 시간")]
    public float blinktime=1;
    float blinkspeed;
    [Header("꺼져있는 시간")]
    public float lightofftime=1;
    [Header("켜져있는 시간")]
    public float lightontime=1;
    public Light BulbLight;
    public Light Light;
    bool lightblinkstate;
 float initlightintensity;
    
    IEnumerator LightBlink()
    {
        while (true)
        {
            if (lightblinkstate)
            {
                Light.intensity -= blinkspeed * Time.fixedDeltaTime;
                BulbLight.intensity = 0.4f;
                if (Light.intensity <= 0)
                {
                    lightblinkstate = false;
                    Light.intensity = 0;
                    BulbLight.intensity = 0;
                    yield return new WaitForSeconds(lightofftime);
                }
            }
            else
            {
                Light.intensity += blinkspeed * Time.fixedDeltaTime;
                BulbLight.intensity = 0.65f;
                if (Light.intensity >= initlightintensity)
                {
                    lightblinkstate = true;
                    Light.intensity = initlightintensity;
                    BulbLight.intensity = 1;
                    yield return new WaitForSeconds(lightontime);
                }
            }
            yield return null;
        }
    }
    
    private void Awake()
    {
        initlightintensity = Light.intensity;
    }
    private void Update()
    {
        blinkspeed = initlightintensity / blinktime;
    }
    private void OnBecameInvisible()
    {
        StopAllCoroutines();
    }
    private void OnBecameVisible()
    {
        StartCoroutine(LightBlink());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class renderpassmanager : MonoBehaviour
{
    public UniversalRendererData rendererdata;
    public Material pixelmaterial;
    public float maxPixelate;
    public float minpixelate;
    public float time;
    float speed;
    bool trans3D;
    ScriptableRendererFeature fullscreenrenderfeauture;
    void Start()
    {
        foreach(var a in rendererdata.rendererFeatures)
        {
            if(a is FullScreenPassRendererFeature renderfeautre&&a.name== "FullScreenPassRendererFeature")
            {
                pixelmaterial = renderfeautre.passMaterial;
                fullscreenrenderfeauture = a;
            }
        }

        fullscreenrenderfeauture.SetActive(false);

    }
    public void changepixel()
    {
        fullscreenrenderfeauture.SetActive(true);

        //time = PlayerHandler.instance.CurrentCamera.GetComponent<CameraManager_Switching2D3D>().transitionDuration;
        speed = (maxPixelate - minpixelate)/time;
        if (trans3D)
        {
         
            pixelmaterial.SetFloat("_pixelRate", minpixelate);
            StartCoroutine(changepixelCorutine(speed, minpixelate, maxPixelate));
        }
        else
        {
            
            pixelmaterial.SetFloat("_pixelRate", maxPixelate);
            StartCoroutine(changepixelCorutine(-speed, maxPixelate, minpixelate));
        }
    }
    IEnumerator changepixelCorutine(float speed,float initpixel,float destinationpixel)
    {
        float timer = 0;
        float tmp=initpixel;
        while (timer < time)
        {
            tmp += Time.unscaledDeltaTime*speed;
            pixelmaterial.SetFloat("_pixelRate", tmp);
            timer += Time.unscaledDeltaTime;
    
            yield return null;
        }
        pixelmaterial.SetFloat("_pixelRate", destinationpixel);

        if (trans3D)
            fullscreenrenderfeauture.SetActive(false);
        trans3D = !trans3D;
        
    }
        // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            changepixel();
        }
    }
}

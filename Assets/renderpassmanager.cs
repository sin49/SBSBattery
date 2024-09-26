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
    public float MiniMumPixelate;
    public float pixelate2D;
    public float time;
    float speed;

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
        if(fullscreenrenderfeauture!=null&&(int)PlayerStat.instance.MoveState>3)
        fullscreenrenderfeauture.SetActive(false);
        else
            fullscreenrenderfeauture.SetActive(true);

    }
    public void changepixel(bool trans3D)
    {
        if (fullscreenrenderfeauture == null)
            return;
        fullscreenrenderfeauture.SetActive(true);

       
        if (trans3D)
        {
         
            pixelmaterial.SetFloat("_pixelRate", pixelate2D);
            StartCoroutine(changepixelCorutine3D());
        }
        else
        {
         
            pixelmaterial.SetFloat("_pixelRate", maxPixelate);
            StartCoroutine(changepixelCorutine2D());
        }
    }
    IEnumerator changepixelCorutine2D()
    {
        time = PlayerHandler.instance.CurrentCamera.GetComponent<CameraManager_Switching2D3D>().transitionDuration;
        time /= 5;
        speed = (maxPixelate - MiniMumPixelate) / time;
    
        float timer = 0;
        float tmp = maxPixelate;
        while (timer < time)
        {
            tmp += Time.unscaledDeltaTime * speed*-1;
            pixelmaterial.SetFloat("_pixelRate", tmp);
            timer += Time.unscaledDeltaTime;

            yield return new WaitForSecondsRealtime(0);
        }
        pixelmaterial.SetFloat("_pixelRate", MiniMumPixelate);
        yield return new WaitForSecondsRealtime(time * 4
            );
        speed = (pixelate2D - MiniMumPixelate) / time;

        while (timer < time)
        {
            tmp += Time.unscaledDeltaTime * speed;
            pixelmaterial.SetFloat("_pixelRate", tmp);
            timer += Time.unscaledDeltaTime;

            yield return new WaitForSecondsRealtime(0);
        }
        pixelmaterial.SetFloat("_pixelRate", pixelate2D);



    }
    IEnumerator changepixelCorutine3D()
    {
        time = PlayerHandler.instance.CurrentCamera.GetComponent<CameraManager_Switching2D3D>().transitionDuration;

        speed = (maxPixelate - pixelate2D) / time;
        float timer = 0;
        float tmp= pixelate2D;
        while (timer < time)
        {
            tmp += Time.unscaledDeltaTime*speed;
            pixelmaterial.SetFloat("_pixelRate", tmp);
            timer += Time.unscaledDeltaTime;

            yield return new WaitForSecondsRealtime(0);
        }
        pixelmaterial.SetFloat("_pixelRate", maxPixelate);

        
            fullscreenrenderfeauture.SetActive(false);
 
        
    }
        // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha4))
    //    {
    //        changepixel();
    //    }
    //}
}

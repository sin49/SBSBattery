using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObj : MonoBehaviour
{
    private MeshRenderer r;
    private Material transparent;
    public float alpha_ = 0.1f;
    public float transparencySpeed = 1f;
    public float returnTimer = 0.7f;
    private Coroutine returnCoroutine;

    private void Awake()
    {
        r = GetComponent<MeshRenderer>();
        if (r != null)
        {
            transparent = r.material;
        }
    }

    public void ChangeTransparency(bool isTransparent)
    {
        if (isTransparent)
        {
            StartCoroutine(SetTransparency(alpha_));
        }
        else
        {
            if (returnCoroutine != null)
            {
                StopCoroutine(returnCoroutine);
            }
            returnCoroutine = StartCoroutine(SetTransparency(1f));
        }
    }

    private IEnumerator SetTransparency(float targetAlpha)
    {
        transparent.SetFloat("_Surface", 1);
        Color color = transparent.GetColor("_Color");
        float startAlpha = color.a;
        float Timer = 0f;

        while (Timer < returnTimer)
        {
            Timer += Time.deltaTime;
            float t = Mathf.Clamp01(Timer / returnTimer);
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            transparent.SetColor("_Color", color);

            transparent.color = color;
            yield return null;
        }
        color.a = targetAlpha;
        transparent.SetColor("_Color", color);
        transparent.color = color;

        
  
    }
}
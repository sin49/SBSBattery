using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UIScaleChange : MonoBehaviour
{
    RectTransform uiRect;

    private void Awake()
    {
        uiRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (GameManager.instance != null)
            ScreenAdjust();
    }

    public void ScreenAdjust()
    {
        Vector3 scaleFix = new Vector3(1 / GameManager.instance.saveRatio, 1 / GameManager.instance.saveRatio, 1 / GameManager.instance.saveRatio);

        uiRect.localScale = scaleFix;
    }
}

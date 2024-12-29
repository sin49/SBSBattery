using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundHandle : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public PauseSoundSetting pauseSound;
    public Image sliderImage;   

    public int volumeIndex; 
    bool onHandle, isActive;

    Slider handleSlider;

    private void Awake()
    {
        handleSlider= GetComponentInParent<Slider>();
    }

    private void Update()
    {
        if (!isActive) return;
        UpdateActiveSlider();
    }

    public void ActiveSlider()
    {
        Debug.Log("ActiveSlider call");
        isActive = true;
        sliderImage.sprite = pauseSound.activeVolume;
        onHandle = true;
        pauseSound.cantControl = true;
    }

    public void UpdateActiveSlider()
    {
        if (onHandle)
        {
            pauseSound.UpdateVolumeByHandle(volumeIndex);
        }

        Debug.Log(onHandle);
    }

    public void DeactiveSlider()
    {
        Debug.Log("DeactiveSlider call");
        isActive = false;
        sliderImage.sprite = pauseSound.deactiveVolume;
        onHandle = false;
        pauseSound.cantControl = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("drag End");
        //isActive = false;
        DeactiveSlider();
        pauseSound.selected = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pointerdown call");
        ActiveSlider();
        pauseSound.selected = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("draging");
        handleSlider.OnDrag(eventData);
    }
}

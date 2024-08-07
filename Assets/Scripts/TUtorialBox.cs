using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TUtorialBox : MonoBehaviour
{
    public Canvas Canvas;



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(Canvas != null)
                Canvas.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Destroy(TutorialText.transform.parent.gameObject);
            Canvas.gameObject.SetActive(false);
            //TutorialText.gameObject.SetActive(false);
        }
    }
}

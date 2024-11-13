using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TUtorialBox : MonoBehaviour
{
    public Canvas Canvas;

    public string TutorialValue;

    private void Awake()
    {
        Canvas.gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (TutorialValue == "마우스")
            {
                if (PlayerHandler.instance != null)
                {
                    if (PlayerHandler.instance.CurrentType == TransformType.mouseform)
                    {
                        if (Canvas != null)
                            Canvas.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (Canvas != null)
                            Canvas.gameObject.SetActive(false);
                    }

                }
            }
            else
            {
                if (Canvas != null)
                    Canvas.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Destroy(TutorialText.transform.parent.gameObject);
            if (Canvas != null)
                Canvas.gameObject.SetActive(false);
            //TutorialText.gameObject.SetActive(false);
            //if (SceneManager.GetActiveScene().name == "Tutorial" && !PlayerPrefs.HasKey("AttackTuto"))
            //    GameManager.instance.attackTuto = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckTutorialValue();
        }
    }

    public void CheckTutorialValue()
    {
        switch (TutorialValue)
        {
            case "공격":
                //PlayerPrefs.SetInt("AttackTuto", 1);
                GameManager.instance.attackTuto = true;
                break;
            default:
                break;
        }

    }
}

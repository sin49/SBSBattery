using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameUIManager : MonoBehaviour
{
    public GameObject RemoteTargetUI;
    public GameObject InteractTargetUI;
    public GameObject StageNameObject;
    public TextMeshProUGUI StageNameText;
    private void Awake()
    {
        RemoteTargetUI.SetActive(false);
        InteractTargetUI.SetActive(false);
    }
    private void Start()
    {
        StageNameText.text=SceneManager.GetActiveScene().name;//나중에 따로 이름 정할수있는 공간 만들기
        StageNameText.gameObject.SetActive(true);
    }
    public void UpdateRemoteTargetUI(GameObject target)
    {
        if (target != null)
        {

            RemoteTargetUI.SetActive(true);
            RemoteTargetUI.transform.position = PlayerHandler.instance.CurrentCamera.WorldToScreenPoint(
       target.transform.position
                );

        }
        else
        {

            RemoteTargetUI.SetActive(false);
        }
    }
    public void UpdateInteractUI(GameObject target)
    {

            InteractTargetUI.SetActive(true);
            InteractTargetUI.transform.position = PlayerHandler.instance.CurrentCamera.WorldToScreenPoint(
                    target.transform.GetChild(0).position
                );

     
       
    }

}

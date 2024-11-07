using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string SceneName;
    public int SceneIndex;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //GameManager.instance.LoadingScene(SceneName);
            GameManager.instance.loadscenebycheckpoint(SceneIndex);
        }
    }
}

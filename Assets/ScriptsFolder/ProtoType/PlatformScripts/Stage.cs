using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public bool stageReady;

    private void FixedUpdate()
    {
        if (stageReady && Input.GetKeyUp(KeyCode.F))
        {
            SceneManager.LoadScene("NextStage");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stageReady = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stageReady = false;
        }
    }
}

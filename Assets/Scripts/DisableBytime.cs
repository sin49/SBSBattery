using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBytime : MonoBehaviour
{
    public float time = 1;

    private void OnEnable()
    {
        StartCoroutine(DisableBytimeCorutine());
    }
    IEnumerator DisableBytimeCorutine()
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }
   
}

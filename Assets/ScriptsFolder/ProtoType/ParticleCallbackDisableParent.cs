using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCallbackDisableParent : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        if(transform.parent != null)
        this.transform.parent.gameObject.SetActive(false);
    }
}

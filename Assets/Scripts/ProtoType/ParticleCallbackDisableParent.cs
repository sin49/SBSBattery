using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCallbackDisableParent : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
}

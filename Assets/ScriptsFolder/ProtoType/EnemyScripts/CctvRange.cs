using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CctvRange : MonoBehaviour
{
    CctvEnemy cctvE;
    Light cctvLight;
    CapsuleCollider capsule;

    Coroutine ct;

    private void Awake()
    {
        cctvE = GetComponentInParent<CctvEnemy>();
        cctvLight = transform.parent.GetComponent<Light>();
        capsule = GetComponent<CapsuleCollider>();        
    }

    private void Update()
    {
        capsule.height = cctvLight.range / 4;
        capsule.center = new(capsule.center.x, capsule.height / 2, capsule.center.z);
        capsule.radius = cctvLight.spotAngle * 0.02f;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("거림");
        if (other.CompareTag("Player"))
        {
            cctvE.tap.tracking = true;
            cctvE.tap.target = other.transform;
            cctvE.TrackingPlayer();
            if (ct != null)
            {
                StopCoroutine(ct);
                ct = null;
            }
            ct = StartCoroutine(WaitCallEnemy());            
            Debug.Log("배터리 발견함");
        }
    }    

    IEnumerator WaitCallEnemy()
    {
        yield return new WaitForSeconds(cctvE.callTime);

        if (cctvE.eStat.hp > 0)
        {
            //cctvE.EnemyCall();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ct != null)
            {
                StopCoroutine(ct);
                ct = null;
            }
            ct = StartCoroutine(StopTrackingTime());
            Debug.Log("배터리 도망감");
        }
    }

    IEnumerator StopTrackingTime()
    {
        yield return new WaitForSeconds(cctvE.stopTime);
        cctvE.tap.tracking = false;
        cctvE.endWait = true;
        cctvE.pointCheck = true;
    }
}

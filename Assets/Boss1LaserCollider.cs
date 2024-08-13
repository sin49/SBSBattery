using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1LaserCollider : MonoBehaviour
{
    float lifetime;
    float ScaleDecreaseSpeed;
 public   event Action<Boss1LaserCollider> PullingAction;
    public void initLaserCollider(float lifetime,Vector3 Scale,Action<Boss1LaserCollider> pullingevent)
    {
        this.lifetime = lifetime;
        this.transform.localScale = Scale;
        ScaleDecreaseSpeed = transform.localScale.x / lifetime;
        PullingAction = pullingevent;
        StartCoroutine(LasercolHandle());
    }
    IEnumerator LasercolHandle()
    {
        while (lifetime>0)
        {
            transform.localScale -= ScaleDecreaseSpeed * Vector3.one * Time.fixedDeltaTime;
            lifetime -= Time.fixedDeltaTime;
            yield return null;
        }
        PullingAction?.Invoke(this);
        this.gameObject.SetActive(false);
        PullingAction = null;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerHandler.instance.CurrentPlayer.onInvincible)
        {
            Debug.Log("레이저 피해");
            //other.GetComponent<Player>().Damaged(1);
        }
    }
}

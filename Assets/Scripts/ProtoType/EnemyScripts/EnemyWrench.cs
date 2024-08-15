using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWrench : MonoBehaviour
{
    public float damage;
    public float rangeSpeed;
    public float rotateSpeed;
    public GameObject rotateObj;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * rangeSpeed * Time.deltaTime, Space.World);
        rotateObj.transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")
            && !PlayerHandler.instance.CurrentPlayer.onInvincible)
        {
            PlayerHandler.instance.CurrentPlayer.Damaged(damage);
            if(PoolingManager.instance != null)
            PoolingManager.instance.ReturnPoolObject(this.gameObject);
        }
        else if (other.CompareTag("Ground") || other.CompareTag("InteractiveObject")
            || other.CompareTag("InteractivePlatform") || other.CompareTag("GameController"))
        {
            if (PoolingManager.instance != null)
                PoolingManager.instance.ReturnPoolObject(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("InteractiveObject")
            || collision.gameObject.CompareTag("InteractivePlatform") || collision.gameObject.CompareTag("GameController"))
        {
            if (PoolingManager.instance != null)
                PoolingManager.instance.ReturnPoolObject(this.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        if (PoolingManager.instance != null)
            PoolingManager.instance.ReturnPoolObject(this.gameObject);
    }
}

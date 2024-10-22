using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCollider : MonoBehaviour
{
    public float lifetime;
    float lifetime_;
    private void OnEnable()
    {
        lifetime_ = lifetime;
    }
    private void FixedUpdate()
    {
        if (lifetime_ > 0) lifetime_ -= Time.deltaTime; else this.gameObject.SetActive(false);
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&(int)PlayerStat.instance.MoveState>3)
        {
            PlayerHandler.instance.CurrentPlayer.Damaged(1);

        }
    }
}

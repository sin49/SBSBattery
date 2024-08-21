using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour,DamagedByPAttack
{
    public int hp;
    public ParticleSystem DestroyEffect;
    public void Damaged(float f)
    {
        hp--;
        if (hp <= 0)
        {
            if(DestroyEffect!=null)
            Instantiate(DestroyEffect,transform.position,transform.rotation);
            //�ı� ���� ����
            this.gameObject.SetActive(false);
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Damaged(1);
            other.gameObject.SetActive(false);
        }
    }
}

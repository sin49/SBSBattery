using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : PlayerAttack
{
    public GameObject hitEffect; // ����Ʈ ������
    
    public ParticleSystem saveEffect; // ��ƼŬ ����

    private void Start()
    {
        //saveEffect = Instantiate(hitEffect).GetComponent<ParticleSystem>();
        damage = PlayerStat.instance.atk;
        gameObject.SetActive(false);
    }
    public override void DamageCollider(Collider other)
    {
        base.DamageCollider(other);
        Vector3 hitPos = new(other.transform.position.x, other.transform.position.y + .5f, other.transform.position.z);
        Instantiate(hitEffect, hitPos, Quaternion.identity);
        //saveEffect.transform.position = new(other.transform.position.x, other.transform.position.y + .5f, other.transform.position.z);
        //saveEffect.Play();
    }
}
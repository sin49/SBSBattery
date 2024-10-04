using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RagdolEnemy : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public float secondForce;
    public Rigidbody ele;
    public Collider enemyCol;
    public Animator ani;

    public Transform body;

    public Collider[] c;
    public Rigidbody[] r;
    [HideInInspector] public bool isRagdoll;

    public RagdolHit rh;

    public float DeadTime;


    private void Awake()
    {
        initializeragdol();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Y))
        {
            RagdollOn();
            if(isRagdoll)
            rb.AddForce((Vector3.forward + Vector3.up) * force, ForceMode.Impulse);
        }*/

        if (isRagdoll)
        {
            rb.AddForce(transform.forward * secondForce);
        }
    }

    public void ThrowRagdoll()
    {
        RagdollOn();
        if (isRagdoll)
        {
            rb.AddForce((transform.forward + transform.up) * force, ForceMode.Impulse);
            rh.enabled = true;

            StartCoroutine(DeadTimer());
        }
    }

    public void RagdolDeadEffect(ParticleSystem deadEffect)
    {
        Instantiate(deadEffect, rb.transform.position, Quaternion.identity);
    }

    public void RagdollOn()
    {
        isRagdoll = true;

        foreach (Collider c in c)
        {
            c.enabled = true;
        }
        foreach (Rigidbody r in r)
        {
            r.isKinematic = false;
        }


        ani.enabled = false;
        Destroy(ele);
        enemyCol.enabled = false;
    }

    public void initializeragdol()
    {
        c = body.GetComponentsInChildren<Collider>();
        r = body.GetComponentsInChildren<Rigidbody>();
        isRagdoll = false;

        foreach (Collider c in c)
        {
            c.enabled = false;
        }

        foreach (Rigidbody r in r)
        {
            r.isKinematic = true;
        }

        rh.enabled = false;
    }
    

    IEnumerator DeadTimer()
    {
        float timer = 0;
        while (timer < DeadTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Dead();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    Rigidbody rb;
    public float damage;
    public float fallingwaitingtime;
    public float fallingSpeed;

    public Vector3 fieldPos;
    public GameObject warningObj;
    public Vector3 circlePos;
    public float disToField;


    public Action ObjectgroundedSoundEvent;
    public GameObject hitEffect;
  
    
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        circlePos = new(transform.position.x, fieldPos.y + 0.1f, transform.position.z);
    }

    private void OnDestroy()
    {
        ObjectgroundedSoundEvent?.Invoke();
        ObjectgroundedSoundEvent = null;
    }
    private void Update()
    {

        transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime, Space.World);
        //warningObj.transform.position = circlePos;
        WarningValue();
    }

    public void WarningValue()
    {
        Vector3 vec = circlePos - transform.position;
        disToField = vec.magnitude;

        //Color warningColor = warningObj.GetComponent<SpriteRenderer>().color;
        //warningColor.a += 0.45f * Time.deltaTime;
        //warningObj.GetComponent<SpriteRenderer>().color = warningColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("보스 낙하물 피격");
            PlayerHandler.instance.CurrentPlayer.Damaged(damage);
            if(hitEffect != null)
            Destroy(Instantiate(hitEffect, transform.position, Quaternion.identity), 2f);
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            if(hitEffect != null)
            Destroy(Instantiate(hitEffect, transform.position, Quaternion.identity), 2f);
            Destroy(gameObject);
        }
    }


}

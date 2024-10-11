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
    public GameObject warningObj3D;
    public Vector3 circlePos;
    public float disToField;


    public Action ObjectgroundedSoundEvent;
    public GameObject hitEffect;
    public bool endCorutine;
    public float DisableTimer=0.25f;

    IEnumerator FallEffectCorutine()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        yield return new WaitForSeconds(DisableTimer);
        rb.isKinematic = false;
        rb.useGravity = true;
  
        warningObj.SetActive(false);
        warningObj3D.SetActive(false);
        endCorutine = true;
    }
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        circlePos = new(transform.position.x, fieldPos.y + 0.1f, transform.position.z);
        StartCoroutine(FallEffectCorutine());
    }

    private void OnDestroy()
    {
        ObjectgroundedSoundEvent?.Invoke();
        ObjectgroundedSoundEvent = null;
    }
    private void Update()
    {
        if(endCorutine)
        transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime, Space.World);
        //warningObj.transform.position = circlePos;
        WarningValue();
    }
    private void OnEnable()
    {
        if ((int)PlayerStat.instance.MoveState < 4)
        {
            warningObj.SetActive(true);
            warningObj3D.SetActive(false);
        }
        else
        {
            warningObj3D.SetActive(true);
            warningObj.SetActive(false);
        }
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
            Instantiate(hitEffect, transform.position, Quaternion.identity);

;
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
          

            Destroy(gameObject);
        }
    }


}

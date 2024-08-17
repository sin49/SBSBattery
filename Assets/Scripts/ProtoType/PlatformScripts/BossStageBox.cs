using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.TextCore;
using UnityEngine.Rendering;
using System;
using Random = UnityEngine.Random;


public class BossStageBox : MonoBehaviour
{
    Rigidbody rb;
    public GameObject enemyPrefab;
    public bool onGround;
    public Animator boxAnim;
    public GameObject deadEffect;

    public GameObject warningObj;
    public float fallingSpeed;
    public Vector3 fieldPos;
    public Vector3 warningPos;

    public float disToField;

    [Header("추가 벡터, 벡터 결정할 최소/최대 범위")]
    public Vector3 distanceValue; // 현재 오브젝트의 좌표값에 더할 벡터
    public Vector3 min, max; // 더해줄 벡터의 값을 결정할 최소/최대 벡터값

    public Vector3 fieldMin;
    Vector3 targetVec;


    public Action ObjectgroundedSoundEvent;

    bool fixVec;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();        
        warningPos = new(transform.position.x, fieldPos.y+0.1f, transform.position.z);
        RandomDistanceValue();
    }
  
    private void OnDestroy()
    {
        ObjectgroundedSoundEvent?.Invoke();
        ObjectgroundedSoundEvent = null;
    }
    private void Update()
    {
        if (!onGround)
        {
            transform.Translate(Vector3.down.normalized * fallingSpeed * Time.deltaTime, Space.World);
            warningObj.transform.position = warningPos;
        }
        WarningValue();
    }

    public void WarningValue()
    {
        Vector3 vec = warningPos - transform.position;
        disToField = vec.magnitude;

        Color warningColor = warningObj.GetComponent<SpriteRenderer>().color;
        warningColor.a += 0.45f * Time.deltaTime;
        warningObj.GetComponent<SpriteRenderer>().color = warningColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            boxAnim.SetTrigger("Open");
        }
    }

    public void SpawnBoxEnemy()
    {
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        var obj= Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        /*obj.transform.position = new(transform.position.x, transform.position.y, PlayerHandler.instance.CurrentPlayer.transform.localPosition.z);*/
        SetJumpVector(obj);
        ObjectJump(distanceValue, obj);
    }

    public void RandomDistanceValue()
    {
        distanceValue.x = Random.Range(min.x, max.x);
        distanceValue.y = Random.Range(min.y, max.y);
        distanceValue.z = Random.Range(min.z, max.z);
    }

    public void SetJumpVector(GameObject obj)
    {
        targetVec = transform.position + distanceValue;
        RetryTargetVec();
        Vector3 vec = targetVec - transform.position;
        Vector3 normalVec = vec.normalized;

        Vector3 angleAxis = Vector3.Cross(transform.forward, normalVec);
        float angle = Mathf.Acos(Vector3.Dot(transform.forward, normalVec)) * Mathf.Rad2Deg;

        obj.transform.rotation = Quaternion.AngleAxis(angle, angleAxis);
        obj.transform.rotation = Quaternion.Euler(0, obj.transform.eulerAngles.y, 0);

        BossStageEnemy bse;
        if (obj.TryGetComponent<BossStageEnemy>(out bse))
        {
            bse.rotPos = targetVec;
        }
    }

    public void RetryTargetVec()
    {
        if (targetVec.x > fieldPos.x)
        {
            fixVec = false;
            targetVec.x =targetVec.x-6;
            distanceValue.x = -distanceValue.x;
            Debug.Log("fixVec.x is Big");
        }
        else
        {
            fixVec = true;
        }

        if (targetVec.x < fieldMin.x)
        {
            fixVec = false;
            targetVec.x= targetVec.x+6;
            Mathf.Abs(distanceValue.x);
            Debug.Log("fixVec.x is Small");
        }
        else
        {
            fixVec = true;
        }

        if (targetVec.z > fieldPos.z)
        {
            
            targetVec.z = targetVec.z - 6;
            distanceValue.z = -distanceValue.z;
            Debug.Log("fixVec.z is Big");
        }


        if (targetVec.z < fieldMin.z)
        {
            targetVec.z = targetVec.z + 6;
            Mathf.Abs(distanceValue.z);
            Debug.Log("fixVec.z is Small");
        }        

        targetVec = transform.position + distanceValue;
    }

    public void ObjectJump(Vector3 distanceValue, GameObject obj)
    {
        Rigidbody rigid = obj.gameObject.GetComponent<Rigidbody>();

        float v_y = Mathf.Sqrt(2 * -Physics.gravity.y * distanceValue.y);

        float v_x = distanceValue.x * v_y / (2 * distanceValue.y);

        float v_z = distanceValue.z * v_y / (2 * distanceValue.y);

        Vector3 force = rigid.mass * (new Vector3(v_x, v_y, v_z) - rigid.velocity);
        rigid.AddForce(force, ForceMode.Impulse);
    }
}

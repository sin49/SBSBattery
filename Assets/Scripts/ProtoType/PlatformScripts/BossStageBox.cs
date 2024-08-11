using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();        
        warningPos = new(transform.position.x, fieldPos.y+0.1f, transform.position.z);
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
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        /*obj.transform.position = new(transform.position.x, transform.position.y, PlayerHandler.instance.CurrentPlayer.transform.localPosition.z);*/
    }
}

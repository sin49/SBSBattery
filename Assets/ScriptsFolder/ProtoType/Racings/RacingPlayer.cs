using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingPlayer : MonoBehaviour
{
    public float moveLimit;
    public float movespeed;
    public float jumpTIme;
    float jumptimer;
    bool Onjump;
    float moveLeftMeter;
    float MoveRightMeter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveLeftMeter = 25 - moveLimit;
        MoveRightMeter = 25 + moveLimit;
        if (Onjump)
        {
            jumpTIme += Time.deltaTime;
            if (jumpTIme >= jumptimer)
            {
                jumpTIme = 0;
                Onjump = false;
            }
        }
        if (this.transform.position.x < moveLeftMeter)
        {
            this.transform.Translate(Vector3.right* (this.transform.position.x- moveLeftMeter));
        }
        if (this.transform.position.x > MoveRightMeter)
        {
            this.transform.Translate(Vector3.right * (this.transform.position.x - MoveRightMeter));
        }
        if (Input.GetKey(KeyCode.LeftArrow))    
        {
            this.transform.Translate(Vector3.left * movespeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(Vector3.right * movespeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.C))
        {
            Onjump = true;
        }
    }
}

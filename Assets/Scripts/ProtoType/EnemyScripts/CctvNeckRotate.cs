using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CctvNeckRotate : MonoBehaviour
{
    public Transform target;
    [Tooltip("회전 속도")]public float rotateSpeed;
    

    public CctvEnemy cctv;
    [HideInInspector]
    public float angleValue;
    public float waitTimer;
    private void Awake()
    {
        cctv = GetComponentInParent<CctvEnemy>();
    }

    private void Start()
    {
        waitTimer = cctv.waitTime;
    }

    private void Update()
    {
        if (!cctv.endWait && cctv.pointCheck)
        {
            if (waitTimer > 0)
            {
                waitTimer -= Time.deltaTime;
            }
            else
            {
                cctv.endWait = true;
                waitTimer = cctv.waitTime;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
  
        if (target != null) {
         
            Vector3 vec = (target.position - transform.position).normalized;
            vec.y = 0;
            var a = Quaternion.LookRotation(vec);
            
            //a.y += 90;
            //a.z -= 90;
            var b = Quaternion.RotateTowards(transform.rotation, a, rotateSpeed * Time.fixedDeltaTime);

            var c = Quaternion.Euler(0, b.eulerAngles.y, 90);
            //c.x -= 90;
            //c.y -= 90;
            //c.z -= 90;
            transform.rotation = c;



            angleValue = Quaternion.Angle(transform.rotation, a);
            if(PlayerHandler.instance != null&& PlayerHandler.instance.CurrentPlayer!=null && target.gameObject != PlayerHandler.instance.CurrentPlayer.gameObject)
            {
                if (angleValue > 89.5f && angleValue < 90.005f)
                {
                    cctv.pointCheck = true;
                    target = null;
                }
            }
        }
    }
}

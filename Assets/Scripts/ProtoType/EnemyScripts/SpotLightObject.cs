using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SpotLightObject : MonoBehaviour
{
    public bool tracking;
    public Transform target;
    public Transform targetPlayer;
    Quaternion lightRot;

    [Header("����Ʈ ���ӽð�, ����Ʈ�� ���� �� ������Ʈ�� �ӵ�")]
    float spotLightTimer;
    public float spotLightTime;
    public float lightSpeed;
    public Light spotLight;
    public Color originColor;
    public Color checkColor;

    [Header("���� �Ÿ���, ���� ���󰡱� �� ���ð�")]
    public float targetDistance;
    float disValue;
    float readyTimer;
    public float timerMax;

    [Header("�������Ͽ� ���� ����")]
    public GameObject fallingObject;
    bool readyFalling;
    float fallingTimer;
    public float fallingTime;
    public float blinkTime;
    Vector3 fallingPoint;

    private void Awake()
    {
        spotLight = GetComponentInChildren<Light>();
        lightRot = transform.rotation;
        readyTimer = timerMax;
    }

    private void Start()
    {
        //StartCoroutine(TrackingSpotLight());
        spotLight.enabled = false;
    }

    private void Update()
    {
        //TargetQueue();
      
        if (tracking)
        {
            transform.LookAt(target);
        }
    }

    private void FixedUpdate()
    {
        //while (readyTimer > 0)
        //{
        //    readyTimer -= Time.deltaTime;
        //    //yield return null;
        //}

        //while (true/*spotLightTimer < spotLightTime*/)
        //{
        if (fallingTimer > 0)
        {
            fallingTimer -= Time.deltaTime;
        }

        if (readyTimer < 0)
        {
            //target.LookAt(target);

            var vector = (targetPlayer.position - target.transform.position);
            vector.y = 0f;
            if (vector.magnitude > 0.5f)
            {
                Debug.Log(vector.normalized);
                var MoveVector = vector.normalized * lightSpeed;
                //Debug.Log(MoveVector + "ũ��" + MoveVector.magnitude);
                if (MoveVector.magnitude < 2)
                {

                    MoveVector = MoveVector.normalized * 2;
                }

                if(!readyFalling)
                    target.Translate(MoveVector * Time.deltaTime, Space.World);
            }
            else
            {
                if (fallingTimer <= 0 && !readyFalling)
                {
                    readyFalling = true;
                    fallingPoint = new Vector3(target.position.x, transform.position.y, target.position.z);
                    StartCoroutine(FallingAttack());
                }

            }            
           
            spotLightTimer += Time.deltaTime;
            if (spotLightTimer > spotLightTime)
                this.enabled = false;
        }
        else
        {
            readyTimer -= Time.deltaTime;
        }
    }
    public float fallingwaitingtime;
    IEnumerator FallingAttack()
    {
        //spotLight.color = checkColor;        
        spotLight.enabled = true;
       var obj= Instantiate(fallingObject, fallingPoint, Quaternion.identity).GetComponent<FallingObject>();
        obj.fallingwaitingtime = fallingwaitingtime;
        yield return new WaitForSeconds(blinkTime);



        //spotLight.color = originColor;
        spotLight.enabled = false;
        readyFalling = false;
        fallingTimer = fallingTime;
    }

    public void HandleSpotLight(HandleSpotlight handle)
    {
        target = handle.moveTarget.transform;
        tracking = true;
        //StartCoroutine(TrackingLight());
    }

    public void HandleSpotLight(BossHandle handle)
    {
        target = handle.moveTarget.transform;
        tracking = true;
    }

    public void InitRotation()
    {
        transform.rotation = lightRot;
        gameObject.SetActive(false);
    }

    //IEnumerator TrackingSpotLight()
    //{
       
    //}

    IEnumerator TrackingLight()
    {
        while (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            transform.LookAt(target);
            yield return null;
        }
    }
}

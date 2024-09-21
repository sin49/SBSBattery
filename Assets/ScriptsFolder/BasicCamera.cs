using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicCamera : MonoBehaviour
{
    public Transform target;
    
    protected Vector3 camPos;
    protected Vector3 camRot;
    public bool ZPin;

    CameraMoveRange bindingcamera;
    [Range(0, 1), Header("ī�޶� �ö󰡴� �÷��̾� ��ġ ����")]
    public float CameraUPViewportPos;
    [Range(0, 1), Header("ī�޶� �������� �÷��̾� ��ġ ����")]
    public float CameraDownViewportPos;
    [Header("ī�޶� �߰� �ð�")]
    public float CameraTrakingTime;

    protected Camera CurrentCamera;

    protected Vector3 CalculateVector;
    [Header("ī�޶� ��鸲 ȿ��")]
    [Header("���� �ð�")]
    public float shakeDuration = 0.5f;

    [Header("���� ����")]
    public float shakeMagnitude = 0.2f;
    [Header("ȸ�� �ӵ�")]
    public float dampingSpeed = 1.0f;

  

    private Vector3 initialPosition;
    private float currentShakeDuration;
    bool CameraShakingChecker;

    void initializeCameraPosition()
    {
        if(target!=null)
        CurrentCamera.transform.position = target.position + camPos;
     
    }
    public void StartCameraShake()
    {
     
        currentShakeDuration = shakeDuration;
        initialPosition = CurrentCamera.transform.localPosition;
        CameraShakingChecker = true;
    }
    void CameraShake()
    {
        if (currentShakeDuration > 0)
        {
            CurrentCamera.transform.localPosition = CurrentCamera.transform.localPosition + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else if(CameraShakingChecker)
        {
            currentShakeDuration = 0;
            if (target)
            {
                CurrentCamera.transform.position = target.position + camPos;
            }
            else
            {
                CurrentCamera.transform.localPosition = initialPosition;
            }
            CameraShakingChecker = false;
        }
        
    }
    protected virtual void Awake()
    {
        bindingcamera=GetComponent<CameraMoveRange>();
    }
    protected virtual void Start()
    {
        PlayerHandler.instance.registerPlayerFallEvent(initializeCameraPosition);
    }
    protected virtual float CalculateCameraVector()
    {

        float cameraVector = ((target.position + camPos) - CurrentCamera.transform.position).magnitude;
        return cameraVector / CameraTrakingTime;
    }
    protected void CameraMove(Camera c, float cameraspeed)
    {
        //X�� ������
        float Xvector = target.position.x + camPos.x;
        //Z�� ������
        float Zvector = c.transform.position.z;
        if (!ZPin)
            Zvector = target.position.z + camPos.z;


        //y�� ������

        float Yvector = c.transform.position.y;
        float targetPosYInViewport = c.WorldToViewportPoint(target.transform.position).y;
        //Debug.Log(targetPosYInViewport);
        if (targetPosYInViewport > CameraUPViewportPos)
        {
 
            Yvector = target.position.y + camPos.y;

            //if (target.position.y - transform.position.y < 0)
            //    Yvector = c.transform.position.y;
        }
        else if (targetPosYInViewport < CameraDownViewportPos)
        {

            Yvector = target.position.y + camPos.y;
            //if (target.position.y - transform.position.y > 0)
            //    Yvector = c.transform.position.y;
        }
        //Debug.Log("target.position.y-transform.position" + (target.position.y - transform.position.y));

        CalculateVector = new Vector3(Xvector, Yvector, Zvector);

        // Check for NaN values in CalculateVector
        if (float.IsNaN(CalculateVector.x) || float.IsNaN(CalculateVector.y) || float.IsNaN(CalculateVector.z))
        {
            Debug.LogError("CalculateVector contains NaN values: " + CalculateVector);
            return; // Exit the method to prevent further issues
        }

        //Debug.Log(c.transform.position);
        if (!c.orthographic)
        c.transform.position = Vector3.Lerp(c.transform.position, CalculateVector, Time.deltaTime * cameraspeed);//���� ��ġ
        else
            c.transform.position = Vector2.Lerp(c.transform.position, CalculateVector, Time.deltaTime * cameraspeed);


     


    }

    protected virtual void TargetIsPlayer()
    {
        //Ÿ�� Ž��
        if (PlayerHandler.instance.CurrentPlayer != null)
            target = PlayerHandler.instance.CurrentPlayer.transform;
       
    }

    protected virtual void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
            StartCameraShake();
            TargetIsPlayer();
        if (target == null || CurrentCamera == null)
            return;
        CameraMove(CurrentCamera, CalculateCameraVector());
        if(CurrentCamera!=null)
        PlayerHandler.instance.CurrentCamera = CurrentCamera;

        CameraShake();

        if (bindingcamera != null)
            bindingcamera.BindingCamera(CurrentCamera);
    }
}

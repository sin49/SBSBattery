using System.Collections;
using UnityEngine;

public class SwitchingCamera : BasicCamera
{
    private bool isTransitioning;

    public Camera Camera2D;
    public Camera Camera3D;


    [Header("2D 카메라 회전")]
    public Vector3 camrot2D;

    [Header("2D 카메라 위치")]
    public Vector3 camPos2D;
    [Header("2D 카메라 orthographic 사이즈")]
    public float orthographicSize2D = 5f;
    [Header("2D 카메라 near/far clipping planes")]
    public float nearClipPlane2D = -10f;
    public float farClipPlane2D = 10f;

    [Header("3D 카메라 회전")]
    public Vector3 camrot3D;
    [Header("3D 카메라 위치")]
    public Vector3 camPos3D;
    [Header("3D 카메라 field of view")]
    public float fieldOfView3D = 60f;
    [Header("3D 카메라 near/far clipping planes")]
    public float nearClipPlane3D = 0.1f;
    public float farClipPlane3D = 1000f;



    [Header("2D할지 3D할지 결정하는거 (Play mode 중에는 건들지 말기)")]
    public bool is2D;




    [Header("카메라 2D/3D 전환 시간")]
    public float CameraChangeDuration = 1.0f;




   protected override void Start()
    {
        base.Start();
        Apply2DSettings();
        Apply3DSettings();
        Camera3D.transform.rotation=Camera2D.transform.rotation;
        SwapCurrentCamera();
        PlayerStat.instance.MoveState = PlayerMoveState.Xmove;
        PlayerHandler.instance.registerCorutineRegisterEvent(StartChangeCameraCorutine);

    }

    protected override float CalculateCameraVector()
    {
        camPos = is2D ? camPos2D : camPos3D;
        camRot = is2D ? camrot2D : camrot3D;
        return base.CalculateCameraVector();
    }
   
    protected override void FixedUpdate()
    {
        if (isTransitioning)
            return;
        Apply2DSettings();
        Apply3DSettings();
        
        if (!is2D)
            Camera2D.transform.position = Camera3D.transform.position;
        else
        {
            Camera3D.transform.position = Camera2D.transform.position;
        }
        base.FixedUpdate();

       
    }

    public void ActiveZPin(float f)
    {
        ZPin = true;
        CurrentCamera.transform.position = new Vector3(CurrentCamera.transform.position.x, CurrentCamera.transform.position.y, f);
    }

    public void DeactiveZPin()
    {
        ZPin = false;
    }
    public void StartChangeCameraCorutine()
    {
        PlayerHandler.instance.RegisterCameraRotateCorutine(SwitchCameraMode());
    }
  

    IEnumerator SwitchCameraMode()
    {
        Debug.Log("실행됨");
        //다른 이벤트 적용의 시간을 잠깐 준다
        //yield return new WaitForSeconds(0.06f);
        CalculateCameraVector();

        CurrentCamera.transform.position = CalculateVector;

        if (!is2D)
            Camera2D.transform.position = Camera3D.transform.position;
        else
        {
            Camera3D.transform.position = Camera2D.transform.position;
        }
        isTransitioning = true;
        Time.timeScale = 0;
        is2D = !is2D;
        

        if (is2D)
        {
            Debug.Log("2D로");
            // 3D에서 2D로 전환
            yield return StartCoroutine(TransitionCamera(camPos2D, camrot2D, true));
           
        }
        else
        {
            Debug.Log("3D로");
            // 2D에서 3D로 전환
            yield return StartCoroutine(TransitionCamera(camPos3D, camrot3D, false));
        
        }

        Time.timeScale = 1.0f;
        isTransitioning = false;
    }

    void SwapCurrentCamera()
    {
        if (is2D)
        {
            Camera2D.transform.position = Camera3D.transform.position;
            Camera3D.enabled = false;
            Camera2D.enabled = true;
          
            CurrentCamera = Camera2D;
        }
        else
        {
            Camera3D.transform.position = Camera2D.transform.position;
            Camera2D.enabled = false;
            Camera3D.enabled = true;
            CurrentCamera = Camera3D;
        }
        if(is2D)
        PlayerStat.instance.MoveState = PlayerMoveState.Xmove;
        else
            PlayerStat.instance.MoveState = PlayerMoveState.XZMove3D;
    }

    IEnumerator TransitionCamera(Vector3 newPos, Vector3 newRot, bool isOrtho)
    {
        if(!is2D)
            SwapCurrentCamera();

        float elapsed = 0.0f;

        Vector3 startingPos = CurrentCamera.transform.position;
        Quaternion startingRot = CurrentCamera.transform.rotation;

        Vector3 targetPosition = target.position + newPos;

        while (elapsed < CameraChangeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / CameraChangeDuration);

            CurrentCamera.transform.position = Vector3.Lerp(startingPos, targetPosition, t);
            CurrentCamera.transform.rotation = Quaternion.Lerp(startingRot, Quaternion.Euler(newRot), t);

            yield return null;
        }

        ZPin = isOrtho;
        if(is2D)
            SwapCurrentCamera();
        if (isOrtho)
        {
            Vector3 finalPos = CurrentCamera.transform.position;
            finalPos.z = target.position.z + camPos2D.z;
            CurrentCamera.transform.position = finalPos;
        }
    }

    private void Apply2DSettings()
    {
        Camera2D.orthographicSize = orthographicSize2D;
        Camera2D.nearClipPlane = nearClipPlane2D;
        Camera2D.farClipPlane = farClipPlane2D;
        Camera2D.orthographic = true;
        Camera2D.transform.rotation = Quaternion.Euler(camrot2D);
    }

    private void Apply3DSettings()
    {
        Camera3D.fieldOfView = fieldOfView3D;
        Camera3D.nearClipPlane = nearClipPlane3D;
        Camera3D.farClipPlane = farClipPlane3D;
        Camera3D.orthographic = false;
        if(!is2D)
        Camera3D.transform.rotation = Quaternion.Euler(camrot3D);
    }
    private void RotateCameraTowardsPlayerDirection()
    {
        if (!is2D)
        {
            float hori = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");

   
            if (hori != 0 || vert != 0)
            {
               
                Vector3 direction = new Vector3(hori, 0, vert);
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                CurrentCamera.transform.rotation = Quaternion.Lerp(CurrentCamera.transform.rotation, targetRotation, Time.deltaTime * CalculateCameraVector());
            }
            else
            {

                CurrentCamera.transform.rotation = Quaternion.Lerp(
                    CurrentCamera.transform.rotation,
                    Quaternion.Euler(camrot3D),
                    Time.deltaTime * CalculateCameraVector());
            }
        }
    }
}
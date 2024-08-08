using UnityEngine;

public class CameraShake : MonoBehaviour
{
     Transform cameraTransform;
    [Header("흔드는 시간")]
    public float shakeDuration = 0.5f;

    [Header("흔드는 정도")]
    public float shakeMagnitude = 0.2f;
    [Header("회복 속도")]
    public float dampingSpeed = 1.0f;

    private Vector3 initialPosition;
    private float currentShakeDuration;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = GetComponent<Transform>();
        }
        initialPosition = cameraTransform.localPosition;//쉐이크 시작 전에 미리 현재 포지션 받기
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
            TriggerShake();
        //일정 시간동안 포지션이 현재 포지션의 사각범위의 랜덤 백터값을 받는다
        //shakeMagnitude=흔드는 힘
        if (currentShakeDuration > 0)
        {
            cameraTransform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            currentShakeDuration = 0f;
            cameraTransform.localPosition = initialPosition;
        }
    }

    public void TriggerShake()
    {
        currentShakeDuration = shakeDuration;
    }
}


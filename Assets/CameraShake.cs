using UnityEngine;

public class CameraShake : MonoBehaviour
{
     Transform cameraTransform;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.2f;
    public float dampingSpeed = 1.0f;

    private Vector3 initialPosition;
    private float currentShakeDuration;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = GetComponent<Transform>();
        }
        initialPosition = cameraTransform.localPosition;//����ũ ���� ���� �̸� ���� ������ �ޱ�
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
            TriggerShake();
        //���� �ð����� �������� ���� �������� �簢������ ���� ���Ͱ��� �޴´�
        //shakeMagnitude=���� ��
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


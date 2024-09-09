using System.Collections;
using UnityEngine;

public class StageOverviewCamera : MonoBehaviour
{
    public Transform[] waypoints; // ī�޶� �̵��� ����� Ʈ��������
    public float transitionTime = 1.0f; // �� ��� ���̸� �̵��ϴ� �� �ɸ��� �ð�

    private int currentWaypointIndex = 0;

    void Start()
    {
        StartCoroutine(MoveAlongWaypoints());
    }

    IEnumerator MoveAlongWaypoints()
    {
        while (true)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            Vector3 initialPosition = transform.position;
            Quaternion initialRotation = transform.rotation;

            float elapsedTime = 0f;

            while (elapsedTime < transitionTime)
            {
                transform.position = Vector3.Lerp(initialPosition, targetWaypoint.position, elapsedTime / transitionTime);
                transform.rotation = Quaternion.Lerp(initialRotation, targetWaypoint.rotation, elapsedTime / transitionTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // ��Ȯ�� ��ǥ ��ġ�� �����ϵ��� ����
            transform.position = targetWaypoint.position;
            transform.rotation = targetWaypoint.rotation;

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}

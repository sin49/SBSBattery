using System.Collections;
using UnityEngine;

public class StageOverviewCamera : MonoBehaviour
{
    public Transform[] waypoints; // 카메라가 이동할 경로의 트랜스폼들
    public float transitionTime = 1.0f; // 각 경로 사이를 이동하는 데 걸리는 시간

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

            // 정확히 목표 위치에 도달하도록 설정
            transform.position = targetWaypoint.position;
            transform.rotation = targetWaypoint.rotation;

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}

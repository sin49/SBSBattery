using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPM : MonoBehaviour
{
    public List<Utility_Pole_Repository> utility_Pole;
    [Space(30)]
    public bool F5;
    [Space(30)]
    [Header("휘는 정도")]
    public float maxSagAmount = 0.2f; // 최대 휘는 정도
    public float minSagAmount = 3f; // 최소 휘는 정도
    [Space(10)]
    [Header("선 두께")]
    public float maxWireThickness = 0.03f; // 최대 선 두께
    public float minWireThickness = 0.01f; // 최소 선 두께
    [Space(10)]
    [Header("전선 구간 세그먼트 수 (16 추천)")]
    public int segmentsPerWire = 16; // 전선 구간 세그먼트 수
    [Space(10)]
    public Material wireMaterial; // 전선 머테리얼

    // 유니티 에디터에서 오브젝트가 변경되면 호출되는 메서드
    void OnValidate()
    {
        // 세그먼트 수가 1보다 작아지지 않도록 제한
        if (segmentsPerWire < 1)
        {
            segmentsPerWire = 1;
        }

        // 하위 오브젝트에서 Utility_Pole_Repository 추가
        UpdateUtilityPoles();

        if (utility_Pole == null || utility_Pole.Count == 0) return;

        RedrawWires(); // 위치가 변경되면 전선을 다시 그림
    }

    // 하위 오브젝트에서 Utility_Pole_Repository 추가
    void UpdateUtilityPoles()
    {
        // 현재 게임 오브젝트의 하위 오브젝트에서 Utility_Pole_Repository를 찾음
        Utility_Pole_Repository[] poles = GetComponentsInChildren<Utility_Pole_Repository>();

        // 리스트에 없는 전봇대를 추가
        foreach (var pole in poles)
        {
            if (!utility_Pole.Contains(pole))
            {
                utility_Pole.Add(pole);
            }
        }

        // 내림차순으로 정렬
        utility_Pole.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));
    }

    // 전선 다시 그리기
    void RedrawWires()
    {
        if (utility_Pole == null || utility_Pole.Count == 0) return;

        for (int i = 0; i < utility_Pole.Count - 1; i++)
        {
            CreateWireBetweenPoles(utility_Pole[i], utility_Pole[i + 1]);
        }
    }

    // 두 전봇대 사이에 전선을 생성하는 메서드
    void CreateWireBetweenPoles(Utility_Pole_Repository poleA, Utility_Pole_Repository poleB)
    {
        if (poleA.point == null || poleB.point == null) return;

        for (int i = 0; i < Mathf.Min(poleA.point.Count, poleB.point.Count); i++)
        {
            LineRenderer lineRenderer = poleA.point[i].GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                lineRenderer = poleA.point[i].gameObject.AddComponent<LineRenderer>();
            }

            // 선 두께를 랜덤하게 설정
            float randomThickness = Random.Range(minWireThickness, maxWireThickness);
            lineRenderer.startWidth = randomThickness;
            lineRenderer.endWidth = randomThickness;
            lineRenderer.positionCount = segmentsPerWire;
            lineRenderer.material = wireMaterial;
            lineRenderer.useWorldSpace = true;

            Vector3 startPoint = poleA.point[i].position;
            Vector3 endPoint = poleB.point[i].position;

            // 각 선에 대해 랜덤한 휘기 정도를 설정
            float randomSagAmount = Random.Range(minSagAmount, maxSagAmount);

            for (int j = 0; j < segmentsPerWire; j++)
            {
                float t = (float)j / (segmentsPerWire - 1);
                Vector3 currentPoint = Vector3.Lerp(startPoint, endPoint, t);

                // 랜덤 휘기 정도를 사용
                float sagFactor = Mathf.Sin(t * Mathf.PI) * randomSagAmount;
                currentPoint.y -= sagFactor;

                lineRenderer.SetPosition(j, currentPoint);
            }
        }
    }
}

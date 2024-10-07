using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPM : MonoBehaviour
{
    public List<Utility_Pole_Repository> utility_Pole;
    [Space(30)]
    public bool F5;
    [Space(30)]
    [Header("�ִ� ����")]
    public float maxSagAmount = 0.2f; // �ִ� �ִ� ����
    public float minSagAmount = 3f; // �ּ� �ִ� ����
    [Space(10)]
    [Header("�� �β�")]
    public float maxWireThickness = 0.03f; // �ִ� �� �β�
    public float minWireThickness = 0.01f; // �ּ� �� �β�
    [Space(10)]
    [Header("���� ���� ���׸�Ʈ �� (16 ��õ)")]
    public int segmentsPerWire = 16; // ���� ���� ���׸�Ʈ ��
    [Space(10)]
    public Material wireMaterial; // ���� ���׸���

    // ����Ƽ �����Ϳ��� ������Ʈ�� ����Ǹ� ȣ��Ǵ� �޼���
    void OnValidate()
    {
        // ���׸�Ʈ ���� 1���� �۾����� �ʵ��� ����
        if (segmentsPerWire < 1)
        {
            segmentsPerWire = 1;
        }

        // ���� ������Ʈ���� Utility_Pole_Repository �߰�
        UpdateUtilityPoles();

        if (utility_Pole == null || utility_Pole.Count == 0) return;

        RedrawWires(); // ��ġ�� ����Ǹ� ������ �ٽ� �׸�
    }

    // ���� ������Ʈ���� Utility_Pole_Repository �߰�
    void UpdateUtilityPoles()
    {
        // ���� ���� ������Ʈ�� ���� ������Ʈ���� Utility_Pole_Repository�� ã��
        Utility_Pole_Repository[] poles = GetComponentsInChildren<Utility_Pole_Repository>();

        // ����Ʈ�� ���� �����븦 �߰�
        foreach (var pole in poles)
        {
            if (!utility_Pole.Contains(pole))
            {
                utility_Pole.Add(pole);
            }
        }

        // ������������ ����
        utility_Pole.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));
    }

    // ���� �ٽ� �׸���
    void RedrawWires()
    {
        if (utility_Pole == null || utility_Pole.Count == 0) return;

        for (int i = 0; i < utility_Pole.Count - 1; i++)
        {
            CreateWireBetweenPoles(utility_Pole[i], utility_Pole[i + 1]);
        }
    }

    // �� ������ ���̿� ������ �����ϴ� �޼���
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

            // �� �β��� �����ϰ� ����
            float randomThickness = Random.Range(minWireThickness, maxWireThickness);
            lineRenderer.startWidth = randomThickness;
            lineRenderer.endWidth = randomThickness;
            lineRenderer.positionCount = segmentsPerWire;
            lineRenderer.material = wireMaterial;
            lineRenderer.useWorldSpace = true;

            Vector3 startPoint = poleA.point[i].position;
            Vector3 endPoint = poleB.point[i].position;

            // �� ���� ���� ������ �ֱ� ������ ����
            float randomSagAmount = Random.Range(minSagAmount, maxSagAmount);

            for (int j = 0; j < segmentsPerWire; j++)
            {
                float t = (float)j / (segmentsPerWire - 1);
                Vector3 currentPoint = Vector3.Lerp(startPoint, endPoint, t);

                // ���� �ֱ� ������ ���
                float sagFactor = Mathf.Sin(t * Mathf.PI) * randomSagAmount;
                currentPoint.y -= sagFactor;

                lineRenderer.SetPosition(j, currentPoint);
            }
        }
    }
}

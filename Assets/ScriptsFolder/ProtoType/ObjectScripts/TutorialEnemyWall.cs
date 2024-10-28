using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyWall : MonoBehaviour
{
    public GameObject enemy;

    private void Update()
    {
        if (!enemy.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}

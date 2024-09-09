using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBoxAttack : MonoBehaviour
{
    public BoxTestt enemyBox;
    public float damage;

    private void Awake()
    {
        enemyBox = transform.root.GetComponent<BoxTestt>();
    }

    private void Start()
    {
        damage = enemyBox.eStat.atk;
    }
}

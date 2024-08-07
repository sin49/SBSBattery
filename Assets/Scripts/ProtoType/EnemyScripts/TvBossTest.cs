using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvBossTest : MonoBehaviour
{
    EnemyStat estat;
    Rigidbody enemyRb;

    private void Awake()
    {
        estat = GetComponent<EnemyStat>();
        enemyRb= GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        estat.eState = EnemyState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

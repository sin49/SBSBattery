using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum MonsterState { idle, patrol, tracking, attack, hitted, dead}

public class EnemyStateMachine : MonoBehaviour
{
    public MonsterState mState;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        mState = MonsterState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecideState()
    {
        switch (mState)
        {
            case MonsterState.idle:
                break;
            case MonsterState.patrol:
                break;
            case MonsterState.tracking:
                break;
            case MonsterState.attack:
                break;
            case MonsterState.hitted:
                break;
            case MonsterState.dead:
                break;
            default:
                break;
        }
    }
}

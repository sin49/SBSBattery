using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResourceManager : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> AttackCollider = new List<GameObject>();
    public List<RuntimeAnimatorController> EnemyHumonoidController;
    public static EnemyResourceManager Instance;
    private void Awake()
    {
  
        if (Instance == null)
            Instance = this;
    }
}

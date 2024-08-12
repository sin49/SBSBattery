using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public Player player;
    public Enemy enemy;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void TransformEnd()
    {
        PlayerHandler.instance.formChange = false;
        Time.timeScale = 1f;
    }

    public void BoxOpend()
    {
        if (GetComponentInParent<EnemyInstantiateObject>() != null)
        {
            GetComponentInParent<EnemyInstantiateObject>().SpawnBoxEnemy();
        }
        else
        {
            GetComponentInParent<BossStageBox>().SpawnBoxEnemy();
        }
        
        transform.parent.gameObject.SetActive(false);        
    }

    public void EnemyHitted()
    {
        Material[] materials = enemy.renderer.materials;
        materials[1] = enemy.idleMat;
        enemy.renderer.materials = materials;
    }
}

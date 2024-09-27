using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public Player player;
    Enemy enemy;
    BossStageEnemy bse;

    private void Awake()
    {
        if (GetComponentInParent<Player>() != null)
        {
            player = GetComponentInParent<Player>();
        }
        else if(GetComponentInParent<Enemy>() != null)
        {
            enemy= GetComponentInParent<Enemy>();
        }
        else
        {
            bse = GetComponentInParent<BossStageEnemy>();
        }
    }

    public void TransformEnd()
    {
        PlayerHandler.instance.formChange = false;
        Time.timeScale = 1f;
    }

    public void RushEndCanHandle()
    {
        Debug.Log("돌진 준비/끝 동작 호출되었습니다");
        Player player = PlayerHandler.instance.CurrentPlayer;
        Destroy(Instantiate(player.changeEffect, player.transform.position, Quaternion.identity), 1.5f);
        PlayerHandler.instance.CantHandle = false;
    }

    public void BoxOpend()
    {
        if (GetComponentInParent<EnemyInstantiateObject>() != null)
        {
            GetComponentInParent<EnemyInstantiateObject>().SpawnBoxEnemy();
        }
        Destroy(transform.parent.gameObject);
          
    }

    public void EnemyHitted()
    {
        if (enemy != null)
        {
            CursorInteractObject cursorInteract;
            if (enemy.skinRenderer != null)
            {
                if (TryGetComponent<CursorInteractObject>(out cursorInteract))
                {
                    enemy.activeAttack = false;
                    return;
                }
                else
                {
                    Material[] materials = enemy.skinRenderer.materials;
                    materials[1] = enemy.idleMat;
                    enemy.skinRenderer.materials = materials;
                }
            }
            enemy.activeAttack = false;
        }
        else
        {
            CursorInteractObject cursorInteract;
            if (bse.skinRenderer != null)
            {
                if (TryGetComponent<CursorInteractObject>(out cursorInteract))
                {
                    bse.activeAttack = false;
                    return;
                }
                else
                {
                    Material[] materials = bse.skinRenderer.materials;
                    materials[1] = bse.idleMat;
                    bse.skinRenderer.materials = materials;
                }
            }
            bse.activeAttack = false;
        }
    }

    public void EnemyAttackEvent()
    {

    }    
}

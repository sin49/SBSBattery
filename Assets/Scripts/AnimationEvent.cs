using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public Player player;

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
        GetComponentInParent<EnemyInstantiateObject>().SpawnBoxEnemy();
        transform.parent.gameObject.SetActive(false);        
    }
}

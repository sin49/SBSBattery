using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DownAttackCollider : MeleeCollider
{
 


    private void Start()
    {
        //saveEffect = Instantiate(hitEffect).GetComponent<ParticleSystem>();
        damage = PlayerStat.instance.atk;
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("몬스터 확인");
            //DamageCollider(other);
            DamagedByPAttack script;
            if (other.TryGetComponent<DamagedByPAttack>(out script))
            {
                script.Damaged(damage);
                Debug.Log("몬스터 Damage받음");
            }
            Vector3 effectPos = new(other.transform.position.x, other.transform.position.y + .5f, other.transform.position.z);

            //saveEffect.transform.position = new(other.transform.position.x, other.transform.position.y + .5f, other.transform.position.z);
            //saveEffect.Play();
        }


        if (other.CompareTag("Ground"))
        {
            TransformPlace transformPlace;
            if (other.TryGetComponent<TransformPlace>(out transformPlace))
            {
                Debug.Log("트랜스폼오브젝트 탐지");
                transformPlace.transformStart(PlayerHandler.instance.CurrentPlayer.gameObject);
                PlayerHandler.instance.CurrentPlayer.onTransform = true;
            }
            else
            {
                BrokenPlatform brokenPlatform;
                ObjectScale ironInteract;
                if (other.TryGetComponent<BrokenPlatform>(out brokenPlatform))
                {
                    Debug.Log("부서지는 플랫폼");
                    PlayerHandler.instance.CurrentPlayer.BounceByBroeknPlatform();
                }
                else if(TryGetComponent<ObjectScale>(out ironInteract))
                {
                    return;
                }
                else
                    gameObject.SetActive(false);
            }
        }        
    }
    #region 튕김 방향
    public float DecideDirection()
    {
        float r = 0;
        switch (PlayerStat.instance.direction)
        {
            case direction.Right:
                r = -1;
                break;
            case direction.Left:
                r = 1;
                break;
        }
        return r;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class DownAttackCollider2D : MonoBehaviour
{
    public ParticleSystem hitEffect;
    public HouseholdIronTransform hosueiron;
    ParticleSystem saveEffect;
    public DownAttackCollider downattack;
    float damage;
    protected  void Start()
    {
        saveEffect = Instantiate(hitEffect).GetComponent<ParticleSystem>();
        damage = PlayerStat.instance.atk;

    }
    protected  void Update()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        {
            if (
                    (int)PlayerStat.instance.MoveState < 4)
            {
                if (other.CompareTag("Enemy"))
                {
                    Debug.Log("¸ó½ºÅÍ È®ÀÎ");
                    //DamageCollider(other);
        
                   
                        if (hosueiron!=null)
                        {
                           
                            other.transform.parent.GetComponent<Enemy>().FlatByIronDwonAttack(hosueiron.flatTime);
                            CheckMonster(other);
                        }

                   

                    saveEffect.transform.position = new(other.transform.position.x, other.transform.position.y + .5f, other.transform.position.z);
                    saveEffect.Play();
                }


                if (other.CompareTag("Ground"))
                {
                    TransformPlace transformPlace;
                    if (other.TryGetComponent<TransformPlace>(out transformPlace))
                    {
                        Debug.Log("Æ®·£½ºÆû¿ÀºêÁ§Æ® Å½Áö");
                        transformPlace.transformStart(PlayerHandler.instance.CurrentPlayer.gameObject);
                        PlayerHandler.instance.CurrentPlayer.onTransform = true;
                    }
                    else
                    {
                        BrokenPlatform brokenPlatform;
                        ObjectScale ironInteract;
                        if (other.TryGetComponent<BrokenPlatform>(out brokenPlatform))
                        {
                            Debug.Log("ºÎ¼­Áö´Â ÇÃ·§Æû");
                            PlayerHandler.instance.CurrentPlayer.BounceByBroeknPlatform();
                        }
                        else if (TryGetComponent<ObjectScale>(out ironInteract))
                        {
                            return;
                        }

                    }
                }
            }
        }
    }

    #region Æ¨±è ¹æÇâ
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

    public void CheckMonster(Collider2D other)
    {
        fireenemy fireMonster;
        if (other.TryGetComponent<fireenemy>(out fireMonster))
        {
            fireMonster.StopCoroutine();
            ParticleSystem[] effects = fireMonster.fireeffects;
            foreach (ParticleSystem fires in effects)
            {
                fires.gameObject.SetActive(false);
            }
            fireMonster.breathsmallcollider.gameObject.SetActive(false);
            fireMonster.breathcollider.gameObject.SetActive(false);
        }
    }
}

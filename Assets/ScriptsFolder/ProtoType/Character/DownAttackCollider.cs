using UnityEngine;

public class DownAttackCollider : MeleeCollider
{
 


    private void Start()
    {

        saveEffect = Instantiate(hitEffect).GetComponent<ParticleSystem>();
        damage = PlayerStat.instance.atk;
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter(Collider other)
    {        
        
        if (other.CompareTag("Enemy"))
        {
            DamageCollider(other);
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

                gameObject.SetActive(false);
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
}

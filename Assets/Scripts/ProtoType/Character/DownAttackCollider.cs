using UnityEngine;

public class DownAttackCollider : MeleeCollider
{
 

    Player player;
    Rigidbody playerRb;

    private void Start()
    {
        player = transform.parent.parent.GetComponent<Player>();
        playerRb = player.GetComponent<Rigidbody>();
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
                PlayerHandler.instance.CurrentPlayer.onTransform = true;
            }

            gameObject.SetActive(false);
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

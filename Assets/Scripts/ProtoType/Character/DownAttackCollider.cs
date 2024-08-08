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
                Debug.Log("Ʈ������������Ʈ Ž��");
                PlayerHandler.instance.CurrentPlayer.onTransform = true;
            }

            gameObject.SetActive(false);
        }        
    }

    #region ƨ�� ����
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

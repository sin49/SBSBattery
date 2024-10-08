using UnityEngine;

public class DownAttackCollider : MeleeCollider
{

    private void Start()
    {
        saveEffect = Instantiate(hitEffect).GetComponent<ParticleSystem>();
        damage = PlayerStat.instance.atk;
        gameObject.SetActive(false);
    }
    protected override void Update()
    {
        
    }
    public void DeactiveCollider()
    {
        this.gameObject.SetActive(false);
    }
    protected override void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("���� Ȯ��");
            //DamageCollider(other);
            DamagedByPAttack script;
            if (other.TryGetComponent<DamagedByPAttack>(out script))
            {
                if (GetComponentInParent<HouseholdIronTransform>())
                {                    
                    HouseholdIronTransform iron = GetComponentInParent<HouseholdIronTransform>();
                    other.GetComponent<Enemy>().FlatByIronDwonAttack(iron.flatTime);
                    CheckMonster(other);
                }
                script.Damaged(damage);
                Debug.Log("���� Damage����");
            }

            saveEffect.transform.position = new(other.transform.position.x, other.transform.position.y + .5f, other.transform.position.z);
            saveEffect.Play();
        }


        if (other.CompareTag("Ground"))
        {
            TransformPlace transformPlace;
            if (other.TryGetComponent<TransformPlace>(out transformPlace))
            {
                Debug.Log("Ʈ������������Ʈ Ž��");
                transformPlace.transformStart(PlayerHandler.instance.CurrentPlayer.gameObject);
                PlayerHandler.instance.CurrentPlayer.onTransform = true;
            }
            else
            {
                BrokenPlatform brokenPlatform;
                ObjectScale ironInteract;
                if (other.TryGetComponent<BrokenPlatform>(out brokenPlatform))
                {
                    Debug.Log("�μ����� �÷���");
                    PlayerHandler.instance.CurrentPlayer.BounceByBroeknPlatform();
                }
                else if(TryGetComponent<ObjectScale>(out ironInteract))
                {
                    return;
                }
            
            }
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

    public void CheckMonster(Collider other)
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

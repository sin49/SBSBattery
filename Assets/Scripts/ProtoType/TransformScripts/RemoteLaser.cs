using UnityEngine;

public class RemoteLaser : PlayerAttack
{

    public float rangeSpeed;
    public GameObject hitEffect;
    public ParticleSystem saveEffect;
    public float laserTime = 5;
    
    // Start is called before the first frame update
  
    private void Start()
    {
        damage = PlayerStat.instance.atk;
    }
  
    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Translate(transform.forward * rangeSpeed * Time.fixedDeltaTime, Space.World);
        if (laserTime > 0)
            laserTime -= Time.fixedDeltaTime;
        else
            DestroyLaser();        
    }
    void DestroyLaser()
    {
        if (PoolingManager.instance != null)
        {
            laserTime = 5;
            PoolingManager.instance.ReturnPoolObject(this.gameObject);
        }
        else
            Destroy(gameObject);
    }
    public override void DamageCollider(Collider other)
    {
        base.DamageCollider(other);

        if (saveEffect != null)
        {
            saveEffect.transform.position = new(other.transform.position.x, other.transform.position.y + .5f, other.transform.position.z);
            saveEffect.Play();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Ground"))
        {
            DestroyLaser();
        }
    }
}

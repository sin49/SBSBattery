using UnityEngine;

public class RemoteLaser : PlayerAttack
{

    public float rangeSpeed;
    public GameObject hitEffect;
    public ParticleSystem saveEffect;

    // Start is called before the first frame update
  
    private void Start()
    {
        damage = PlayerStat.instance.atk;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * rangeSpeed * Time.deltaTime, Space.World);
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

   
    private void OnBecameInvisible()
    {
        if (PoolingManager.instance != null)
            PoolingManager.instance.ReturnPoolObject(this.gameObject);
        else
            Destroy(gameObject);
    }
   
}

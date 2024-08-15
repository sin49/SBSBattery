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
        Debug.Log("콜라이딩");
        if (saveEffect != null)
        {
            saveEffect.transform.position = new(other.transform.position.x, other.transform.position.y + .5f, other.transform.position.z);
            saveEffect.Play();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Debug.Log("콜라이딩" + other.name);
    }
    private void OnBecameInvisible()
    {
        if (PoolingManager.instance != null)
            PoolingManager.instance.ReturnPoolObject(this.gameObject);
        else
            Destroy(gameObject);
    }
   
}

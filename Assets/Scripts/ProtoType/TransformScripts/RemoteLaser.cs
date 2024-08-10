using UnityEngine;

public class RemoteLaser : PlayerAttack
{

    public float rangeSpeed;
    public GameObject hitEffect;
    public ParticleSystem saveEffect;

    // Start is called before the first frame update
    void Awake()
    {
        //saveEffect = Instantiate(hitEffect).GetComponent<ParticleSystem>();
        
        //gameObject.SetActive(false);
    }

    private void Start()
    {
        damage = PlayerStat.instance.atk;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * rangeSpeed * Time.deltaTime, Space.World);
    }

 

    private void OnBecameInvisible()
    {
        if (PoolingManager.instance != null)
            PoolingManager.instance.ReturnPoolObject(this.gameObject);
        else
            Destroy(gameObject);
    }
}

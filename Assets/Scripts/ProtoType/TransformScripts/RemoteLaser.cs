using UnityEngine;

public class RemoteLaser : MonoBehaviour
{
    public float damage;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (!enemy.eStat.onInvincible)
            {
                enemy.Damaged(damage);
                Instantiate(saveEffect, new(enemy.transform.position.x + 0.1f, enemy.transform.position.y+0.25f, enemy.transform.position.z), Quaternion.identity);
                /*saveEffect.transform.position = other.transform.position;
                saveEffect.Play();*/
                if (PoolingManager.instance != null)
                    PoolingManager.instance.ReturnPoolObject(this.gameObject);
                else
                    Destroy(gameObject);
            }
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

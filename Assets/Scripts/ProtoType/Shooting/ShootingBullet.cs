using UnityEngine;

public class ShootingBullet : MonoBehaviour
{
    public bool Player;
    public float speed;
    public float lifetime=999;
    float lifetimer;
    Vector2 Vector;

    public float snappoint = 0.6f;

    public void Setbullet(float speed,Vector3 vector,float lifetime,bool Player)
    {
        this.lifetime = lifetime;
        this.speed = speed;
        this.Vector = vector;
        this.Player = Player;
    }
    void DestroyDisable()
    {
        //if (this.transform.localPosition.x > ShootingFIeld.instance.MaxSizeX + snappoint || this.transform.localPosition.x < ShootingFIeld.instance.MinSizeX - snappoint ||
        //    this.transform.localPosition.y > ShootingFIeld.instance.MaxSizeY + snappoint || this.transform.localPosition.y < ShootingFIeld.instance.MinSizeY - snappoint)
        //    Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        //this.transform.position = new Vector3(transform.position.x, transform.position.y, ShootingPlayer.instance.transform.position.z);
        transform.Translate(Vector * speed * Time.deltaTime);
        lifetimer += Time.deltaTime;
        if (lifetimer >= lifetime)
        {
            Destroy(gameObject);
        }
        //DestroyDisable();
       
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if ((Player && collision.CompareTag("Enemy")) || (!Player && collision.CompareTag("Player")))
        {
            collision.GetComponent<ShootingObject>().hitted();
           gameObject.SetActive(false);
        }
    }
   
}

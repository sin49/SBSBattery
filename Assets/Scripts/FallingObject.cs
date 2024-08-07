using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public int damage;
    Rigidbody rb;
    public float fallingwaitingtime;

    // Start is called before the first frame update
    void Start()
    { 
        rb = GetComponent<Rigidbody>();
        StartCoroutine(FallingWaitingTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHandler.instance.CurrentPlayer.Damaged(damage);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator FallingWaitingTime()
    {
        yield return new WaitForSeconds(fallingwaitingtime);

        rb.useGravity = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderColliderCheck : MonoBehaviour
{
    public Ladder ladder;
    public Transform endPoint;

    private void Awake()
    {
        ladder = GetComponentInParent<Ladder>();
        endPoint = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerHandler.instance.ladderInteract)
            {
                other.transform.position = endPoint.position;

                PlayerHandler.instance.ladderInteract = false;
                PlayerHandler.instance.CurrentPlayer.GetComponent<Rigidbody>().useGravity = true;
            }
        }

    }
}

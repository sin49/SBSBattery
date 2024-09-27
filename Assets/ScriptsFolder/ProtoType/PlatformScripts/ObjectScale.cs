using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScale : MonoBehaviour
{
    public GameObject parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            if (!PlayerHandler.instance.CurrentPlayer.GetComponent<HouseholdIronTransform>())
            {
                Debug.Log("�ٸ��� �ƴ�");
            }
            else
            {
                if(parent != null)
                    parent.transform.localScale = new Vector3(1, 0.1f, 1);
                other.gameObject.SetActive(false);
            }
        }
    }
}

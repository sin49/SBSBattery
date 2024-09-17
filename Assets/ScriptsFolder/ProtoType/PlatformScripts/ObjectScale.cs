using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScale : MonoBehaviour
{
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
                this.transform.localScale = new Vector3(1, 0.1f, 1);
                other.gameObject.SetActive(false);
            }
        }
    }
}

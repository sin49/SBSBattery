using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnable : MonoBehaviour
{
    public float distance;
    public float rangeValue;

    private void Update()
    {
        PlayerCheck();
    }

    public void PlayerCheck()
    {
        if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentPlayer != null)
        {
            distance = Vector3.Distance(transform.position, PlayerHandler.instance.CurrentPlayer.transform.position);

            if (distance < rangeValue)
                transform.GetChild(0).gameObject.SetActive(true);
            else
                transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}

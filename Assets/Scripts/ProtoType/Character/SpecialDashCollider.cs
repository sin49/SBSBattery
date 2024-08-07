using System.Collections;
using UnityEngine;

public class SpecialDashCollider : MonoBehaviour
{
    public State cState;

    private void OnEnable()
    {
        StartCoroutine(WaitAndActiveFalse());
    }

    IEnumerator WaitAndActiveFalse()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            EnemyStat enemyStat = other.GetComponent<Enemy>().eStat;

            if (enemyStat.characterState == cState)
            {
                enemy.Damaged(PlayerStat.instance.atk * 3, other.gameObject);
                Debug.Log($"다리미 대쉬 대미지>> {PlayerStat.instance.atk * 3}");
            }
            else
            {
                Debug.Log("지나갑니다");
            }
        }        
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    /*public void Dash()
    {
        if (!onDash)
        {
            Debug.Log("��� ��Ÿ�� ���Դϴ�");
        }
        else
        {
            onInvincible = true;
            onDash = false;
            gameObject.layer = 6;
            //playerRb.AddForce(Vector3.right * PlayerStat.instance.dashForce, ForceMode.Impulse);

            //IronDash.SetActive(true);

            if (Input.GetKey(KeyCode.RightArrow))
            {
                playerRb.AddForce(transform.forward * PlayerStat.instance.dashForce, ForceMode.Impulse);
                Debug.Log("ª�� �뽬 �Է� ����");
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                playerRb.AddForce(transform.forward * PlayerStat.instance.dashForce, ForceMode.Impulse);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                playerRb.AddForce(transform.forward * PlayerStat.instance.dashForce, ForceMode.Impulse);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                playerRb.AddForce(transform.forward * PlayerStat.instance.dashForce, ForceMode.Impulse);
            }

            velocityValue = playerRb.velocity;
            Debug.Log("��������� �۵��ϳ�");
            StartCoroutine(WaitCoolTime());
        }
    }*/

    /*IEnumerator WaitCoolTime()
    {
        Debug.Log("��� ���� ���Դϴ�");
        playerRb.useGravity = false;

        yield return new WaitForSeconds(PlayerStat.instance.dashTimer);

        gameObject.layer = 0;
        playerRb.velocity = Vector3.zero;
        onInvincible = false;
        playerRb.useGravity = true;

        yield return new WaitForSeconds(PlayerStat.instance.dashCoolTime);

        onDash = true;
        Debug.Log("��� ��Ÿ�� �Ϸ�");
    }*/
}

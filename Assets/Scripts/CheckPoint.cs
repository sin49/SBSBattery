using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Transform ChkPointTestPrefab;
    private void Awake()
    {
        ChkPointTestPrefab.gameObject.SetActive(false);
    }
    public int index;
    public GameObject spawn(GameObject obj)
    {
        var player= Instantiate(obj, ChkPointTestPrefab.position, ChkPointTestPrefab.rotation);
     PlayerHandler.instance.   registerRemoteUI(player);
        return player;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"üũ����Ʈ{index}�� ����");
            PlayerSpawnManager.Instance.ChangeCheckPoint(this);
        }
    }
}

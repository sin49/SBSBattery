using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreater : MonoBehaviour
{
    public GameObject EditorObject;

    public ItemObject itemObj;
 [Header("�´� �������� �ƴϸ� �۵� ���� �� ��")]
    public item itemdata;
    private void Awake()
    {
        EditorObject.SetActive(false);
        itemObj.GetITemData(itemdata);
    }
}

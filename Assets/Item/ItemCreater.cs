using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreater : MonoBehaviour
{
    public GameObject EditorObject;

    public ItemObject itemObj;
 [Header("맞는 아이템이 아니면 작동 정상 안 됨")]
    public item itemdata;
    private void Awake()
    {
        EditorObject.SetActive(false);
        itemObj.GetITemData(itemdata);
    }
}

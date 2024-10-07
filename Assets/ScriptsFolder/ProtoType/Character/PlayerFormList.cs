using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
public class PlayerFormList : MonoBehaviour
{
    public List<GameObject> playerformlist = new List<GameObject>();

    public List<TransformPlace> PlayerFormObject = new List<TransformPlace>();
    public void CheckList()
    {
        //if (playerformlist.Count > 0)
        //{
        //    if (playerformlist[playerformlist.Count - 1].GetComponent<Playerform>() == null)
        //        playerformlist.RemoveAt(playerformlist.Count - 1);
        //}
    }

    private void OnValidate()
    {
        CheckList();
    }
   
}
public interface FormInterface
{

}

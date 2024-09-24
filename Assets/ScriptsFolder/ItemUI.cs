using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemUI : MonoBehaviour
{
 public   TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    Animator ani;
    public float itemUITime = 3f;
   float itemtimer;
    private void Awake()
    {
        ani = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        itemtimer = itemUITime;
    }
    private void Update()
    {
        //ani.SetFloat("itemtimer", itemtimer);
        if (itemtimer >= 0)
            itemtimer -= Time.deltaTime;
        else
            gameObject.SetActive(false);
    }
    public void activeUI(item i)
    {
        Title.text = i.itemname;
        Description.text = i.itemdescription;
        this.gameObject.SetActive(true);
        //ani.Play("Create");
    }
}

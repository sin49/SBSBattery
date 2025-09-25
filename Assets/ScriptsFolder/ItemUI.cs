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
        if(LanguageManager.instance != null)
            LanguageCheck();
        this.gameObject.SetActive(true);
        //ani.Play("Create");
    }

    public void LanguageCheck()
    {
        if (!LanguageManager.instance.isKor)
        {
            if (Description.text.Contains("토큰"))
            {
                Description.text = "Token acquired!";
            }
            else if (Description.text.Contains("체력"))
            {
                Description.text = "Max HP increased!";
            }
            else if (Description.text.Contains("스피드"))
            {
                Description.text = "Speed increased!!";
            }
        }
    }
}

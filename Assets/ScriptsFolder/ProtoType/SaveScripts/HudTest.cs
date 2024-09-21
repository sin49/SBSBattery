using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudTest : MonoBehaviour
{
    public static HudTest instance;

    public RectTransform gaugeBar;
    public RectTransform hpSettingGroup;
    public List<GameObject> hpGroup;

    public GameObject hpPrefab;
    public Player currentPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {        
        //PlayerHpSetting();
        gaugeBar.localScale = new Vector3(0, 1, 1);
        gameObject.SetActive(false);
    }    

    public void GaugeCheck(float value)
    {
        gaugeBar.localScale = new Vector3(value, 1, 1);
    }


    public void ActiveGauge(Vector3 pos)
    {
        Vector3 fixPos = Camera.main.WorldToViewportPoint(pos);
        transform.position = Camera.main.ViewportToScreenPoint(fixPos);

        gameObject.SetActive(true);
    }

    /*public void PlayerHpSetting()
    {
        for (int i = 0; i < PlayerStat.instance.hpMax; i++)
        {
            Instantiate(hpPrefab, hpSettingGroup);
            hpGroup.Add(hpPrefab);
        }
    }*/

    /*public void InitHpState(float currentHP)
    {
        for (int i = (int)PlayerStat.instance.hpMax; i > (int)currentHP; i--)
        {
            hpSettingGroup.GetChild(i - 1).gameObject.SetActive(false);
        }
    }*/
}

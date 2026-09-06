using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerText : MonoBehaviour
{
    TextMeshProUGUI text;
    private void Awake()
    {
        text = this.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //text.text = "충전에너지:" + PlayerHandler.instance.CurrentPower +"/"+ PlayerHandler.instance.MaxPower;
        text.text = "Power:" + PlayerHandler.instance.CurrentPower + "/" + PlayerHandler.instance.MaxPower;
    }
}

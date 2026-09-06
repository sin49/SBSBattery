using TMPro;
using UnityEngine;

public class TransformText : MonoBehaviour
{
    TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    string ReturnTypeString(TransformType type)
    {
        /*switch (type)
        {
            case TransformType.transform0:
                return "소인";
            case TransformType.transform1:
                return "거인";
            default:
                return "없음";
        }*/
        switch (type)
        {
       
            case TransformType.transform1:
                return "NanoDrone";
            default:
                return "없음";
        }
    }
    // Update is called once per frame
    void Update()
    {

        /*text.text = "저장된 변신: " + ReturnTypeString(PlayerHandler.instance.retoretype)
            + " 현재 변신: " + ReturnTypeString(PlayerHandler.instance.CurrentType);*/

        text.text = "Saved Transform: " + ReturnTypeString(PlayerHandler.instance.retoretype)
            + "Current Transform: " + ReturnTypeString(PlayerHandler.instance.CurrentType);
    }
}

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
                return "����";
            case TransformType.transform1:
                return "����";
            default:
                return "����";
        }*/
        switch (type)
        {
       
            case TransformType.transform1:
                return "NanoDrone";
            default:
                return "����";
        }
    }
    // Update is called once per frame
    void Update()
    {

        /*text.text = "����� ����: " + ReturnTypeString(PlayerHandler.instance.retoretype)
            + " ���� ����: " + ReturnTypeString(PlayerHandler.instance.CurrentType);*/

        text.text = "Saved Transform: " + ReturnTypeString(PlayerHandler.instance.retoretype)
            + "Current Transform: " + ReturnTypeString(PlayerHandler.instance.CurrentType);
    }
}

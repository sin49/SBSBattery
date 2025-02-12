using UnityEngine;

public class BackgroundCapture : MonoBehaviour
{
    Material bgMat;
    public Texture2D mapTwoD, mapThreeD;
    public Vector3 posThreeD;

    private void Awake()
    {
        bgMat = GetComponent<MeshRenderer>().materials[0];
        bgMat.SetTexture("_BaseMap", mapTwoD);
    }

    private void Start()
    {
        ResisterChange();
    }

    public void ChangeBackground()
    {
        
        bgMat.SetTexture("_BaseMap", mapThreeD);
        transform.localScale = new(90, 60, 1);
        transform.position = posThreeD;
    }

    public void ResisterChange()
    {
        GameManager.instance.ResisterAction(ChangeBackground);
    }
}

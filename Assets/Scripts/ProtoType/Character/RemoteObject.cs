using UnityEngine;

public abstract class RemoteObject : MonoBehaviour
{
    public bool onActive;
    public bool CanControl = true;
    //UI표시 위치
    public GameObject HudTarget;
    public abstract void Active();

    public abstract void Deactive();


}
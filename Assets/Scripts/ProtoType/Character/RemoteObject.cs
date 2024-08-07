using UnityEngine;

public abstract class RemoteObject : MonoBehaviour
{
    public bool onActive;
    public bool CanControl = true;    

    public abstract void Active();

    public abstract void Deactive();


}
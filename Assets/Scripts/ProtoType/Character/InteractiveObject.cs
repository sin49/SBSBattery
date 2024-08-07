using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    protected direction direct;
    public InteractOption InteractOption;
    public abstract void Active(direction direct);

    }
public enum InteractOption {ray,collider }
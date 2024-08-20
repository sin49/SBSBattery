using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColliderDisplayManager : MonoBehaviour
{
        public static ColliderDisplayManager Instance;
    HashSet<colliderDisplayer> displayers= new HashSet<colliderDisplayer>();
    private void Awake()
    {
        Instance = this;
    }
    public void register(colliderDisplayer displayer)
    {
        displayers.Add(displayer);
    }
    public void ActiveCollderDisplay()
    {
        foreach (var displayer in displayers)
        {
            displayer.ActiveColliderDisplay();
        }
    }
    public void DeactiveColliderDisplay()
    {
        foreach (var displayer in displayers)
        {
            displayer.DeactiveColliderDisplay();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : signalReceiver
{
    public Material OffMaterial;
    public Material OnMaterial;
    public Light LampLight;
    public MeshRenderer meshrenderer;


    void lampOn()
    {
        meshrenderer.material = OnMaterial;
        LampLight.gameObject.SetActive(true);
    }
    void lampoff()
    {
        meshrenderer.material = OffMaterial;
        LampLight.gameObject.SetActive(false);
    }
  
    private void Update()
    {
        if (active)
            lampOn();
        else
            lampoff();
    }
    // Update is called once per frame
    protected override void Awake()
    {
        base.Awake();
        meshrenderer = GetComponent<MeshRenderer>();
        register();
        

    }
}

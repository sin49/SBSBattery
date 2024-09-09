using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalTv : signalReceiver
{
    public TvColor tvColor = TvColor.white;

    public float distanceToRemocon;

    public Material[] tvMaterials;
    public Material ActiveMaterial;
    public Material DeactiveMaterial;

    public Light tvLight;
    public BoxCollider activeCollider;

    bool onViewPort;

    public bool done;

    protected override void Awake()
    {
        base.Awake();
        tvMaterials = new Material[transform.parent.GetComponent<MeshRenderer>().materials.Length];
        tvMaterials = transform.parent.GetComponent<MeshRenderer>().materials;
        register();
        Deactive();
    }

    private void Update()
    {
        if (active && !done)
        {
            done = true;
            Active();
        }

    }

    public void Deactive()
    {
        tvMaterials[1] = DeactiveMaterial;
        transform.parent.GetComponent<MeshRenderer>().materials = tvMaterials;

        activeCollider.enabled = false;
        tvLight.enabled = false;
    }
    public void Active()
    {

        tvMaterials[1] = ActiveMaterial;
        transform.parent.GetComponent<MeshRenderer>().materials = tvMaterials;
        
        activeCollider.enabled = true;
        tvLight.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteTV : RemoteObject
{
    public TvColor tvColor = TvColor.white;

    public float distanceToRemocon;
    
    public Material[] tvMaterials;
    public Material ActiveMaterial;
    public Material DeactiveMaterial;
    public GameObject FrontOBj;
    public Light tvLight;
    BoxCollider activeCollider;

    bool onViewPort;

    private void Awake()
    {
        tvMaterials = new Material[GetComponent<MeshRenderer>().materials.Length];
        tvMaterials = GetComponent<MeshRenderer>().materials;
        tvLight = transform.GetComponentInChildren<Light>();
        activeCollider = transform.GetChild(1).GetComponent<BoxCollider>();
        tvLight.enabled = onActive;
    }
    void Start()
    {
        //Frontrenderer=FrontOBj.GetComponent<MeshRenderer>();         
        Deactive();
    }

    private void Update()
    {
        if (onViewPort && PlayerHandler.instance.CurrentPlayer!=null)
        {
             distanceToRemocon = Vector3.Distance(transform.position, PlayerHandler.instance.CurrentPlayer.transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (onActive)
        {
            tvMaterials[1] = ActiveMaterial;

            activeCollider.enabled = onActive;
            tvLight.enabled = onActive;
        }
        else
        {            
            tvMaterials[1] = DeactiveMaterial;
            
            activeCollider.enabled = onActive;
            tvLight.enabled = onActive;
        }
    }

    public override void Deactive()
    {
        //GetComponent<MeshRenderer>().materials[1] = DeactiveMaterial;
        tvMaterials[1] = DeactiveMaterial;
        GetComponent<MeshRenderer>().materials = tvMaterials;

        //Frontrenderer.material = DeactiveMaterial;
        onActive = false;
        activeCollider.enabled = onActive;
        tvLight.enabled = onActive;
    }
    public override void Active() { 

        tvMaterials[1] = ActiveMaterial;
        GetComponent<MeshRenderer>().materials = tvMaterials;

        onActive = true;
        activeCollider.enabled = onActive;
        tvLight.enabled = onActive;
    }

    private void OnBecameVisible()
    {        
        if (PlayerHandler.instance.CurrentType == TransformType.remoteform)
        {
            //distanceToRemocon = Vector3.Distance(this.transform.position, PlayerHandler.instance.CurrentPlayer.transform.position);
            onViewPort = true;
        }
    }

    private void OnBecameInvisible()
    {
        if (PlayerHandler.instance.CurrentType == TransformType.remoteform)
        {
            onViewPort = false;
        }
    }    
}

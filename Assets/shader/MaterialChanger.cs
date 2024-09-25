using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [Header("���͸��� ü����")]
  public List<Material> materials = new List<Material>();

    public Renderer rend;

    

    void Start()
    {
        if (rend == null)
            rend = this.GetComponent<Renderer>();

    }

   public void CjangeMaterial(int n)
    {
        if(materials.Count<=n)
            n=materials.Count-1;

        rend.material = materials[n];
    }
}

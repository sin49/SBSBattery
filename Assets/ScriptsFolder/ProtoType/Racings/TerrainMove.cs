using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMove : MonoBehaviour
{
  
    public float speed;
    public racingTerrain Terrain;
    public Vector3 DebugPos;

    racingTerrain CurrentTerrain;
    private void Awake()
    {
        Terrain.speed = speed;
        CurrentTerrain = Terrain;
    }
    void CreateTerrain()
    {
       var obj= Instantiate(Terrain.gameObject, CurrentTerrain.transform.position+Vector3.forward*50, Quaternion.identity);
        obj.SetActive(true);
        CurrentTerrain = obj.GetComponent<racingTerrain>();
    }
    // Update is called once per frame
    void Update()
    {
      
            if (CurrentTerrain.transform.position.z <= 0)
            {
                CreateTerrain();
            }

        DebugPos = CurrentTerrain.transform.position;
    }
}

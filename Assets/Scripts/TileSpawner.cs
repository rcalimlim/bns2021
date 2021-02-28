using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public int numOfInitialTiles = 10;
    public GameObject enemy;
    public GameObject health;
    public GameObject buff;

    
    float maxRand = 5;

    void Start() {

        List<Vector3> usedTiles = new List<Vector3>();
        usedTiles.Add(Vector3.zero);
        for(int i=0; i < numOfInitialTiles; i++)
        {   
            Vector3 newTile = new Vector3(Mathf.Floor(Random.Range(-3f, 3f)), Mathf.Floor(Random.Range(-3f, 3f)), 0f);
            while(usedTiles.Contains(newTile))
                newTile = new Vector3(Mathf.Floor(Random.Range(-3f, 3f)), Mathf.Floor(Random.Range(-3f, 3f)), 0f);
            
            usedTiles.Add(newTile);
            spawnTile(newTile);
        }
    }

    public void spawnTile(Vector3 position)
    {
        float num = Mathf.Floor(Random.Range(0f, maxRand)) % maxRand ;
        switch(num)
        {
            case 0f:
            case 2f:
                Instantiate(enemy, position, Quaternion.identity);
                break;
            case 3f:
                Instantiate(health, position, Quaternion.identity);
                break;
            case 4f:
                Instantiate(buff, position, Quaternion.identity);
                break;
            default:
                break;
        }  
    }

}

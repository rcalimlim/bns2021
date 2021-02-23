using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject health;
    public GameObject buff;
    
    float maxRand = 5;


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

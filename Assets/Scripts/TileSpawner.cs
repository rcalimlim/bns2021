using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public int numOfInitialTiles = 10;
    public int turnsUntilScouter = 42;
    public GameObject enemy;
    public GameObject health;
    public GameObject buff;
    public GameObject scouter;

    public bool allowNewTileSpawns = true;
    bool scouterSpawned = false;

    
    float maxRand = 5;

    void Start() {
        if(allowNewTileSpawns)
        {
            List<Vector3> usedTiles = new List<Vector3>();
            usedTiles.Add(Vector3.zero);
            for(int i=0; i < numOfInitialTiles; i++)
            {   
                Vector3 newTile = new Vector3(Mathf.Floor(Random.Range(-3f, 3f)), Mathf.Floor(Random.Range(-3f, 3f)), 0f);
                while(usedTiles.Contains(newTile))
                    newTile = new Vector3(Mathf.Floor(Random.Range(-3f, 3f)), Mathf.Floor(Random.Range(-3f, 3f)), 0f);
                
                usedTiles.Add(newTile);
                spawnTile(newTile, 0);
            }
        }
        
    }

    public void spawnTile(Vector3 position, int turnCount)
    {
        if(allowNewTileSpawns)
        {
            float num = Mathf.Floor(Random.Range(0f, maxRand)) % maxRand ;

            if(!scouterSpawned &&  turnCount >= turnsUntilScouter && num == 0f)
            {
                if(position.y > 2)
                    position += Vector3.down;
                else if(position.y < -2)
                    position += Vector3.up;
                else if(position.x > 2)
                    position += Vector3.left;
                else if(position.x < -2)
                    position += Vector3.right;

                Instantiate(scouter, position, Quaternion.identity);
                scouterSpawned = true;
            }
                
            else if(num == 0f || num == 2f)
                Instantiate(enemy, position, Quaternion.identity);
            else if(num == 3f)
                Instantiate(health, position, Quaternion.identity);
            else if(num == 4f)
                Instantiate(buff, position, Quaternion.identity);           
        }
    }

}

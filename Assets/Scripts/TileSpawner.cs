using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject health;
    public GameObject buff;


    public void spawnTile(Vector3 position)
    {
        float num = Random.Range(0f, 3f);
        if(Mathf.Floor(num) % 3f == 0f)
            Instantiate(enemy, position, Quaternion.identity);
        else if(Mathf.Floor(num) % 3f == 1f)
            Instantiate(health, position, Quaternion.identity);
        else if (Mathf.Floor(num) % 3f == 2f)
            Instantiate(buff, position, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject weakEnemy;
    public GameObject weakBuff;
    public GameObject weakWeapon;


    public void spawnTile(Vector3 position)
    {
        float num = Random.Range(0f, 3f);
        if(Mathf.Floor(num) % 3f == 0f)
            Instantiate(weakEnemy, position, Quaternion.identity);
        else if(Mathf.Floor(num) % 3f == 1f)
            Instantiate(weakBuff, position, Quaternion.identity);
        else if (Mathf.Floor(num) % 3f == 2f)
            Instantiate(weakWeapon, position, Quaternion.identity);
    }
}

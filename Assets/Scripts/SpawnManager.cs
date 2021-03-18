using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update   
    void Start()
    {
        string spawnDoor = PlayerDataManager.Instance.SpawnDoor;
        GameObject player = GameObject.FindWithTag("Player");

        if(spawnDoor != "")
        {
            GameObject door = GameObject.Find(spawnDoor);
            player.transform.position = door.transform.position;
        }
        else
        {
            Vector3 loadAt = PlayerDataManager.Instance.LoadAtPos;
            player.transform.position = loadAt;
        }
    }


}

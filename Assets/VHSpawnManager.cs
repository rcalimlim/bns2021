using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSpawnManager : MonoBehaviour
{
    // list doors
    [SerializeField] private GameObject player;

    private void Start()
    {
        string prevScene = PlayerDataManager.Instance.PrevScene;
        string currScene = PlayerDataManager.Instance.CurrScene;
        string spawnDoor = PlayerDataManager.Instance.SpawnDoor;

        switch ((prevScene, currScene))
        {
            case ("Battle", "VirginiaHouse"):
                SpawnPlayerAt(PlayerDataManager.Instance.LoadAtPos);
                break;
                
            default:
                break;
        }
    }

    private void SpawnPlayerWithOffset(GameObject door, Vector3 offset)
    {
        player.gameObject.transform.position = door.transform.position + offset;
    }

    private void SpawnPlayerAt(Vector3 location)
    {
        player.gameObject.transform.position = location;
    }
}

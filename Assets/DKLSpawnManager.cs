using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DKLSpawnManager : MonoBehaviour
{
    // list doors
    [SerializeField]
    private GameObject bbDoor0;
    [SerializeField]
    private GameObject mbDoor0;
    [SerializeField]
    private GameObject obDoor0;

    private void Start()
    {
        string prevScene = PlayerDataManager.Instance.PrevScene;
        string currScene = PlayerDataManager.Instance.CurrScene;
        string spawnDoor = PlayerDataManager.Instance.SpawnDoor;

        switch ((prevScene, currScene))
        {
            case ("BryceBedroom", "DiningKitchenLiving"):
                SpawnPlayerWithOffset(bbDoor0, new Vector3(0f, 2f, 0f));
                break;

            case ("MarlonBedroom", "DiningKitchenLiving"):
                SpawnPlayerWithOffset(mbDoor0, new Vector3(0f, 2f, 0f));
                break;

            case ("OliverBedroom", "DiningKitchenLiving"):
                SpawnPlayerWithOffset(obDoor0, new Vector3(-1.5f, 0.5f, 0f));
                break;
                
            default:
                break;
        }
    }

    private void SpawnPlayerWithOffset(GameObject door, Vector3 offset)
    {
        GameObject.FindWithTag("Player").transform.position = door.transform.position + offset;
    }
}

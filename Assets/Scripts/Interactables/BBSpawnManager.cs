using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBSpawnManager : MonoBehaviour
{
    // list doors
    [SerializeField]
    private GameObject bbDoor0;
    [SerializeField]
    private GameObject bbDoor1;

    private void Start()
    {
        string prevScene = PlayerDataManager.Instance.PrevScene;
        string currScene = PlayerDataManager.Instance.CurrScene;
        string spawnDoor = PlayerDataManager.Instance.SpawnDoor;

        switch ((prevScene, currScene))
        {
            case ("DiningKitchenLiving", "BryceBedroom"):
                SpawnPlayerWithOffset(bbDoor0, new Vector3(0f, -0.5f, 0f));
                break;

            case ("BNS2019+", "BryceBedroom"):
                SpawnPlayerWithOffset(bbDoor1, new Vector3(-1.5f, 0.5f, 0f));
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

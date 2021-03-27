using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBSpawnManager : MonoBehaviour
{
    // list doors
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bbDoor0;
    [SerializeField] private GameObject bbDoor1;
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private Dialog dialogOnRevive;
    [SerializeField] private FreeRoamStressBar stressBar;
    [SerializeField] private AudioClip backgroundMusic;

    private void Start()
    {
        if (PlayerDataManager.Instance.IsDead() == true || PlayerDataManager.Instance.HasDied == true)
        {
            SoundManager.Instance.PlayMusic(backgroundMusic, true);
            PlayerDataManager.Instance.ResetStress();
            PlayerDataManager.Instance.HasDied = false;
            stressBar.SetStress(0);
            SpawnPlayerWithOffset(respawnPoint, new Vector3(0f, 0.5f, 0f));
            StartCoroutine(DialogOnRevive());
        }
        else
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

                case ("Battle", "BryceBedroom"):
                    SpawnPlayerAt(PlayerDataManager.Instance.LoadAtPos);
                    break;
                    
                default:
                    break;
            }
        }
    }

    private IEnumerator DialogOnRevive()
    {
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialogOnRevive));
        while (GameController.Instance.State == GameState.Dialog)
        {
            yield return null;
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

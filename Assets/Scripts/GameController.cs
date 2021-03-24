using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Battle, Menu, Respawning }
public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    private GameState state = GameState.FreeRoam;
    private PlayerController playerController;
    [SerializeField] Inventory playerInventory;
    [SerializeField] Menu pauseMenu;

    public GameState State { get { return state; } }

    private void Awake()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }
    }
    void Start()
    {
        DialogManager.Instance.OnShowDialog += () => state = GameState.Dialog;
        DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
            }
        };
        pauseMenu.OnOpenMenu += () => state = GameState.Menu;
        pauseMenu.OnCloseMenu += () => {
            if(state == GameState.Menu)
                state = GameState.FreeRoam;
        };

        RespawnManager.Instance.OnDeath += () => state = GameState.Respawning;
        RespawnManager.Instance.OnRevive += () => state = GameState.FreeRoam;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        } 
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        } 
        else if (state == GameState.Battle)
        {
           // ...
        }    
        else if (state == GameState.Menu)
        {
            pauseMenu.HandleUpdate();
        }    
        else if (state == GameState.Respawning)
        {
            // ...
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName="New Menu", menuName = "Menu System/Menu")]
public class Menu : ScriptableObjectBase
{
    [SerializeField]
    GameObject menuPreFab;
    GameObject gameMenu;
    PlayerInput controls;

    public event Action OnOpenMenu;
    public event Action OnCloseMenu;

    public void HandleUpdate()
    {
        if(controls.Menu.Close.triggered)
        {
            Close();
        } 
    }
    public void Close()
    {
        OnCloseMenu?.Invoke();
        Destroy(gameMenu);
        gameMenu = null;
        controls = null;

    }

    public void OpenMenu(PlayerInput _controlHijack)
    {   
        controls = _controlHijack;
        OnOpenMenu?.Invoke();
        if(gameMenu == null)
            gameMenu = Instantiate(menuPreFab,Vector3.zero, Quaternion.identity);
    }
}

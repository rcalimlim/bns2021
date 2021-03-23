using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    // singleton
    private static RespawnManager instance;
    public static RespawnManager Instance { get { return instance; } }

    public event Action OnDeath;
    public event Action OnRevived;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }

        DontDestroyOnLoad (gameObject);
    }

    public void ActivateRespawnSequence()
    {
        Debug.Log("died and now should respawn");
        OnDeath?.Invoke();
    }

    private void HandleUpdate()
    {
        // nothing necessary
    }

}

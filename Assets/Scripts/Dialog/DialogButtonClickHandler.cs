using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogButtonClickHandler : MonoBehaviour
{
    public Action onAccept; 
    public Action onDecline; 

    // TODO: 
    void Start()
    {
        onAccept = () => Debug.Log("accept");        
        onDecline = () => Debug.Log("decline");        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnAccept()
    {
        onAccept();
    }
    public void OnDecline()
    {
        onDecline();
    }

    public void ResetActions()
    {
        onAccept = () => Debug.Log("accept");        
        onDecline = () => Debug.Log("decline");        
    }
}

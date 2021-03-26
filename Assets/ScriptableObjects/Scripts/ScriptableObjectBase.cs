using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct KeyValue
{
    public string key;
    public string value;
}

public class ScriptableObjectBase : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

}

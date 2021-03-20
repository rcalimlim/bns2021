using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScriptableObjectBase : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

}

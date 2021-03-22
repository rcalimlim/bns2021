using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIController : MonoBehaviour
{
    public int strength;
    public string type;

    Text strengthNW,strengthSE,typeTxt; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(Text text in GetComponentsInChildren<Text>())
        {
            switch(text.name)
            {
                case "StrengthNW": strengthNW = text; break;
                case "StrengthSE": strengthSE = text; break;
                case "Type": typeTxt = text; break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        strengthNW.text = strength.ToString();
        strengthSE.text = strength.ToString();
        typeTxt.text = type;
    }
}

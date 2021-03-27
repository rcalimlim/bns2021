using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIController : MonoBehaviour
{
    public int strength;
    public string type;

    public static Color Red {get => new Color(0.45f, 0.2f, 0.2f, 1.0f);}
    public static Color Blue {get => new Color(0f, 0.44f, 0.76f, 1f);}
    public static Color Black {get => new Color(0f,0f,0f,1f);}

    public Color color;
    Text strengthNW,strengthSE,typeTxt;
    Image background;
    
    public Button button;

    
    
    // Start is called before the first frame update
    void Awake(){
        foreach(Text text in GetComponentsInChildren<Text>())
        {
            switch(text.name)
            {
                case "StrengthNW": strengthNW = text; break;
                case "StrengthSE": strengthSE = text; break;
                case "Type": typeTxt = text; break;
            }
        }

        color = Black;
        button = GetComponentInChildren<Button>();
        background = GetComponentInChildren<Image>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   

        background.color = color;
        strengthNW.text = strength.ToString();
        strengthSE.text = strength.ToString();
        typeTxt.text = type;
    }
}

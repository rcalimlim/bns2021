using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName="New Armor", menuName="Item System/Armor")]
public class Armor: EquipableItem
{   
    [SerializeField]
    ArmorType type;

    [SerializeField]
    Sprite portrait;
    
    [SerializeField]
    List<Sprite> extraPortraits;

    public override string Type
    {
        get => Enum.GetName(typeof(ArmorType), type); 
    }

    public List<Sprite> ExtraPortraits {get => extraPortraits;}
    public Sprite Portrait {get => portrait;}
}

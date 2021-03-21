using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName="New Armor", menuName="Item System/Armor")]
public class Armor: EquipableItem
{   
    [SerializeField]
    ArmorType type;
    public override string Type
    {
        get => Enum.GetName(typeof(ArmorType), type); 
    }
}

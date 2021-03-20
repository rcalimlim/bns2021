using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName="New Armor", menuName="Item System/Armor")]
public class Armor: EquipableItem
{   
    [SerializeField]
    ArmorType type;
    public string Type
    {
        get { return Enum.GetName(typeof(WeaponType), type); }
    }
}

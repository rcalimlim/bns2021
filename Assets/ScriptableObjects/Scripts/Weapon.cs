using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item System/Weapon")]
public class Weapon: EquipableItem
{   
    [SerializeField]
    WeaponType weaponType;
    public override string Type
    {
        get => Enum.GetName(typeof(WeaponType), weaponType); 
    }
}

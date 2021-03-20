using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item System/Weapon")]
public class Weapon: EquipableItem
{   
    [SerializeField]
    WeaponType weaponType;
    public string Type
    {
        get { return Enum.GetName(typeof(WeaponType), weaponType); }
    }
}

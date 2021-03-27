using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item System/Weapon")]
public class Weapon: EquipableItem
{   
    [SerializeField]
    WeaponType weaponType;

    [SerializeField] 
    AttackKeyValue[] attackNames;

    [SerializeField]
    DefenseKeyValue[] defenseNames;

    Dictionary<string, string> skillNames;
    public Dictionary<string, string> SkillNames {get {
        if(skillNames == null || skillNames.Count == 0)
            populateDict();
        return skillNames;
    }}

    void populateDict() {
        // Default atk-def names
        skillNames = new Dictionary<string, string>();
        foreach(string attack in System.Enum.GetNames(typeof(Attack)))
            skillNames.Add(attack, attack);
        
        foreach(string defense in System.Enum.GetNames(typeof(Defense)))
            skillNames.Add(defense, defense);
        
        // overwrite the defaults with what was given
        foreach (AttackKeyValue keyValue in attackNames)
            skillNames[keyValue.key.ToString()] = keyValue.value;
        
        foreach (DefenseKeyValue keyValue in defenseNames)
            skillNames[keyValue.key.ToString()] = keyValue.value;
        
    }

    public override string Type
    {
        get => Enum.GetName(typeof(WeaponType), weaponType); 
    }
}

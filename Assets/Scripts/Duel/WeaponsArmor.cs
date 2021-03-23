using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special
{
    string name;
    string description;
    string recipe;
    bool expended;

    public Special(string name, string description, string recipe)
    {
        this.name = name;
        this.description = description;
        this.recipe = recipe;
    }
}

public class Equipment
{
    EquitmentRaiting rating;
    string name;
    string description;
    string details;
    Special special;

    public override string Rating
    {
        get => Enum.GetName(typeof(EquitmentRaiting), rating);
    }

    public string getSpecialName()
    {
        return this.special.name;
    }

    public string getSpecialDescription()
    {
        return this.special.description;
    }

    public string getSpecialRecipe()
    {
        return this.special.recipe;
    }

    public bool useSpecial()
    {
        if (this.special.expended == false) {
            this.special.expended = true;
            return true;
        }
        return false;
    }
}

public class DuelWeapon : Equipment
{
    WeaponType weaponType;

    public DuelWeapon(
            string name,
            WeaponType weaponType,
            EquitmentRaiting rating,
            string description,
            Special special)
    {
        this.name = name;
        this.weaponType = weaponType;
        this.description = description;
        this.special = special;
    }

    public DuelWeapon(DuelWeapon w) 
    {
        this.name = w.name;
        this.weaponType = w.weaponType;
        this.description = w.description;
        this.special = w.special;
        this.special.expended = false;
    }

    public override string Type
    {
        get => Enum.GetName(typeof(WeaponType), weaponType);
    }

}

public class DuelArmor : Equipment
{
    ArmorType armorType;

    public DuelArmor(
            string name,
            ArmorType armorType,
            EquitmentRaiting rating,
            string description,
            Special special)
    {
        this.name = name;
        this.armorType = armorType;
        this.description = description;
        this.special = special;
    }

    public DuelArmor(DuelArmor a)
    {
        this.name = a.name;
        this.armorType = a.armorType;
        this.description = a.description;
        this.special = a.special;
        this.special.expended = false;
    }

    public override string Type
    {
        get => Enum.GetName(typeof(WeaponType), weaponType);
    }
}

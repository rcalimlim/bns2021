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
        name = name;
        description = description;
        recipe = recipe;
    }
}

public class Equipment
{
    EquitmentRaiting rating;
    string name;
    string description;
    string details;
    Special special;

    public override string rating
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

public class Weapon : Equipment
{
    WeaponType weaponType;

    public Weapon(
            string name,
            WeaponType weaponType,
            EquitmentRaiting rating,
            string description,
            Special special)
    {
        this.name = name;
        this.weaponType = weaponType;
        string this.description = description;
        Special this.special = special;
    }

    public Weapon(Weapon w) 
    {
        this.name = w.name;
        this.weaponType = w.weaponType;
        string this.description = w.description;
        Special this.special = w.special;
    }

    public override string Type
    {
        get => Enum.GetName(typeof(WeaponType), weaponType);
    }

}

public class Armor : Equipment
{
    ArmorType armorType;

    public Armor(
            string name,
            ArmorType armorType,
            EquitmentRaiting rating,
            string description,
            Special special)
    {
        this.name = name;
        this.armorType = armorType;
        string this.description = description;
        Special this.special = special;
    }

    public Armor(Armor a)
    {
        this.name = a.name;
        this.armorType = a.armorType;
        string this.description = a.description;
        Special this.special = a.special;
    }

    public override string Type
    {
        get => Enum.GetName(typeof(WeaponType), weaponType);
    }
}

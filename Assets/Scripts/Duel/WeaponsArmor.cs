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

    // Unity assums private if no protection is listed. These are getters for those values
    public string Name {get=> name;}
    public string Description {get => description;}
    public string Recipe {get => recipe;}
    public bool Expended {
        get => expended;
        set => expended = value; // value is what ever is on the right hand side of the equation. See useSpecial()
    }
}

public class Equipment
{
    // These are nativly private so the child can not access them unless we
    // set them to protected
    protected EquitmentRaiting rating;
    protected string name;
    protected string description;
    protected string details;
    protected Special special;

    public string Name {
        get => name;
        set => name = value;}
    public string Description {get => description;}
    public string Details{get => details;}
     
    // removed the override because it is not overriding any method from its parent class
    public string Rating
    {
        get => Enum.GetName(typeof(EquitmentRaiting), rating);
    }
    
    /* wrote a set method in but you can't use this and the string one above it without different names
     // since rating is an Enum the above was written with the intention of only Read operations and not sets
     // since the upgrading of equipment was designed for outside of battle.
    public EquitmentRaiting Rating
    { set => rating = value; }
    */

    // You could do this if you wanted
    public string SpecialName{get => special.Name;}   
    public string getSpecialName()
    {
        return this.special.Name;
    }

    public string getSpecialDescription()
    {
        return this.special.Description;
    }

    public string getSpecialRecipe()
    {
        return this.special.Recipe;
    }

    public bool useSpecial()
    {
        // Using the Get/Set set in the Special class
        if (this.special.Expended == false) {
            this.special.Expended = true;
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
        this.special.Expended = false;
    }

    public string Type
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
        this.special.Expended = false;
    }

    public string Type
    {
        get => Enum.GetName(typeof(ArmorType), armorType);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special
{
    public string name;
    public string description;
    public string recipe;
    public bool expended;

    public Special(string name, string description, string recipe)
    {
        this.name = name;
        this.description = description;
        this.recipe = recipe;
    }
}

public class Equipment
{
    public EquitmentRaiting rating;
    public string name;
    public string description;
    public string details;
    public Special special;

    public string Rating
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
    public WeaponType weaponType;

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

    public string Type
    {
        get => Enum.GetName(typeof(WeaponType), weaponType);
    }

}

public class DuelArmor : Equipment
{
    public ArmorType armorType;

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

    public string Type
    {
        get => Enum.GetName(typeof(ArmorType), armorType);
    }
}

/*
public class WeaponsArmorTest
{
    static void main(string[] args)
    {
        string bs_name = "Buster Sword";
        WeaponType bs_wt = Heavy;
        EquitmentRaiting bs_rat = A;
        string bs_description = "A large broadsword that has inherited the hopes of those who fight.";
        Special bs_special = new Special(
                "Focused Thrust",
                "Lunge toward an enemy with a piercing strike that hits multiple times. Significantly increases stagger.",
                "1.opponent.hp.params.debuff=-20;1.opponent.defense.params.set=1"
                );
        DuelWeapon busterSword = new DuelWeapon(
                bs_name,
                bs_wt,
                bs_rat,
                bs_description,
                bs_special
                );
        Console.WriteLine(busterSword.name);
        Console.WriteLine(busterSword.description);
        Console.WriteLine(busterSword.Type);
        Console.WriteLine(busterSword.Rating);
        Console.WriteLine(busterSword.getSpecialName());
        Console.WriteLine(busterSword.getSpecialDescription());
        Console.WriteLine(busterSword.getSpecialRecipe());
        bool success = busterSword.useSpecial();
        Console.WriteLine("Success: " + success);
        
        DuelWeapon busterBarbell = new DuelWeapon(busterSword);
        busterBarbell.name = "A large steel barbell that has inherited the hopes of those who lift.";
        busterBarbell.rating = B;
        success = busterBarbell.useSpecial();
        Console.WriteLine("Success: " + success);


        string fg_name = "Fencing Gear";
        ArmorType fg_at = Parrying;
        EquitmentRaiting fg_rat = B;
        string fg_description = "Keeps you agile."; 
        Special fg_special = new Special(
                "Full Thrust",
                "All attacks become THRUST.",
                "1.self.attack.params.set=T.duration=3"
                );
        DuelArmor fg = new DuelArmor(
                fg_name,
                fg_at,
                fg_rat,
                fg_description,
                fg_special
                );
        Console.WriteLine(fg.name);
        Console.WriteLine(fg.description);
        Console.WriteLine(fg.Type);
        Console.WriteLine(fg.Rating);
        Console.WriteLine(fg.getSpecialName());
        Console.WriteLine(fg.getSpecialDescription());
        Console.WriteLine(fg.getSpecialRecipe());
        success = fg.useSpecial();
        Console.WriteLine("Success: " + success);
    }

}
*/

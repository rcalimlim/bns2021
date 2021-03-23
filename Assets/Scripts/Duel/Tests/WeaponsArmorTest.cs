using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponArmorTest
{

    static void main(string[] args)
    {
        string bs_name = "Buster Sword";
        WeaponType bs_wt = WeaponType.Heavy;
        EquitmentRaiting bs_rat = EquitmentRaiting.A;
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

        Console.WriteLine(busterSword.Name);
        Console.WriteLine(busterSword.Description);
        Console.WriteLine(busterSword.Type);
        Console.WriteLine(busterSword.Rating); 
        Console.WriteLine(busterSword.getSpecialName());
        Console.WriteLine(busterSword.getSpecialDescription());
        Console.WriteLine(busterSword.getSpecialRecipe());
        bool success = busterSword.useSpecial();
        Console.WriteLine("Success: " + success);
        
        DuelWeapon busterBarbell = new DuelWeapon(busterSword);
        busterBarbell.Name = "A large steel barbell that has inherited the hopes of those who lift.";
        //busterBarbell.Rating = EquitmentRaiting.B; // the Rating accessor is for a string and won't accept an Enum
        success = busterBarbell.useSpecial();
        Console.WriteLine("Success: " + success);


        string fg_name = "Fencing Gear";
        ArmorType fg_at = ArmorType.Parrying;
        EquitmentRaiting fg_rat = EquitmentRaiting.B;
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
        Console.WriteLine(fg.Name);
        Console.WriteLine(fg.Description);
        Console.WriteLine(fg.Type);
        Console.WriteLine(fg.Rating);
        Console.WriteLine(fg.getSpecialName());
        Console.WriteLine(fg.getSpecialDescription());
        Console.WriteLine(fg.getSpecialRecipe());
        success = fg.useSpecial();
        Console.WriteLine("Success: " + success);
    }
}


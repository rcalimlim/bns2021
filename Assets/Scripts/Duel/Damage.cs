using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{

    /* damage calculation */
    /* update cactapus */
    const int BASE_OFFSET = 10;
    const double BASE_ATTACK_FACTOR = 1.2;
    const double MAX_BASE_STACK = 35;
    // should be an integer in [0,10] independent of MAX_BASE_STACK
    const int EFFECTIVE_BASE_STACK = 0;
    // added separately, so should be based on MAX_BASE_STACK
    const double WEAPONTYPE_STACK = 0;
    const double WEAPON_STRAIGHT_STACK = WEAPONTYPE_STACK * 0.25;
    const double WEAPONCLASS_STACK_C = MAX_BASE_STACK * 0.0;
    const double WEAPONCLASS_STACK_B = MAX_BASE_STACK * 0.12;
    const double WEAPONCLASS_STACK_A = MAX_BASE_STACK * 0.24;
    const double WEAPONCLASS_STACK_S = MAX_BASE_STACK * 0.36;
    const double ARMORTYPE_STACK = MAX_BASE_STACK * 0.1;
    const double ARMOR_GUARD_STACK = ARMORTYPE_STACK * 0.5;
    const double ARMORCLASS_STACK_C = MAX_BASE_STACK * 0.00;
    const double ARMORCLASS_STACK_B = MAX_BASE_STACK * 0.06;
    const double ARMORCLASS_STACK_A = MAX_BASE_STACK * 0.12;
    const double ARMORCLASS_STACK_S = MAX_BASE_STACK * 0.18;

    // mult constant values (should default to 1)
    // ===
    const double BASE_EFFECTIVE_MULT = 1.25;
    const double WEAPONTYPE_MULT = 1.25;
    const double WEAPON_STRAIGHT_MULT = 1.075;
    const double WEAPONCLASS_MULT_C = 1;
    const double WEAPONCLASS_MULT_B = 1;
    const double WEAPONCLASS_MULT_A = 1;
    const double WEAPONCLASS_MULT_S = 1;
    const double ARMORTYPE_MULT = 1/1.3;
    const double ARMOR_GUARD_MULT = 1/1.05;
    const double ARMORCLASS_MULT_C = 1/1;
    const double ARMORCLASS_MULT_B = 1/1.05;
    const double ARMORCLASS_MULT_A = 1/1.1;
    const double ARMORCLASS_MULT_S = 1/1.15;
/*
    public int stackBase(
            Attack attackT,
            Defense defenseT,
            int attackS,
            int defenseS)
    {
        return NaN;
    }

    public int stackWeaponType(
            WeaponType weaponT,
            Attack attackT
            )
    {
        return NaN;
    }

    public int stackWeaponClass(EquitmentRaiting weaponR)
    {

    }

    public int stackArmorType(ArmorType armorT, Defense defenseT)
    {
    }
*/


    public int damage(
            Attack attackT,
            Defense defenseT,
            int attackS,
            int defenseS,
            WeaponType weaponT,
            EquitmentRaiting weaponR,
            ArmorType armorT,
            EquitmentRaiting armorR
            )
    {
        /*
        int stackEvent = 0;
        int multEvent = 1;
        double stackDmg = (
                     stackBase(attackT, defenseT, attackS, defenseS)
                     + stackWeaponType(weaponT, attackT)
                     + stackWeaponClass(weaponR)
                     - stackArmorType(armorT, defenseT)
                     - stackArmorClass(armorR)
                    );
        double multDmg = (
                     multBase(attackT, defenseT, attackS, defenseS)
                     * multWeaponType(weaponT, attackT)
                     * multWeaponClass(weaponR)
                     * multArmorType(armorT, defenseT)
                     * multArmorClass(armorR)
                );
        dmg = Math.Max(Math.Ceiling(stackDmg * multDmg), 1);
        return multNegate(attackT, defenseT) * dmg;
        */
        return 1; // not implemented
    }
}


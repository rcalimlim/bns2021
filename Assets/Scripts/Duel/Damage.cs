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

    public static bool isEffective(Attack attackT, Defense defenseT)
    {
        return (attackT == Attack.Strike && defenseT == Defense.Parry) ||
            (attackT == Attack.Lash && defenseT == Defense.Dodge) ||
            (attackT == Attack.Thrust && defenseT == Defense.Block);
    }

    public static bool isCounter(Attack attackT, Defense defenseT)
    {
        return (attackT == Attack.Strike && defenseT == Defense.Dodge) ||
            (attackT == Attack.Lash && defenseT == Defense.Block) ||
            (attackT == Attack.Thrust && defenseT == Defense.Parry);
    }

    public static bool isWeaponSpecialty(Attack attackT, WeaponType weaponT) {
        return (attackT == Attack.Strike && weaponT == WeaponType.Heavy) ||
            (attackT == Attack.Lash && weaponT == WeaponType.Curved) ||
            (attackT == Attack.Thrust && weaponT == WeaponType.Fencing);
    }

    public static bool isArmorSpecialty(Defense defenseT, ArmorType armorT) {
        return (defenseT == Defense.Parry && armorT == ArmorType.Parrying) ||
            (defenseT == Defense.Block && armorT == ArmorType.Blocking) ||
            (defenseT == Defense.Dodge && armorT == ArmorType.Dodging);
    }

    public static double stackBase(
            Attack attackT,
            Defense defenseT,
            int attackS,
            int defenseS)
    {
        int effectiveBonus = 0;
        if (isEffective(attackT,defenseT)) {
            effectiveBonus = EFFECTIVE_BASE_STACK;
        }
        return (attackS * BASE_ATTACK_FACTOR
                - defenseS
                + BASE_OFFSET
                + effectiveBonus)/10 * MAX_BASE_STACK;
    }

    public static double stackWeaponType(
            WeaponType weaponT,
            Attack attackT
            )
    {
        if (weaponT == WeaponType.Straight) {
            return WEAPON_STRAIGHT_STACK;
        }
        if (isWeaponSpecialty(attackT, weaponT)) {
            return WEAPONTYPE_STACK;
        }
        return 0;
    }

    public static double stackWeaponClass(EquitmentRaiting weaponR)
    {
        switch (weaponR) {
            case EquitmentRaiting.C: return WEAPONCLASS_STACK_C;
            case EquitmentRaiting.B: return WEAPONCLASS_STACK_B;
            case EquitmentRaiting.A: return WEAPONCLASS_STACK_A;
            case EquitmentRaiting.S: return WEAPONCLASS_STACK_S;
            default: return 0;
        }
    }

    public static double stackArmorType(
            ArmorType armorT,
            Defense defenseT
            )
    {
        if (armorT == ArmorType.Guarding) return ARMOR_GUARD_STACK;
        if (isArmorSpecialty(defenseT, armorT)) {
            return ARMORTYPE_STACK;
        }
        return 0;
    }

    public static double stackArmorClass(EquitmentRaiting armorR)
    {
        switch (armorR) {
            case EquitmentRaiting.C: return ARMORCLASS_STACK_C;
            case EquitmentRaiting.B: return ARMORCLASS_STACK_B;
            case EquitmentRaiting.A: return ARMORCLASS_STACK_A; 
            case EquitmentRaiting.S: return ARMORCLASS_STACK_S;
            default: return 0;
        }
    }

    public static int multNegate(Attack attackT, Defense defenseT)
    {
        if (isCounter(attackT, defenseT)) {
            return 0;
        }
        return 1;
    }

    public static double multBase(
            Attack attackT,
            Defense defenseT,
            int attackS,
            int defenseS
            )
    {
        if (isEffective(attackT, defenseT)) {
            return BASE_EFFECTIVE_MULT;
        }
        return 1;
    }

    public static double multWeaponType(
            WeaponType weaponT,
            Attack attackT
            )
    {
        if (weaponT == WeaponType.Straight) return WEAPON_STRAIGHT_MULT;
        if (isWeaponSpecialty(attackT, weaponT)) return WEAPONTYPE_MULT;
        return 1;
    }

    public static double multWeaponClass(EquitmentRaiting weaponR)
    {
        switch (weaponR) {
            case EquitmentRaiting.C: return WEAPONCLASS_MULT_C;
            case EquitmentRaiting.B: return WEAPONCLASS_MULT_B;
            case EquitmentRaiting.A: return WEAPONCLASS_MULT_A;
            case EquitmentRaiting.S: return WEAPONCLASS_MULT_S;
            default: return 1;
        }
    }

    public static double multArmorType(
            ArmorType armorT,
            Defense defenseT
            )
    {
        if (armorT == ArmorType.Guarding) return ARMOR_GUARD_MULT;
        if (isArmorSpecialty(defenseT, armorT)) {
            return ARMORTYPE_MULT;
        }
        return 1;
    }

    public static double multArmorClass(EquitmentRaiting armorR)
    {
        switch (armorR) {
            case EquitmentRaiting.C: return ARMORCLASS_MULT_C;
            case EquitmentRaiting.B: return ARMORCLASS_MULT_B;
            case EquitmentRaiting.A: return ARMORCLASS_MULT_A;
            case EquitmentRaiting.S: return ARMORCLASS_MULT_S;
            default: return 1;
        }
    }

    public static int damage(
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
        int dmg = (int)Math.Max(Math.Ceiling(stackDmg * multDmg), 1);
        return multNegate(attackT, defenseT) * dmg;
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{

    public bool RunTest = false;

    void Start() {
        if (RunTest) {
            testDamage();
        }
    }

    string ratingStr(EquitmentRaiting rating) {
        return Enum.GetName(typeof(EquitmentRaiting),rating);
    }

    string attackStr(Attack attack) {
        return Enum.GetName(typeof(Attack),attack);
    }

    string defenseStr(Defense defense) {
        return Enum.GetName(typeof(Defense),defense);
    }

    string weaponTypeStr(WeaponType wt) {
        return Enum.GetName(typeof(WeaponType),wt);
    }

    string armorTypeStr(ArmorType aT) {
        return Enum.GetName(typeof(ArmorType),aT);
    }

    void testDamage() {
        Attack[] attackTypes = {
            Attack.Strike,
            Attack.Thrust,
            Attack.Lash
        };
        Defense[] defenseTypes = {
            Defense.Dodge,
            Defense.Parry,
            Defense.Block
        };
        WeaponType[] weaponTypes = {
            WeaponType.Heavy,
            WeaponType.Fencing,
            WeaponType.Curved,
            WeaponType.Straight
        };
        ArmorType[] armorTypes = {
            ArmorType.Dodging,
            ArmorType.Blocking,
            ArmorType.Parrying,
            ArmorType.Guarding
        };
        EquitmentRaiting[] ratings = {
            EquitmentRaiting.C,
            EquitmentRaiting.B,
            EquitmentRaiting.A,
            EquitmentRaiting.S
        };
        int[] strengths = {1, 5, 9};
        //
        foreach (WeaponType wT in weaponTypes) {
            foreach (EquitmentRaiting wR in ratings) {
                Debug.LogFormat("Weapon {0} {1}", wT, wR);
                foreach (ArmorType aT in armorTypes) {
                    foreach (EquitmentRaiting aR in ratings) {
                        Debug.LogFormat("Armor {0} {1}", aT, aR);
                        foreach (Attack attack in attackTypes) {
                            foreach (Defense defense in defenseTypes) {
                                for (int aS = 1; aS < 10; aS += 4) {
                                    for (int dS = 1; dS < 10; dS += 4) {
                                        int dmg = Damage.damage(
                                                attack,
                                                defense,
                                                aS,
                                                dS,
                                                wT,
                                                wR,
                                                aT,
                                                aR
                                                );
                                        Debug.LogFormat(
                                                "{0}{1} vs {2}{3} : {4}",
                                                attackStr(attack),
                                                aS,
                                                defenseStr(defense),
                                                dS,
                                                dmg
                                                );
                                    }
                                }
                            } // foreach Defense
                        } // foreach Attack
                    } // foreach EquitmentRaiting equipment rating
                } // foreach ArmorType
            } // foreach EquitmentRaiting weapon rating
        } // foreach WeaponType
    } // testDamage
}

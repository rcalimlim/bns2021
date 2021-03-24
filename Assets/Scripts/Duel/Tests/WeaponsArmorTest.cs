using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsArmorTest : MonoBehaviour
{

    public bool RunTest = false;

    void Start() {
        Debug.LogFormat("chicken party");
        if (RunTest) {
            testDuelWeapon();
            testDuelArmor();
        }
    }

    void testDuelWeapon()
    {
        Debug.LogFormat("\nTesting creating a DuelWeapon from scratch\n***");
        string bs_name = "Buster Sword";
        WeaponType bs_wt = WeaponType.Heavy;
        EquitmentRaiting bs_rat = EquitmentRaiting.A;
        Debug.LogFormat("This should be A: {0}",Enum.GetName(typeof(EquitmentRaiting),bs_rat));
        string bs_description = "A large broadsword that has inherited the hopes of those who fight.";
        Special bs_special = new Special(
                "Focused Thrust",
                "Lunge toward an enemy with a piercing strike that hits multiple times. Significantly increases stagger."
                );
        List<Special> bss = new List<Special>();
        bss.Add(bs_special);
        DuelWeapon busterSword = new DuelWeapon(
                bs_name,
                bs_wt,
                bs_rat,
                bs_description,
                bss
                );
        Debug.LogFormat(
                "name: {0} desc: {1} type: {2} raiting: {3}",
                busterSword.name,
                busterSword.description,
                busterSword.Type,
                busterSword.Rating
                );
        foreach (Special s in busterSword.specials) {
            Debug.LogFormat("special: {0} {1} {2} {3}",
                    s.name,
                    s.description,
                    s.expended,
                    s.timing
                    );
        }
       
        Debug.LogFormat("\nTesting cloning a DuelWeapon from another DuelWeapon " +
                "then changing the attributes\n***");
        DuelWeapon busterBarbell = new DuelWeapon(busterSword);
        busterBarbell.name = "Buster Barbell";
        busterBarbell.description = "A large steel barbell that has inherited the hopes of those who lift.";
        Special bbFAIVES = new Special(
                "FAIVES",
                "FAIVES.",
                duration: 3,
                timing: 0
                );
        busterBarbell.specials = new List<Special>();
        busterBarbell.specials.Add(bbFAIVES);
        busterBarbell.rating = EquitmentRaiting.B;
        Debug.LogFormat(
                "name: {0} desc: {1} type: {2} raiting: {3}",
                busterBarbell.name,
                busterBarbell.description,
                busterBarbell.Type,
                busterBarbell.Rating
                );
        foreach (Special s in busterBarbell.specials) {
            Debug.LogFormat("special: {0} {1} {2} {3}",
                    s.name,
                    s.description,
                    s.expended,
                    s.timing
                    );
        }
    }

    void testDuelArmor()
    {
        string fg_name = "Fencing Gear";
        ArmorType fg_at = ArmorType.Parrying;
        EquitmentRaiting fg_rat = EquitmentRaiting.B;
        string fg_description = "Keeps you agile."; 
        Special fg_special = new Special(
                "Parry Reposte",
                "When you successfully counter your opponent, you cause damage.",
                duration: 0, 
                timing: 2
                );
        List<Special> fgs = new List<Special>();
        fgs.Add(fg_special);
        DuelArmor fg = new DuelArmor(
                fg_name,
                fg_at,
                fg_rat,
                fg_description,
                fgs
                );
        Debug.LogFormat(
                "name: {0} desc: {1} type: {2} raiting: {3}",
                fg.name,
                fg.description,
                fg.Type,
                fg.Rating
                );
        foreach (Special s in fg.specials) {
            Debug.LogFormat("special: {0} {1} {2} {3}",
                    s.name,
                    s.description,
                    s.expended,
                    s.timing
                    );
        }
    }
}


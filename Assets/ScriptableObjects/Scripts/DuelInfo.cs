using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct AttackKeyValue
{
    public Attack key;
    public string value;
}

[System.Serializable]
public struct DefenseKeyValue
{
    public Defense key;
    public string value;
}

[CreateAssetMenu(fileName="New DuelInfo", menuName = "Duel System/Duel Info")]
public class DuelInfo : ScriptableObjectBase
{
   
    [SerializeField]
    DuelID duelID;

    [SerializeField] Sprite enemyPortrait;
    [SerializeField] Sprite enemyAvatar;
    [SerializeField] Sprite duelTopBg;
    [SerializeField] Sprite duelBottomBg;
    [SerializeField] AudioClip[] backgroundMusics;

    [SerializeField]
    Weapon weapon;

    [SerializeField]
    Armor armor;
    
    [SerializeField] 
    AttackKeyValue[] attackNames;

    [SerializeField]
    DefenseKeyValue[] defenseNames;

    public DuelID DuelId {get => duelID;}
    public Sprite DuelTopBg {get => duelTopBg;}
    public Sprite DuelBottomBg {get => duelBottomBg;}
    public Dictionary<string, string> skillNames;
    public Dictionary<string, string> SkillNames {get {
        if(skillNames == null || skillNames.Count == 0)
            populateDict();
        return skillNames;
    }}
    public AudioClip[] BGM {get => backgroundMusics;}
    public Sprite Portrait {get=> enemyPortrait;}
    public Sprite Avatar {get => enemyAvatar;}
    public Weapon Weapon{get => weapon;}
    public Armor Armor{get => armor;}


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
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName="New Armor", menuName="Item System/Armor")]
public class Armor: EquipableItem
{   
    [SerializeField]
    ArmorType type;

    [SerializeField]
    Sprite portrait;

    [SerializeField]
    Sprite battleAvatar;
    
    [SerializeField]
    List<Sprite> extraPortraits;
    
    [SerializeField]
    List<Sprite> extraAvatars;

    public override string Type
    {
        get => Enum.GetName(typeof(ArmorType), type); 
    }

    public Sprite Avatar {get => battleAvatar;}
    public List<Sprite> ExtraAvatars {get => extraAvatars;}
    public List<Sprite> ExtraPortraits {get => extraPortraits;}
    public Sprite Portrait {get => portrait;}
}

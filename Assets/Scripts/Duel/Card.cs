using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attack
{
    Lash,
    Strike,
    Thrust
}
public enum Defense
{
    Block,
    Dodge,
    Parry,
}
public class Card
{
    private int strength;
    private Enum type;

    public Card(int _strength, Enum _type)
    {
        this.strength = _strength;
        this.type = _type;
    }
    
    /*
     * Getters
    */
    public int Strength{get => strength;}
    public string LongType{get => Enum.GetName(type.GetType(), type);}
    public string Type{get => Enum.GetName(type.GetType(), type).Substring(0,1);}
    public string Name{get => $"{Type}{Strength}";}
    public string LongName{get => $"{LongType} {Strength}";}

    /*
     * 
    */
    public string AllCardInfo()
    {
        return $"Card Info: \n\tName: {Name}\n\tLong Name: {LongName}\n\tStrength: {Strength}\n\tType Name: {Type}\n\tLong Type Name: {LongType}";
    }
}

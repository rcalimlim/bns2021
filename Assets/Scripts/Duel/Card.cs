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
    public string CardClass{
        get { return (type.GetType() == typeof(Attack))? "Attack" : "Defense";}
    }

    /*
     * Helper Methods
    */
    public string AllCardInfo()
    {
        return $"Card Info: \n\tName: {Name}\n\tLong Name: {LongName}\n\tStrength: {Strength}\n\tType Name: {Type}\n\tLong Type Name: {LongType}";
    }

    private static Enum GetRandomType(Type enumType)
    {
        Array enums = Enum.GetValues(enumType);
        return (Enum)enums.GetValue(UnityEngine.Random.Range(0, enums.Length));
    }

    /*
     * Generators - Please use these to create Cards. 
     * Note: typeStr is case sensitive
    */
    public static Card MakeAttackCard(int strength, string typeStr)
    {
        List<String> cardTypes = new List<string>(Enum.GetNames(typeof(Attack)));
        if(strength > 0 && strength < 10 && cardTypes.Contains(typeStr))
            return new Card(strength, (Attack) Enum.Parse(typeof(Attack), typeStr));
        return null;
    }

    public static Card MakeDefenseCard(int strength, string typeStr)
    {
        List<String> cardTypes = new List<string>(Enum.GetNames(typeof(Defense)));
        if(strength > 0 && strength < 10 && cardTypes.Contains(typeStr))
            return new Card(strength, (Defense) Enum.Parse(typeof(Defense), typeStr));
        return null;
    }

    // Random Generators
    public static Card MakeRandomAttackCard()
    {   
        Enum type = GetRandomType(typeof(Attack));
        return new Card(UnityEngine.Random.Range(1,10), type);
    }

    public static Card MakeRandomDefenseCard()
    {
        Enum type = GetRandomType(typeof(Defense));
        return new Card(UnityEngine.Random.Range(1,10), type);
    }
}
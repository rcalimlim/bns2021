using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponType 
{
    Straight,
    Heavy,
    Fencing,
    Curved
}

public enum ArmorType 
{
    Guarding,
    Parrying,
    Blocking,
    Dodging
}

public enum EquitmentRaiting
{
    C,
    B,
    A,
    S
}

[CreateAssetMenu(fileName="New Item", menuName = "Item System/Item")]
public class Item : ScriptableObjectBase
{
    [SerializeField]
    string description;
    [SerializeField] Sprite sprite;

    public string Description
    {
        get {return description;}
    }

    public Sprite Sprite
    {
        get {return sprite;}
    }
}

[System.Serializable]
public abstract class EquipableItem : Item
{
    [SerializeField]
    EquitmentRaiting rating;

    [SerializeField]
    bool upgradable = true;

    [SerializeField]
    List<Special> specials = new List<Special>();

    public List<Special> Specials {get => specials;}

    public abstract string Type{get;}

    public string Rating
    {
        get {
            switch(rating)
            {
                case EquitmentRaiting.C:
                    return "C";
                case EquitmentRaiting.B:
                    return "B";
                case EquitmentRaiting.A:
                    return "A";
                case EquitmentRaiting.S:
                    return "✩";
                default:
                    return "";
            }
        }
    }

    public bool CanBeUpgraded()
    {
        if (upgradable && rating != EquitmentRaiting.S)
        {
            return true;
        }
        return false;
    }

    public string GetUpgradeRating()
    {
        switch(rating)
        {
            case EquitmentRaiting.C:
                return "B";
            case EquitmentRaiting.B:
                return "A";
            case EquitmentRaiting.A:
                return "✩";
            default:
                return "";
        }
    }

    public void Upgrade()
    {
        if(upgradable)
        {
            switch(rating)
            {
                case EquitmentRaiting.C:
                    rating = EquitmentRaiting.B; 
                    break;
                case EquitmentRaiting.B:
                    rating = EquitmentRaiting.A;
                    break;
                case EquitmentRaiting.A:
                    rating = EquitmentRaiting.S;
                    break;
                case EquitmentRaiting.S:
                default:
                    break;
            }
        }
    }
}

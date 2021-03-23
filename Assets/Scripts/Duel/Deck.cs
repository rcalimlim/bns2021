using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Deck
{
    private Stack<Card> attackCards;
    private Stack<Card> defenseCards;
    
    public Deck() 
    {
        BuildDecks();
    }

    public int AttackCardsRemaining {get => attackCards.Count;}
    public int DefenseCardsRemaining {get => defenseCards.Count;}

    public void Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value; 
        }
    }

    public Card Draw(string cardType)
    {
        if(cardType.ToLower() == "attack")
            return DrawAttackCard();
        else if(cardType.ToLower() == "defense")
            return DrawDefenseCard();
        
        return null;
    }

    public Card DrawAttackCard()
    {   if(attackCards.Count > 0)
            return attackCards.Pop();
        return null;
    }

    public Card DrawDefenseCard()
    {
        if(defenseCards.Count > 0)
            return defenseCards.Pop();
        return null;
    }

    private void BuildDecks()
    {
        List<string> attackTypes = new List<string>(System.Enum.GetNames(typeof(Attack)));
        List<string> defenseTypes = new List<string>(System.Enum.GetNames(typeof(Defense)));

        List<Card> attacks = new List<Card>();
        List<Card> defenses = new List<Card>();

        for(int strength = 1; strength < 10; strength++)
        {
            for(int count = 0; count < 4; count++)
            {
                foreach (string type in attackTypes)
                    attacks.Add(Card.MakeAttackCard(strength, type));
                foreach (string type in defenseTypes)
                    defenses.Add(Card.MakeDefenseCard(strength, type));
            }
        }

        for(int strength = 3; strength < 7; strength++) 
        {
            foreach (string type in attackTypes)
                    attacks.Add(Card.MakeAttackCard(strength, type));
                foreach (string type in defenseTypes)
                    defenses.Add(Card.MakeDefenseCard(strength, type));
        }

        // Shuffling a deck of playing cards 7 times
        //  generally creates a true random shuffle
        for(int i = 0; i < 7; i++)
        {
            Shuffle(attacks);
            Shuffle(defenses);
        }
        

        attackCards = new Stack<Card>(attacks);
        defenseCards = new Stack<Card>(defenses);
    }

}

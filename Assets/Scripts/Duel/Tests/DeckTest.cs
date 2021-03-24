using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckTest : MonoBehaviour
{
    public bool RunTests = false;
    // Start is called before the first frame update
    void Start()
    {
        if(RunTests)
        {
            createDeck();
            testDraw();
        }
        
    }

    void createDeck()
    {
        Deck deck = new Deck();

        print($"# Attack Cards: {deck.AttackCardsRemaining} Expected: 120\n# Defense Cards: {deck.DefenseCardsRemaining} Expected: 120");
    }

    void testDraw()
    {
        Deck deck = new Deck();
        string output = "";
        while(deck.AttackCardsRemaining > 0 && deck.DefenseCardsRemaining >  0)
        {
            output += $"Attack Card {deck.Draw("Attack").Name} Defense Card {deck.Draw("Defense").Name}\n";
        }
        print(output);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    public bool RunTests = false;
    // Start is called before the first frame update
    void Start()
    {
        if(RunTests)
        {
            createCard();
            makeRandomCards();
            makeCard();
        }
    }


    void createCard()
    {
        Card attackCard = new Card(3, Attack.Lash);
        print("Attack Card - " + attackCard.AllCardInfo());

        Card defenseCard = new Card(5, Defense.Parry);
        print("Defense Card - " + defenseCard.AllCardInfo());
    }

    void makeRandomCards()
    {
        for(int i=0; i < 5; i++)
            print("Random Attack Card:" + Card.MakeRandomAttackCard().AllCardInfo());

        for(int i=0; i < 5; i++)
            print("Random Defense Card:" + Card.MakeRandomDefenseCard().AllCardInfo());
    }
    void makeCard()
    {
        print($"Attempt MakeAttackCard (success): {Card.MakeAttackCard(1, "Lash")?.AllCardInfo()}");
        print($"Attempt MakeAttackCard (success): {Card.MakeAttackCard(9, "Thrust")?.AllCardInfo()}");
        print($"Attempt MakeAttackCard (success): {Card.MakeAttackCard(5, "Strike")?.AllCardInfo()}");
        
        print($"Attempt MakeAttackCard (failure): {Card.MakeAttackCard(-5, "Lash")?.AllCardInfo()}");
        print($"Attempt MakeAttackCard (failure): {Card.MakeAttackCard(15, "Lash")?.AllCardInfo()}");
        print($"Attempt MakeAttackCard (failure): {Card.MakeAttackCard(5, "Block")?.AllCardInfo()}");
        print($"Attempt MakeAttackCard (failure): {Card.MakeAttackCard(5, "lash")?.AllCardInfo()}");

        print($"Attempt MakeDefenseCard (success): {Card.MakeDefenseCard(1, "Block")?.AllCardInfo()}");
        print($"Attempt MakeDefenseCard (success): {Card.MakeDefenseCard(9, "Dodge")?.AllCardInfo()}");
        print($"Attempt MakeDefenseCard (success): {Card.MakeDefenseCard(5, "Parry")?.AllCardInfo()}");

        print($"Attempt MakeDefenseCard (failure): {Card.MakeDefenseCard(0, "Parry")?.AllCardInfo()}");
        print($"Attempt MakeDefenseCard (failure): {Card.MakeDefenseCard(10, "Dodge")?.AllCardInfo()}");
        print($"Attempt MakeDefenseCard (failure): {Card.MakeDefenseCard(5, "parry")?.AllCardInfo()}");
        print($"Attempt MakeDefenseCard (failure): {Card.MakeDefenseCard(1, "Lash")?.AllCardInfo()}");
    }

}

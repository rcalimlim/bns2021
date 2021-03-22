using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        createCard();
    }


    void createCard()
    {
        Card attackCard = new Card(3, Attack.Lash);
        print("Attack Card - " + attackCard.AllCardInfo());

        Card defenseCard = new Card(5, Defense.Parry);
        print("Defense Card - " + defenseCard.AllCardInfo());

    }
}
